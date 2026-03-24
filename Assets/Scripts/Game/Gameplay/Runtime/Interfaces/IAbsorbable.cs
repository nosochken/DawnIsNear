using System;

namespace Game.Gameplay
{
    public interface IAbsorbable : IEventableAbsorbable
    {
        public IValuableSize Size { get; }

        public void BeAbsorbed();
    }
}