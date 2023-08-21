using UnityEngine;

public class Health
{
    private BaseMob _owner;
    private GameObject _healthBy;
    private float _countHealth;

    public BaseMob Owner => _owner;
    public GameObject HealthBy => _healthBy;
    public float CountHealth => _countHealth;

    public Health(
        BaseMob owner,
        GameObject healthBy,
        float countHealth)
    {
        _owner = owner;
        _healthBy = healthBy;
        _countHealth = countHealth;
    }
}