using System;

namespace Game.Gameplay
{
    public interface IAbsorbable
    {
        public event Action<IAbsorbable> Absorbed;
        
        public ITargetable Owner { get; }
        
        public void BeAbsorbed();
    }
}