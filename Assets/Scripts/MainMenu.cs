namespace Assets.Scripts
{
    using System;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private SceneAsset _startGame;
        [SerializeField] private SceneAsset _options;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) NewGame();
            if (Input.GetKeyDown(KeyCode.Alpha2)) Options();
            if (Input.GetKeyDown(KeyCode.Alpha3)) Exit();
        }

        public void NewGame()
        {
            try
            {
                SceneManager.LoadScene(_startGame.name);
            }
            catch (Exception exception)
            {
                Debug.LogWarning(exception.Message);
            }
        }

        public void Options()
        {
            try
            {
                SceneManager.LoadScene(_options.name);
            }
            catch (Exception exception)
            {
                Debug.LogWarning(exception.Message);
            }
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}