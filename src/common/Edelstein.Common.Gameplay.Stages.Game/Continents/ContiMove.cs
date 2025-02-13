using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Stages.Game.Objects;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Continents;
using Edelstein.Protocol.Gameplay.Stages.Game.Continents.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Util.Tickers;
using Microsoft.Extensions.Logging;
using Stateless;

namespace Edelstein.Common.Gameplay.Stages.Game.Continents;

public class ContiMove : AbstractFieldObjectPool, IContiMove, ITickable
{
    private readonly ILogger _logger;
    private readonly StateMachine<ContiMoveState, ContiMoveStateTrigger> _stateMachine;

    public ContiMove(
        ILogger<ContiMove> logger,
        IFieldManager fieldManager,
        IContiMoveTemplate template
    )
    {
        _logger = logger;
        Template = template;

        StartShipMoveField = fieldManager.Retrieve(template.StartShipMoveFieldID).Result!;
        WaitField = fieldManager.Retrieve(template.WaitFieldID).Result!;
        MoveField = fieldManager.Retrieve(template.MoveFieldID).Result!;
        if (template.CabinFieldID.HasValue)
            CabinField = fieldManager.Retrieve(template.CabinFieldID.Value).Result;
        EndField = fieldManager.Retrieve(template.EndFieldID).Result!;
        EndShipMoveField = fieldManager.Retrieve(template.EndShipMoveFieldID).Result!;

        var now = DateTime.UtcNow;

        NextBoarding = now
            .AddMinutes(now.Minute % Template.Term == 0
                ? 0
                : Template.Term - now.Minute % Template.Term)
            .AddMinutes(Template.Delay)
            .AddSeconds(-now.Second);

        _logger.LogDebug(
            "{Name} contimove is scheduled to board at {NextBoarding}",
            Template.Name, NextBoarding
        );

        ResetEvent();

        _stateMachine = new StateMachine<ContiMoveState, ContiMoveStateTrigger>(ContiMoveState.Dormant);

        _stateMachine
            .Configure(ContiMoveState.Dormant)
            .Permit(ContiMoveStateTrigger.Board, ContiMoveState.Wait);
        _stateMachine
            .Configure(ContiMoveState.Wait)
            .Permit(ContiMoveStateTrigger.Start, ContiMoveState.Move);
        _stateMachine
            .Configure(ContiMoveState.Move)
            .OnEntryFromAsync(ContiMoveStateTrigger.Start, async () =>
            {
                await Move(WaitField, MoveField);
                await StartShipMoveField.Dispatch(
                    new PacketWriter()
                        .WriteByte((byte)ContiMoveTarget.TargetStartShipMoveField)
                        .WriteByte((byte)ContiMoveStateTrigger.Start)
                );
            })
            .OnExitAsync(async () =>
            {
                await Move(MoveField, EndField);
                if (CabinField != null)
                    await Move(CabinField, EndField);

                await EndShipMoveField.Dispatch(
                    new PacketWriter()
                        .WriteByte((byte)ContiMoveTarget.TargetEndShipMoveField)
                        .WriteByte((byte)ContiMoveStateTrigger.End)
                );

                NextBoarding = NextBoarding.AddMinutes(Template.Term);
                ResetEvent();
            })
            .Permit(ContiMoveStateTrigger.MobGen, ContiMoveState.Event)
            .Permit(ContiMoveStateTrigger.End, ContiMoveState.Dormant);
        _stateMachine
            .Configure(ContiMoveState.Event)
            .SubstateOf(ContiMoveState.Move)
            .OnEntryAsync(async () =>
            {
                NextEvent = null;

                _logger.LogDebug(
                    "{Name} contimove started the event, ending at {NextEventEnd}",
                    template.Name, NextEventEnd
                );

                // TODO: Mobspawns

                await MoveField.Dispatch(
                    new PacketWriter()
                        .WriteByte((byte)ContiMoveTarget.TargetMoveField)
                        .WriteByte((byte)ContiMoveStateTrigger.MobGen)
                );
            })
            .OnExitAsync(async () =>
            {
                _logger.LogDebug(
                    "{Name} contimove ended the event",
                    template.Name
                );

                // TODO: Mobspawns

                await MoveField.Dispatch(
                    new PacketWriter()
                        .WriteByte((byte)ContiMoveTarget.TargetMoveField)
                        .WriteByte((byte)ContiMoveStateTrigger.MobDestroy)
                );
            })
            .Permit(ContiMoveStateTrigger.MobDestroy, ContiMoveState.Move);

        _stateMachine.OnTransitioned(t => _logger.LogDebug(
                "{Name} contimove state triggered {Trigger} and transitioned to {State}, next state change at {NextState}",
                Template.Name, t.Trigger, t.Destination, t.Trigger switch
                {
                    ContiMoveStateTrigger.Board => NextStart,
                    ContiMoveStateTrigger.Start => NextEvent ?? NextEnd,
                    ContiMoveStateTrigger.MobGen => NextEventEnd,
                    ContiMoveStateTrigger.MobDestroy => NextBoarding,
                    ContiMoveStateTrigger.End => NextBoarding,
                    _ => NextBoarding
                }
            )
        );
    }

    public int ID => Template.ID;
    public IContiMoveTemplate Template { get; }

    public ContiMoveState State => _stateMachine.State;

    public IField StartShipMoveField { get; }
    public IField WaitField { get; }
    public IField MoveField { get; }
    public IField? CabinField { get; }
    public IField EndField { get; }
    public IField EndShipMoveField { get; }


    public DateTime NextBoarding { get; private set; }
    public DateTime NextStart => NextBoarding.AddMinutes(Template.Wait);
    public DateTime NextEnd => NextStart.AddMinutes(Template.Required);
    public DateTime? NextEvent { get; private set; }
    public DateTime? NextEventEnd => NextBoarding.AddMinutes(Template.Wait).AddMinutes(Template.EventEnd);

    public override IReadOnlyCollection<IFieldObject> Objects => new[]
        {
            StartShipMoveField,
            WaitField,
            MoveField,
            CabinField,
            EndField,
            EndShipMoveField
        }
        .Where(f => f != null)
        .SelectMany(f => f!.Objects)
        .ToImmutableList();

    public override Task Enter(IFieldObject obj) => (State switch
    {
        ContiMoveState.Wait => WaitField,
        ContiMoveState.Move => MoveField,
        ContiMoveState.Event => MoveField,
        _ => StartShipMoveField
    }).Enter(obj);

    public override Task Leave(IFieldObject obj) =>
        WaitField.Enter(obj);

    public override IFieldObject? GetObject(int id) => Objects.FirstOrDefault(o => o.ObjectID == id);
    public override IEnumerable<IFieldObject> GetObjects() => Objects;

    public Task Trigger(ContiMoveStateTrigger trigger) => _stateMachine.FireAsync(trigger);

    public async Task OnTick(DateTime now)
    {
        if (_stateMachine.IsInState(ContiMoveState.Dormant))
            if (now > NextBoarding)
                await Trigger(ContiMoveStateTrigger.Board);

        if (_stateMachine.IsInState(ContiMoveState.Wait))
            if (now > NextStart)
                await Trigger(ContiMoveStateTrigger.Start);

        if (_stateMachine.IsInState(ContiMoveState.Move))
            if (now > NextEnd)
                await Trigger(ContiMoveStateTrigger.End);

        if (State == ContiMoveState.Move)
            if (NextEvent.HasValue && now > NextEvent.Value)
                await Trigger(ContiMoveStateTrigger.MobGen);

        if (State == ContiMoveState.Event)
            if (NextEventEnd.HasValue && now > NextEventEnd.Value)
                await Trigger(ContiMoveStateTrigger.MobDestroy);
    }

    private static Task Move(IField from, IField to) =>
        Task.WhenAll(from
            .GetObjects<IFieldUser>()
            .Select(u => to.Enter(u, 0)));

    private void ResetEvent()
    {
        var random = new Random(
            NextBoarding.Year +
            NextBoarding.Month +
            NextBoarding.Day +
            NextBoarding.Hour +
            NextBoarding.Minute
        );

        if (!Template.Event || random.Next(100) > 30) return;

        NextEvent = NextBoarding
            .AddMinutes(Template.Wait)
            .AddMinutes(random.Next(Template.Required - 5))
            .AddMinutes(2);
        _logger.LogDebug(
            "{Name} contimove event is scheduled at {NextEvent} to {NextEventEnd}",
            Template.Name, NextEvent, NextEventEnd
        );
    }
}
