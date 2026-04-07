using System;

namespace Game.Gameplay
{
    public interface IAbsorbable
    {
        public event Action<IAbsorbable> Absorbed;
        
        public EntityType Type { get; }
        
        public ISize Size { get; }

        public void BeAbsorbed();
    }
}