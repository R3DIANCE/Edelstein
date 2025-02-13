﻿using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects;

public abstract class AbstractFieldLife<TMovePath, TMoveAction> :
    AbstractFieldObject, IFieldLife<TMovePath, TMoveAction>
    where TMovePath : IMovePath<TMoveAction>
    where TMoveAction : IMoveAction
{
    protected AbstractFieldLife(TMoveAction action, IPoint2D position, IFieldFoothold? foothold = null) : base(position)
    {
        Action = action;
        Foothold = foothold;
    }

    public TMoveAction Action { get; protected set; }
    public IFieldFoothold? Foothold { get; private set; }

    public void SetPosition(IPoint2D position)
    {
        Position = position;
        Foothold = Field?.Template.Footholds
            .Find(Position)
            .FirstOrDefault(f => f.Line.Intersects(Position));
    }

    public Task Move(IPoint2D position)
    {
        Position = position;
        return UpdateFieldSplit();
    }

    public virtual async Task Move(TMovePath ctx)
    {
        if (Field == null) return;

        if (ctx.Action != null) Action = ctx.Action;
        if (ctx.Position != null) Position = ctx.Position;

        Foothold = Field.Template.Footholds
            .Find(Position)
            .FirstOrDefault(f => f.Line.Intersects(Position));

        await UpdateFieldSplit();

        if (FieldSplit != null)
            await FieldSplit.Dispatch(GetMovePacket(ctx), this);
    }

    private async Task UpdateFieldSplit()
    {
        var split = Field?.GetSplit(Position);

        if (split == null)
        {
            if (Field != null) await Field.Enter(this);
            return;
        }

        if (FieldSplit != split) await split.Enter(this);
    }

    protected abstract IPacket GetMovePacket(TMovePath ctx);
}
