﻿namespace Edelstein.Common.Gameplay.Social
{
    public enum PartyResultCode : byte
    {
        LoadParty_Done = 0x7,
        CreateNewParty_Done = 0x8,
        CreateNewParty_AlreayJoined = 0x9,
        CreateNewParty_Beginner = 0xA,
        CreateNewParty_Unknown = 0xB,
        WithdrawParty_Done = 0xC,
        WithdrawParty_NotJoined = 0xD,
        WithdrawParty_Unknown = 0xE,
        JoinParty_Done = 0xF,
        JoinParty_Done2 = 0x10,
        JoinParty_AlreadyJoined = 0x11,
        JoinParty_AlreadyFull = 0x12,
        JoinParty_OverDesiredSize = 0x13,
        JoinParty_UnknownUser = 0x14,
        JoinParty_Unknown = 0x15,
        InviteParty_Sent = 0x16,
        InviteParty_BlockedUser = 0x17,
        InviteParty_AlreadyInvited = 0x18,
        InviteParty_AlreadyInvitedByInviter = 0x19,
        InviteParty_Rejected = 0x1A,
        InviteParty_Accepted = 0x1B,
        KickParty_Done = 0x1C,
        KickParty_FieldLimit = 0x1D,
        KickParty_Unknown = 0x1E,
        ChangePartyBoss_Done = 0x1F,
        ChangePartyBoss_NotSameField = 0x20,
        ChangePartyBoss_NoMemberInSameField = 0x21,
        ChangePartyBoss_NotSameChannel = 0x22,
        ChangePartyBoss_Unknown = 0x23,
        AdminCannotCreate = 0x24,
        AdminCannotInvite = 0x25,
        UserMigration = 0x26,
        ChangeLevelOrJob = 0x27,
        SuccessToSelectPQReward = 0x28,
        FailToSelectPQReward = 0x29,
        ReceivePQReward = 0x2A,
        FailToRequestPQReward = 0x2B,
        CanNotInThisField = 0x2C,
        ServerMsg = 0x2D
    }
}
