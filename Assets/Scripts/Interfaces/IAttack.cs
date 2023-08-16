internal interface IAttack
{
    public StatesOfAttack StateOfAttack { get; }

    void Attack();
}