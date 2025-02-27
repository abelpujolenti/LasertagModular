using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainScreen
{
    public class LoadSceneButton : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void LoadScene()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}