namespace Assets.Scripts.Interfaces
{
    public interface IEffect
    {
        public string EffectName { get; }
        public bool IsActive { get; set; }
    }
}