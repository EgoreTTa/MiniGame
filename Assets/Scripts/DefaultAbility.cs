using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BaseMob))]
public class DefaultAbility : MonoBehaviour, IAbility
{
    [SerializeField] private StatesOfAbility _stateOfAbility;
    [SerializeField] private float _timeToSwing;
    [SerializeField] private float _timeToCasted;
    [SerializeField] private float _timeToRecovery;
    [SerializeField] private float _timeReload;
    [SerializeField] private float _radius;
    private BaseMob _owner;
    [SerializeField] private bool _isReady;
    public StatesOfAbility StateOfAbility => _stateOfAbility;

    private void Awake()
    {
        _owner = GetComponent<BaseMob>();
    }

    private void Ready()
    {
        _isReady = true;
    }

    private void IntoCasted()
    {
        _stateOfAbility = StatesOfAbility.Casted;
        Casted();
        Invoke(nameof(IntoRecovery), _timeToCasted);
    }

    private void IntoRecovery()
    {
        _stateOfAbility = StatesOfAbility.Recovery;
        Invoke(nameof(IntoStandby), _timeToRecovery);
    }

    private void Casted()
    {
        var casted = Physics2D.CircleCastAll(
            transform.position,
            _radius,
            Vector2.zero);
        var mobs = casted
            .Where(x =>
                x.transform.GetComponent<BaseMob>() != null
                &&
                x.transform.GetComponent<BaseMob>() != _owner)
            .Distinct()
            .Select(x => x.transform.GetComponent<BaseMob>())
            .ToArray();
        foreach (var mob in mobs)
        {
            mob.gameObject.AddComponent<ElementalEffect2>();
        }
    }

    private void IntoStandby()
    {
        _stateOfAbility = StatesOfAbility.Standby;
    }

    public void Cast()
    {
        if (_stateOfAbility != StatesOfAbility.Standby
            ||
            _isReady is false) return;

        _isReady = false;
        Invoke(nameof(Ready), _timeReload);
        Swing();
    }

    private void Swing()
    {
        _stateOfAbility = StatesOfAbility.Swing;
        Invoke(nameof(IntoCasted), _timeToSwing);
    }
}