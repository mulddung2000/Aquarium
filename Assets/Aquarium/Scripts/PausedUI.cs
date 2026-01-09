using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aquarium
{
    /// <summary>
    /// Paused UI를 관리하는 클래스
    /// </summary>
    public class PausedUI : MonoBehaviour
    {
        #region Variables
        [Header("UI")]
        [SerializeField] private GameObject paused;

        [Header("Scene")]
        [SerializeField] private string loadToScene = "MainMenu";
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            if (paused != null)
                paused.SetActive(false);

            Time.timeScale = 1f;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Toggle();
            }
        }
        #endregion

        #region Custom Method
        public void Toggle()
        {
            if (paused == null)
            {
                Debug.LogError("Paused UI 오브젝트가 연결되지 않았습니다.");
                return;
            }

            bool isOpen = !paused.activeSelf;
            paused.SetActive(isOpen);

            Time.timeScale = isOpen ? 0f : 1f;
        }

        public void MainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(loadToScene);
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        #endregion
    }
}
