using System;

namespace Game.Gameplay
{
    public interface IAbsorbable : ITargetable
    {
        public event Action<IAbsorbable> Absorbed;
        
        public int Size { get; }
        public bool IsActive { get; }

        public void BeAbsorbed();
    }
}