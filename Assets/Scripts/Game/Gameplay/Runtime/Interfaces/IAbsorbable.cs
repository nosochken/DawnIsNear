using System;

namespace Game.Gameplay
{
    public interface IAbsorbable
    {
        public event Action<IAbsorbable> Absorbed;
        
        public EntityType Type { get; }
        public IBody Body { get; }

        public void BeAbsorbed();
        public void DecreaseByOneSize();

    }
}