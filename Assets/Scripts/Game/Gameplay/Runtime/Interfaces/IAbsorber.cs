namespace Game.Gameplay
{
    internal interface IAbsorber : ITargetable
    {
        public int Size { get; }
        public bool IsActive { get; }
    }
}