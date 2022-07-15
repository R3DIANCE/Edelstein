﻿using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Packets;

public interface IPacketHandlerManager<TStageUser> where TStageUser : IStageUser
{
    void Add(IPacketHandler<TStageUser> handler);
    void Remove(IPacketHandler<TStageUser> handler);

    Task Process(TStageUser user, IPacketReader reader);
}
