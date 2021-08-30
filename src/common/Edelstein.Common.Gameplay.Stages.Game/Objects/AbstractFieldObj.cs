﻿using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects
{
    public abstract class AbstractFieldObj : IFieldObj
    {
        public abstract FieldObjType Type { get; }

        public int ID { get; set; }

        public IField Field { get; set; }
        public IFieldSplit FieldSplit { get; set; }

        public Point2D Position { get; set; }

        public async Task UpdateFieldSplit(
            Func<IPacket> getEnterPacket = null,
            Func<IPacket> getLeavePacket = null
        )
        {
            if (Field == null)
            {
                if (FieldSplit != null)
                    await FieldSplit.Leave(this);
                return;
            }

            var split = Field.GetSplit(Position);

            if (FieldSplit != split) await split.Enter(this, getEnterPacket, getLeavePacket);
        }

        public abstract IPacket GetEnterFieldPacket();
        public abstract IPacket GetLeaveFieldPacket();
    }
}
