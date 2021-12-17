﻿using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class CheckPasswordHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.CheckPassword;
        private readonly LoginStage _stage;

        public CheckPasswordHandler(LoginStage stage)
            => _stage = stage;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.LoggedOut);

        public override async Task Handle(LoginStageUser user, IPacketReader packet)
        {
            var username = packet.ReadString();
            var password = packet.ReadString();
            _ = packet.ReadBytes(16); // MachineID
            _ = packet.ReadInt(); // GameRoomClient
            _ = packet.ReadByte(); // GameStartMode
            _ = packet.ReadByte(); // Unknown1
            _ = packet.ReadByte(); // Unknown2
            _ = packet.ReadInt(); // PartnerCode

            var result = LoginResultCode.Success;
            var account = await _stage.AccountRepository.RetrieveByUsername(username);

            if (account == null)
            {
                if (_stage.Info.AutoRegister)
                {
                    account = new Account
                    {
                        Username = username,
                        Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password)
                    };

                    await _stage.AccountRepository.Insert(account);
                    _stage.Logger.LogInformation($"Created new account {account.Username} (ID: {account.ID})");
                }
                else result = LoginResultCode.NotRegistered;
            }
            else
            {
                var session = await _stage.SessionRegistry.DescribeByAccount(new DescribeSessionByAccountRequest { Account = account.ID });

                if (!BCrypt.Net.BCrypt.EnhancedVerify(password, account.Password))
                    result = LoginResultCode.IncorrectPassword;

                if (session.Session.State != SessionState.Offline)
                    result = LoginResultCode.AlreadyConnected;
            }

            var response = new UnstructuredOutgoingPacket(PacketSendOperations.CheckPasswordResult);

            response.WriteByte((byte)result);
            response.WriteByte(0); // Unknown1
            response.WriteInt(0); // Unknown2

            if (result == LoginResultCode.Success)
            {
                response.WriteInt(account.ID);
                response.WriteByte(account.Gender ?? 0);
                response.WriteByte((byte)account.GradeCode);
                response.WriteShort((short)account.SubGradeCode);
                response.WriteByte(0); // nCountryID
                response.WriteString(account.Username); // sNexonClubID
                response.WriteByte(0); // nPurchaseEXP
                response.WriteByte(0); // ChatUnblockReason
                response.WriteLong(0); // dtChatUnblockDate
                response.WriteLong(0); // dtRegisterDate
                response.WriteInt(4); // nNumOfCharacter
                response.WriteByte(1); // v44
                response.WriteByte(0); // sMsg

                response.WriteLong(user.Key);

                // user.State = account.Gender == null ? LoginState.SelectGender : LoginState.SelectWorld; TODO fix select gender related DC
                user.Account = account;
                user.State = LoginState.SelectWorld;

                await _stage.Enter(user);
            }

            await user.Dispatch(response);
        }
    }
}
