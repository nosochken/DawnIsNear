using UnityEngine;

namespace Game.Gameplay
{
    public interface ITargetable
    {
        public bool IsActive { get; }
        public ISizeData Size { get; }
        public Vector2 CurrentPosition { get; }
    }
}