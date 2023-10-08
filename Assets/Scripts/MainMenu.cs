namespace Assets.Scripts
{
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private int _indexSceneStartGame;

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
                SceneManager.LoadSceneAsync(_indexSceneStartGame);
            }
            catch (Exception exception)
            {
                Debug.LogWarning(exception.Message);
            }
        }

        public void Options()
        {
            Debug.Log("Options");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}