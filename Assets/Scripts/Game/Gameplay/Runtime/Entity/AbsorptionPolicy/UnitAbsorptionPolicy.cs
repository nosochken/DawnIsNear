namespace Game.Gameplay
{
    internal class UnitAbsorptionPolicy : IAbsorptionPolicy
    {
        public bool CanAbsorb(IAbsorbable absorbable, ISize size)
        {
            return absorbable.Body.Size.Current <= size.Current;
        }

        public void Absorb(IAbsorbable absorbable, Size size)
        {
            size.Increase(absorbable.Body.Size.Current);
            absorbable.BeAbsorbed();
        }
    }
}