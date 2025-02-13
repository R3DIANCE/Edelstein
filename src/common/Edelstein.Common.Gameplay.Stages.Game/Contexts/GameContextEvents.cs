﻿using Edelstein.Protocol.Gameplay.Stages.Contracts.Events;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Game.Contracts.Events;
using Edelstein.Protocol.Util.Events;

namespace Edelstein.Common.Gameplay.Stages.Game.Contexts;

public record GameContextEvents(
    IEvent<IUserEnterStage<IGameStageUser>> UserEnterStage,
    IEvent<IUserLeaveStage<IGameStageUser>> UserLeaveStage,
    IEvent<IObjectEnterField> ObjectEnterField,
    IEvent<IObjectLeaveField> ObjectLeaveField,
    IEvent<IObjectEnterFieldSplit> ObjectEnterFieldSplit,
    IEvent<IObjectLeaveFieldSplit> ObjectLeaveFieldSplit
) : IGameContextEvents;
