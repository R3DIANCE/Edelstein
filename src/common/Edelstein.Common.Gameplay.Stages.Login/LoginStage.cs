﻿using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Login.Handlers;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Interop.Contracts;
using Edelstein.Protocol.Util.Ticks;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login
{
    public class LoginStage : AbstractServerStage<LoginStage, LoginStageUser, LoginStageConfig>, ILoginStage<LoginStage, LoginStageUser>
    {
        public ITemplateRepository<ItemTemplate> ItemTemplates { get; set; }
        public ILogger Logger { get; init; }

        public LoginStage(
            LoginStageConfig config,
            IServerRegistryService serverRegistryService,
            ISessionRegistryService sessionRegistry,
            IMigrationRegistryService migrationRegistryService,
            IAccountRepository accountRepository,
            IAccountWorldRepository accountWorldRepository,
            ICharacterRepository characterRepository,
            ITickerManager timerManager,
            IPacketProcessor<LoginStage, LoginStageUser> processor,
            ILogger<IStage<LoginStage, LoginStageUser>> logger,
            ITemplateRepository<ItemTemplate> itemTemplates
        ) : base(
            ServerStageType.Login,
            config,
            serverRegistryService,
            sessionRegistry,
            migrationRegistryService,
            accountRepository,
            accountWorldRepository,
            characterRepository,
            timerManager,
            processor
        )
        {
            Logger = logger;
            ItemTemplates = itemTemplates;

            processor.Register(new CheckPasswordHandler(this));
            processor.Register(new SetGenderHandler());
        }

        public override async Task Enter(LoginStageUser user)
        {
            var session = new SessionObject
            {
                Account = user.Account.ID,
                Server = ID,
                State = SessionState.LoggingIn
            };

            await SessionRegistry.UpdateSession(new UpdateSessionRequest { Session = session });
            await base.Enter(user);
        }
    }
}
