using UnityEngine;

namespace  Game.Gameplay
{
    public abstract class EntityConfigs : ScriptableObject
    {
        [SerializeField] private SizeData _size;
        [SerializeField] private MovementData _movement;
        
        public SizeData Size => _size;
        public MovementData Movement => _movement;
    }
}