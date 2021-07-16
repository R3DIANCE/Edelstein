﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Stages
{
    public interface IStage<TStage, TUser> : IPacketDispatcher
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        ICollection<TUser> Users { get; }

        Task Enter(TUser user);
        Task Leave(TUser user);
    }
}
