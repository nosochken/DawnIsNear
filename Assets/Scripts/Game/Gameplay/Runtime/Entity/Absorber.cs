namespace Game.Gameplay
{
    public class Absorber : IAbsorber
    {
        private Unit _unit;

        public Absorber(Unit unit)
        {
            _unit = unit;
        }
        
        public ITargetable Owner => _unit;

        internal void Subscribe()
        {
            _unit.AbsorbableDetector.Detected +=  OnDetected;
        }

        internal void Unsubscribe()
        {
            _unit.AbsorbableDetector.Detected -=  OnDetected;
        }
        
        private void OnDetected(IAbsorbable absorbable)
        {
            if (absorbable.Owner.CurrentSize <= Owner.CurrentSize)
                Absorb(absorbable);
        }
        
        private void Absorb(IAbsorbable absorbable)
        {
            _unit.Size.Increase(absorbable.Owner.CurrentSize);
            absorbable.BeAbsorbed();
        }
    }
}