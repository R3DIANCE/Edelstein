﻿namespace Edelstein.Common.Gameplay.Stages.Login.Types;

public enum LoginResult : byte
{
    ProcFail = byte.MaxValue,
    Success = 0x0,
    TempBlocked = 0x1,
    Blocked = 0x2,
    Abandoned = 0x3,
    IncorrectPassword = 0x4,
    NotRegistered = 0x5,
    DBFail = 0x6,
    AlreadyConnected = 0x7,
    NotConnectableWorld = 0x8,
    Unknown = 0x9,
    Timeout = 0xA,
    NotAdult = 0xB,
    AuthFail = 0xC,
    ImpossibleIP = 0xD,
    NotAuthorizedNexonID = 0xE,
    NoNexonID = 0xF,
    NotAuthorized = 0x10,
    InvalidRegionInfo = 0x11,
    InvalidBirthDate = 0x12,
    PassportSuspended = 0x13,
    IncorrectSSN2 = 0x14,
    WebAuthNeeded = 0x15,
    DeleteCharacterFailedOnGuildMaster = 0x16,
    NotagreedEULA = 0x17,
    DeleteCharacterFailedEngaged = 0x18,
    IncorrectSPW = 0x14,
    SamePasswordAndSPW = 0x16,
    SamePincodeAndSPW = 0x17,
    RegisterLimitedIP = 0x19,
    RequestedCharacterTransfer = 0x1A,
    CashUserCannotUseSimpleClient = 0x1B,
    DeleteCharacterFailedOnFamily = 0x1D,
    InvalidCharacterName = 0x1E,
    IncorrectSSN = 0x1F,
    SSNConfirmFailed = 0x20,
    SSNNotConfirmed = 0x21,
    WorldTooBusy = 0x22,
    OTPReissuing = 0x23,
    OTPInfoNotExist = 0x24
}
