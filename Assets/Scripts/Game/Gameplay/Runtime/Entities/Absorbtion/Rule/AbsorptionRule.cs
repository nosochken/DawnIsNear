namespace Game.Gameplay
{
    internal static class AbsorptionRule
    {
        internal static bool CanAbsorb(EntityType absorber, EntityType absorbable)
        {
            return absorber switch
            {
                EntityType.Player => absorbable is
                    EntityType.Enemy or
                    EntityType.Boss or
                    EntityType.Food or
                    EntityType.Midge,

                EntityType.Enemy => absorbable is
                    EntityType.Player or
                    EntityType.Enemy or
                    EntityType.Food,

                EntityType.Boss => absorbable is
                    EntityType.Player,

                EntityType.Slime => absorbable is
                    EntityType.Player,

                EntityType.Ball => absorbable is
                    EntityType.Player or
                    EntityType.Enemy,

                _ => false
            };
        }
    }
}