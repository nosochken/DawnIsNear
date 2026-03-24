using UnityEngine;

namespace Game.Gameplay
{
    public interface ITargetable
    {
        public bool IsActive { get; }
        public IValuableSize Size { get; }
        public Vector2 CurrentPosition { get; }
    }
}