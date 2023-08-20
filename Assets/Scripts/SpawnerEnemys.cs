using UnityEngine;

public class SpawnerEnemys : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemys;
    [SerializeField] private float _intervalSpawn;
    private float _timerIntervalSpawn;
    [SerializeField] private Vector2 _leftTopPosition;
    [SerializeField] private Vector2 _rightDownPosition;

    [SerializeField] private float _coefficient;
    [SerializeField] private float _amplifyingFactor;

    [SerializeField] private float _freeZoneRadius;
    [SerializeField] private int _numberOfAttemptsSpawn;

    private void Update()
    {
        if (_timerIntervalSpawn < _intervalSpawn)
        {
            _timerIntervalSpawn += Time.deltaTime;
        }
        else
        {
            _timerIntervalSpawn -= _intervalSpawn;
            Spawn();
        }
    }

    private GameObject SelectForSpawn()
    {
        var indexItem = Random.Range(0, _enemys.Length);
        return _enemys[indexItem];
    }

    private bool SpawnEnemy(GameObject enemy)
    {
        for (int i = 0; i < _numberOfAttemptsSpawn; i++)
        {
            var positionForSpawn = new Vector3(
                Random.Range(
                    _leftTopPosition.x,
                    _rightDownPosition.x),
                Random.Range(
                    _leftTopPosition.y,
                    _rightDownPosition.y));
            if (CheckedFreeZoneForSpawn(positionForSpawn))
            {
                Instantiate(enemy, positionForSpawn, Quaternion.identity);
                return true;
            }
        }

        return false;
    }

    private void Spawn()
    {
        var enemy = SelectForSpawn();

        var successfulSpawn = SpawnEnemy(enemy);

        if (successfulSpawn)
        {
            _coefficient *= _amplifyingFactor;
        }
    }

    private bool CheckedFreeZoneForSpawn(Vector3 position)
    {
        var casted = Physics2D.CircleCastAll(position, _freeZoneRadius, Vector2.zero);

        return casted.Length is 0;
    }
}