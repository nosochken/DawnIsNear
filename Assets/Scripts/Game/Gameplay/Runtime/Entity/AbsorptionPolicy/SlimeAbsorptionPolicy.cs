namespace Game.Gameplay
{
    internal class SlimeAbsorptionPolicy : IAbsorptionPolicy
    {
        public bool CanAbsorb(IAbsorbable absorbable, ISize size = null)
        {
            return absorbable.Body.Size.Current > 0;
        }

        public void Absorb(IAbsorbable absorbable, Size size = null)
        {
            absorbable.DecreaseByOneSize();
            
            if (absorbable.Body.Size.Current <= 0)
                absorbable.BeAbsorbed();
        }
    }
}