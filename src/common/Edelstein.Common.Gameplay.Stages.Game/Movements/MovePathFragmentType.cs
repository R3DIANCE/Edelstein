﻿namespace Edelstein.Common.Gameplay.Stages.Game.Movements;

public enum MovePathFragmentType
{
    Normal = 0x0,
    Jump = 0x1,
    Impact = 0x2,
    Immediate = 0x3,
    Teleport = 0x4,
    RandomTeleport = 0x5,
    DemonTraceTeleport = 0x6,
    ReturnTeleport = 0x7,
    HangOnBack = 0x8,
    Assaulter = 0x9,
    Assassination = 0xa,
    Rush = 0xb,
    StatChange = 0xc,
    SitDown = 0xd,
    StartFallDown = 0xe,
    FallDown = 0xf,
    StartDragDown = 0x10,
    DragDown = 0x11,
    StartWings = 0x12,
    Wings = 0x13,
    AranAdjust = 0x14,
    MobToss = 0x15,
    MobTossSlowdown = 0x16,
    FlyingBlock = 0x17,
    DashSlide = 0x18,
    BMageAdjust = 0x19,
    BlinkLight = 0x1a,
    TeleportZero1 = 0x1b,
    FlashJump = 0x1c,
    DoubleJump = 0x1d,
    DoubleJumpDown = 0x1e,
    TripleJump = 0x1f,
    FlashJumpChangeEff = 0x20,
    RocketBooster = 0x21,
    BackstepShot = 0x22,
    CannonJump = 0x23,
    QuickSilverJump = 0x24,
    MobPowerKnockback = 0x25,
    VerticalJump = 0x26,
    CustomImpact = 0x27,
    CustomImpact2 = 0x28,
    CombatStep = 0x29,
    Hit = 0x2a,
    TimeBombAttack = 0x2b,
    SnowballTouch = 0x2c,
    BuffZoneEffect = 0x2d,
    LeafTornado = 0x2e,
    StylishRope = 0x2f,
    RopeConnect = 0x30,
    StrikerUppercut = 0x31,
    Crawl = 0x32,
    TeleportByMobSkillArea = 0x33,
    ZeroTag = 0x34,
    RetreatShot = 0x35,
    DbBladeAscension = 0x36,
    ImpactIgnoreMovePath = 0x37,
    AngleImpact = 0x38,
    StarPlanetRidingBooster = 0x39,
    UserToss = 0x3a,
    SlashJump = 0x3b,
    MobLadder = 0x3c,
    MobRightAngle = 0x3d,
    MobStopNodeStart = 0x3e,
    MobBeforeNode = 0x3f,
    MobTeleport = 0x40,
    MobAttackRush = 0x41,
    MobAttackRushStop = 0x42,
    MobAttackLeap = 0x43,
    BattlePVPMugongSomersault = 0x44,
    BattlePVPHelenaStepshot = 0x45,
    SunOfGlory = 0x46,
    HookshotStart = 0x47,
    Hookshot = 0x48,
    HookshotEnd = 0x49,
    PinkBeanPogoStick = 0x4a,
    PinkBeanPogoStickEnd = 0x4b,
    PinkbeanRollingAir = 0x4c,
    FinalToss = 0x4d,
    TeleportKinesis1 = 0x4e,
    NightlordShadowWeb = 0x4f,
    TeleportAran1 = 0x50,
    RwExplosionCannon = 0x51
}
