namespace Assets.Scripts.Environments
{
    using System;
    using Interfaces;
    using Mobs.Player;
    using UnityEngine;

    public class DoorEnvironment : Environment, IInteraction
    {
        private enum StatesOfDoor
        {
            Opened,
            Closed
        }

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Collider2D _collider2D;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private StatesOfDoor _stateOfDoor;

        public bool IsInteract => false;

        public void Interact(Player player)
        {
            _stateOfDoor = _stateOfDoor == StatesOfDoor.Closed ? StatesOfDoor.Opened : StatesOfDoor.Closed;
            switch (_stateOfDoor)
            {
                case StatesOfDoor.Opened:
                {
                    _spriteRenderer.sprite = _sprites[1];
                    _collider2D.enabled = false;
                }
                    break;
                case StatesOfDoor.Closed:
                {
                    _spriteRenderer.sprite = _sprites[0];
                    _collider2D.enabled = true;
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}