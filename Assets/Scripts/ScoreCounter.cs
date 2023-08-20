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
        _allScores += mob.Scorer.AllScores * (_streak + 1);
        _scoreKilledEnemy += mob.Scorer.AllScores;
        _countKilledEnemy++;
    }

    public void ClearStreak()
    {
        _streak = 0;
    }
}