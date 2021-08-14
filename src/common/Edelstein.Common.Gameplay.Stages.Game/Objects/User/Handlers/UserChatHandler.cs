﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class UserChatHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.UserChat;

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            _ = packet.ReadInt();
            var message = packet.ReadString();
            var onlyBalloon = packet.ReadBool();

            var chatPacket1 = new UnstructuredOutgoingPacket(PacketSendOperations.UserChat);

            chatPacket1.WriteInt(user.ID);
            chatPacket1.WriteBool(false); // TODO: gm chat
            chatPacket1.WriteString(message);
            chatPacket1.WriteBool(onlyBalloon);

            await user.FieldSplit.Dispatch(chatPacket1);

            if (onlyBalloon) return;

            var chatPacket2 = new UnstructuredOutgoingPacket(PacketSendOperations.UserChatNLCPQ);

            chatPacket2.WriteInt(user.ID);
            chatPacket2.WriteBool(false); // TODO: gm chat
            chatPacket2.WriteString(message);
            chatPacket2.WriteBool(onlyBalloon);
            chatPacket2.WriteString(user.Character.Name);

            await Task.WhenAll(user.Field
                .GetUsers()
                .Except(user.FieldSplit.GetWatchers())
                .Select(u => u.Dispatch(chatPacket2)));
        }
    }
}
