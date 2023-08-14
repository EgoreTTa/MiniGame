using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour, IInteraction
{
    [SerializeField] private string _firstName;
    private bool _isInteract;

    public string FirstName => _firstName;

    public bool IsInteract => _isInteract;

    public void Interact(BaseMob mob)
    {
        Debug.Log($"{_firstName} отвечает {mob.Firstname}");
    }
}