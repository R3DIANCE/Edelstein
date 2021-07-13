﻿using Edelstein.Protocol.Util.Repositories;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Space
{
    public interface IPhysicalSpawnPoint2D : IRepositoryEntry<int>
    {
        Point2D Position { get; }
    }
}
