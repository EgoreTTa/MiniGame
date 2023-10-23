namespace Assets.Scripts.Environments
{
    using Interfaces;
    using Mobs.Player;
    using UnityEngine;

    public class ChestEnvironment : Environment, IInteraction
    {
        private bool _isInteract;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite[] _sprites;

        public bool IsInteract => _isInteract;

        public void Interact(Player player)
        {
            _isInteract = !_isInteract;
            _spriteRenderer.sprite = _isInteract ? _sprites[1] : _sprites[0];
        }
    }
}
