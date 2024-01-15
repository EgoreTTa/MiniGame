namespace Others
{
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    public class SmoothCamera : MonoBehaviour
    {
        [SerializeField] private GameObject _target;

        [Range(0, 1)]
        [SerializeField] private float _speed;

        private void Update()
        {
            if (_target != null)
                transform.position = Vector3.Lerp(
                    transform.position,
                    _target.transform.position,
                    _speed);
        }
    }
}