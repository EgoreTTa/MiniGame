namespace Assets.Scripts.Others
{
    using Enums;
    using JetBrains.Annotations;
    using UnityEngine;

    public class DayTime : MonoBehaviour
    {
        private const int PartOfDayForSecond = 21600;
        private const int SecondsOfDay = 86400;
        [SerializeField] private DayParts _dayTime;
        private float _time;

        public DayParts PartOfDay => _dayTime;

        [UsedImplicitly]
        private void Update()
        {
            _time += Time.deltaTime * 10000;

            switch ((int)_time / PartOfDayForSecond)
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
                    _time -= SecondsOfDay;
                    break;
            }
        }
    }
}