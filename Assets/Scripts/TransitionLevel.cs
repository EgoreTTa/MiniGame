namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [RequireComponent(typeof(Collider2D))]
    public class TransitionLevel : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.isTrigger is false)
                if (collider.gameObject.GetComponent<Player>() is { } player)
                {
                    if (SceneManager.sceneCount == SceneManager.loadedSceneCount)
                        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                }
        }
    }
}