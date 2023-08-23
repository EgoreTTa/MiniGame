namespace Assets.Scripts.NoMonoBehaviour
{
    using Enemies;
    using Items;

    public class ScoreCounter
    {
        private int _allScores;
        private int _scorePickedItem;
        private int _countPickedItem;
        private int _scoreKilledEnemy;
        private int _countKilledEnemy;
        private int _streak;

        public int AllScores => _allScores;
        public int ScorePickedItem => _scorePickedItem;
        public int CountPickedItem => _countPickedItem;
        public int ScoreKilledEnemy => _scoreKilledEnemy;
        public int CountKilledEnemy => _countKilledEnemy;
        public int Streak => _streak;

        public void PickItem(BaseItem item)
        {
            _allScores += item.ScoreCount * _streak;
            _scorePickedItem += item.ScoreCount;
            _countPickedItem++;
        }

        public void KilledEnemy(BaseMob mob)
        {
            _countKilledEnemy++;
        }

        public void ClearStreak()
        {
            _streak = 0;
        }
    }
}