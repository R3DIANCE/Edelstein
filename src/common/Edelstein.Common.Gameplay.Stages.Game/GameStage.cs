﻿using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Handlers;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Continent;
using Edelstein.Protocol.Gameplay.Stages.Game.FieldSets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates.Options;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates.Sets;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Util.Ticks;
using Microsoft.Extensions.Logging;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public class GameStage : AbstractServerStage<GameStage, GameStageUser, GameStageConfig>, IGameStage<GameStage, GameStageUser>
    {
        public int WorldID => Config.WorldID;
        public int ChannelID => Config.ChannelID;

        public ITemplateRepository<ItemTemplate> ItemTemplates { get; }
        public ITemplateRepository<ItemOptionTemplate> ItemOptionTemplates { get; }
        public ITemplateRepository<ItemSetTemplate> ItemSetTemplates { get; }

        public ITemplateRepository<FieldTemplate> FieldTemplates { get; }

        public ITemplateRepository<NPCTemplate> NPCTemplates { get; }

        public IFieldRepository FieldRepository { get; }
        public IFieldSetRepository FieldSetRepository { get; }
        public IContiMoveRepository ContiMoveRepository { get; }

        private readonly FieldTemplate template;
        private readonly IField field;

        public GameStage(
            GameStageConfig config,
            ILogger<IStage<GameStage, GameStageUser>> logger,
            IServerRegistryService serverRegistryService,
            ISessionRegistryService sessionRegistry,
            IMigrationRegistryService migrationRegistryService,
            IAccountRepository accountRepository,
            IAccountWorldRepository accountWorldRepository,
            ICharacterRepository characterRepository,
            ITickerManager timerManager,
            IPacketProcessor<GameStage, GameStageUser> processor,
            ITemplateRepository<ItemTemplate> itemTemplates,
            ITemplateRepository<ItemOptionTemplate> itemOptionTemplates,
            ITemplateRepository<ItemSetTemplate> itemSetTemplates,
            ITemplateRepository<FieldTemplate> fieldTemplates,
            ITemplateRepository<NPCTemplate> npcTemplates,
            IFieldRepository fieldRepository,
            IFieldSetRepository fieldSetRepository,
            IContiMoveRepository contiMoveRepository
        ) : base(
            ServerStageType.Game,
            config,
            logger,
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
            ItemTemplates = itemTemplates;
            ItemOptionTemplates = itemOptionTemplates;
            ItemSetTemplates = itemSetTemplates;
            FieldTemplates = fieldTemplates;
            FieldRepository = fieldRepository;
            FieldSetRepository = fieldSetRepository;
            ContiMoveRepository = contiMoveRepository;
            NPCTemplates = npcTemplates;

            processor.Register(new UserTransferChannelRequestHandler());
            processor.Register(new UserMoveHandler());
            processor.Register(new UserEmotionHandler());
            processor.Register(new UserChatHandler());
            processor.Register(new UserGatherItemRequestHandler());
            processor.Register(new UserSortItemRequestHandler());
            processor.Register(new UserChangeSlotPositionRequestHandler());

            processor.Register(new NPCMoveHandler());

            template = FieldTemplates.Retrieve(310000000).Result;
            field = new Field(this, template);

            template.Life.ForEach(l => {
                if (l.Type != FieldLifeType.NPC) return;

                var npcTemplate = NPCTemplates.Retrieve(l.TemplateID).Result;
                var npc = new FieldObjNPC(npcTemplate, l.Left)
                {
                    Position = l.Position,
                    Foothold = field.GetFoothold(l.FH),
                    RX0 = l.RX0,
                    RX1 = l.RX1
                };

                field.Enter(npc);
            });
        }

        public override async Task Enter(GameStageUser user)
        {
            await base.Enter(user);

            /*
            var template = await FieldTemplates.Retrieve(user.Character.FieldID);
            var field = new Field(this, template);
            */
            var fieldUser = new FieldObjUser(user);

            user.FieldUser = fieldUser;
            await field.Enter(fieldUser);
        }

        public override async Task Leave(GameStageUser user)
        {
            if (user.Field != null)
                await user.Field.Leave(user.FieldUser);

            await base.Leave(user);
        }
    }

}
