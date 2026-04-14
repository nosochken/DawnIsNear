namespace Game.Gameplay
{
    internal interface IAbsorptionPolicy
    {
        public bool CanAbsorb(IAbsorbable absorbable, ISize size = null);
        public void Absorb(IAbsorbable absorbable, Size size = null);
    }
}