using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Common.Spawning
{
    public interface ISpawnObject
    {
        public void Spawn();
        public void Despawn();
        public void ReSpawn();
        public void PreSpawnConfigure();
        public void PostSpawnConfigure();
    }
}