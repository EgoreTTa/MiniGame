namespace Assets.Scripts.GUI
{
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private int _indexSceneStartGame;
        
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