﻿using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Common.Services.Server.Contracts;

public record SessionResponse(SessionResult Result) : ISessionResponse;
