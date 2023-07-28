using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Player _player1;
    [SerializeField] private Player _player2;

    [SerializeField] private float _timeToFight;
    private float _timerTimeToFight;

    // private void Fight(
    //     Player player1,
    //     Player player2)
    // {
    //     if (_player1 != null
    //         &&
    //         _player2 != null)
    //     {
    //         player1.Attack(player2);
    //         if (player2.Live is false)
    //         {
    //             Final();
    //             return;
    //         }
    //
    //         player2.Attack(player1);
    //         if (player1.Live is false)
    //         {
    //             Final();
    //             return;
    //         }
    //     }
    //     else
    //     {
    //         enabled = false;
    //     }
    // }
    //
    // private void Update()
    // {
    //     if (_timerTimeToFight < _timeToFight)
    //     {
    //         _timerTimeToFight += Time.fixedDeltaTime;
    //     }
    //     else
    //     {
    //         _timerTimeToFight -= _timeToFight;
    //         Fight(_player1, _player2);
    //     }
    // }
    //
    // private void Final()
    // {
    //     if (_player1.Live)
    //         Debug.Log($"{_player1.Firstname} победил");
    //     if (_player2.Live)
    //         Debug.Log($"{_player2.Firstname} победил");
    // }
}