using System;
using UnityEngine;

namespace Game.Gameplay
{
    public class Body : MonoBehaviour, IBody
    {
        private Size _size;

        public bool IsActive => isActiveAndEnabled;
        public ISize Size => _size;
        public Vector2 CurrentPosition => transform.position;

        internal void Initialize(Size size)
        {
            _size = size ?? throw new ArgumentNullException(nameof(size));
        }
    }
}