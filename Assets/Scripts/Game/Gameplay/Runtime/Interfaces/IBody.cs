using UnityEngine;

namespace Game.Gameplay
{
    public interface IBody
    {
        public bool IsActive { get; }
        public ISize Size { get; }
        public Vector2 CurrentPosition { get; }
    }
}