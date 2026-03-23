using UnityEngine;

namespace Game.Gameplay
{
    public class Body
    {
        public Size Size { get; private set; }
        public Vector2 Position => _transform.position;
        public bool IsActive => _behaviour.isActiveAndEnabled;

        private Transform _transform;
        private Behaviour _behaviour;
        
        internal Body(int minSize, Transform transform, Behaviour behaviour)
        {
            Size = new Size(minSize);
            _transform = transform;
            _behaviour = behaviour;
        }
    }
}