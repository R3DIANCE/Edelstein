﻿using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class CheckDuplicatedIDHandler : AbstractLoginPacketHandler
{
    private IPipeline<ICharacterCheckDuplicatedID> _pipeline;

    public CheckDuplicatedIDHandler(IPipeline<ICharacterCheckDuplicatedID> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.CheckDuplicatedID;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectCharacter;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var name = reader.ReadString();

        return _pipeline.Process(new CharacterCheckDuplicatedID(user, name));
    }
}
