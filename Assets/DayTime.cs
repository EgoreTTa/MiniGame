using UnityEngine;

public class DayTime : MonoBehaviour
{
    [SerializeField] private DayParts _dayTime;
    private float _time;

    public DayParts PartOfDay
    {
        get => _dayTime;
    }

// Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime*10000;
        switch ((int)_time / 21600)
        {
            case 0:
                _dayTime = DayParts.Night;
                break;
            case 1:
                _dayTime = DayParts.Morning;
                break;
            case 2:
                _dayTime = DayParts.Afternoon;
                break;
            case 3:
                _dayTime = DayParts.Evening;
                break;
            default:
                _time -= 86400;
                break;
        }
        Debug.Log(_dayTime);
    }
}