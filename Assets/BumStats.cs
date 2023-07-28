using UnityEngine;

public class BumStats
{
    private int _allScores = 0;
    private int _scorePickedItem = 5;
    private int _scoreKilledEnemy = 20;
    private int _pickedItem = 0;
    private int _killedEnemies = 0;
    private int _streak = 0;

    public int ScorePickedItem => _scorePickedItem;
    public int ScoreKilledEnemy => _scoreKilledEnemy;
    public int PickedItem => _pickedItem;
    public int KilledEnemies => _killedEnemies;
    public int Streak => _streak;
    public int AllScores => _allScores;

    public void PickItem()
    {
        _allScores += _scorePickedItem;
        _pickedItem++;
    }
    
    public void KilledEnemy()
    {
        _allScores += _scoreKilledEnemy + _streak;
        _killedEnemies++;
        _streak++;
    }

    public void ClearStreak()
    {
       _streak = 0;
    }
}
