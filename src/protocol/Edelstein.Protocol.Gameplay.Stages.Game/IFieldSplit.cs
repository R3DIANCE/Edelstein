﻿using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Protocol.Gameplay.Stages.Game;

public interface IFieldSplit : IFieldObjectPool
{
    IReadOnlyCollection<IFieldSplitObserver> Observers { get; }

    int Row { get; }
    int Col { get; }

    Task Enter(IFieldObject obj, Func<IPacket>? getEnterPacket = null, Func<IPacket>? getLeavePacket = null);
    Task Leave(IFieldObject obj, Func<IPacket>? getLeavePacket = null);

    Task MigrateIn(IFieldObject obj);
    Task MigrateOut(IFieldObject obj);

    Task Observe(IFieldSplitObserver observer);
    Task Unobserve(IFieldSplitObserver observer);
}
