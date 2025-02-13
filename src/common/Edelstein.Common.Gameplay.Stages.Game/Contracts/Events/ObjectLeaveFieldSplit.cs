using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Contracts.Events;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;

namespace Edelstein.Common.Gameplay.Stages.Game.Contracts.Events;

public record ObjectLeaveFieldSplit(
    IFieldObject Object,
    IFieldSplit FieldSplit
) : IObjectLeaveFieldSplit;
