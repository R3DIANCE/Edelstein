﻿using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;

namespace Edelstein.Protocol.Services.Migration.Types;

public interface IMigration
{
    int AccountID { get; }
    int CharacterID { get; }

    string FromServerID { get; }
    string ToServerID { get; }

    long Key { get; }

    IAccount Account { get; }
    IAccountWorld AccountWorld { get; }
    ICharacter Character { get; }
}
