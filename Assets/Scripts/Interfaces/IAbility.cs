public interface IAbility
{
    public StatesOfAbility StateOfAbility { get; }
    
    public void Cast();
}