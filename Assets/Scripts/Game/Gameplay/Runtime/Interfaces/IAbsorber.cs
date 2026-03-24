namespace Game.Gameplay
{
    internal interface IAbsorber
    {
        public bool IsActive { get; }
        public int CurrentSize { get; }
        public Vector2 CurrentPosition { get; }
    }
}