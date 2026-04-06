using System;

namespace Game.Gameplay
{
    public interface IAbsorbable
    {
        public event Action<IAbsorbable> Absorbed;
        
        public ISize Size { get; }

        public void BeAbsorbed();
    }
}