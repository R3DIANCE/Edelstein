﻿using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Spatial;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Repositories;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game
{
    public interface IField : IFieldPool, IStage<IField, IFieldObjUser>, IPhysicalSpace2D, IRepositoryEntry<int>
    {
        FieldTemplate Template { get; }

        IFieldSplit GetSplit(Point2D position);
        IFieldSplit[] GetEnclosingSplits(Point2D position);
        IFieldSplit[] GetEnclosingSplits(IFieldSplit split);

        IFieldPool GetPool(FieldObjType type);

        Task Enter(IFieldObjUser user, byte portal, Func<IPacket> getEnterPacket = null);
        Task Enter(IFieldObjUser user, string portal, Func<IPacket> getEnterPacket = null);

        Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket = null);
        Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket = null);
    }
}
