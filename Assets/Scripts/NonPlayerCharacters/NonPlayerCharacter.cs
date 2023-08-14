using TMPro.EditorUtilities;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour, IInteraction
{
    [SerializeField] private string _firstName;
    [SerializeField] private bool _isInteract;

    public string FirstName => _firstName;

    public bool IsInteract => _isInteract;

    public void Interact(BaseMob mob)
    {
        if (_isInteract == false) _isInteract = true;

        Debug.Log($"{_firstName} отвечает {mob.Firstname}");
    }
}