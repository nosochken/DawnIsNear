using UnityEngine;

namespace Game.Gameplay
{
    public interface ITargetable
    {
        public bool IsActive { get; }
        public int CurrentSize { get; }
        public Vector2 CurrentPosition { get; }
    }

}