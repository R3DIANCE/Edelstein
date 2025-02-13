﻿using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;

public interface ISocketOnPacket<out TStageUser> : IStageUserContract<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    IPacket Packet { get; }
}
