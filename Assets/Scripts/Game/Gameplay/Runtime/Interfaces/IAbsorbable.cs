using System;

namespace Game.Gameplay
{
    public interface IAbsorbable : IAbsorbableEvents
    {
        public ISizeData Size { get; }

        public void BeAbsorbed();
    }
}