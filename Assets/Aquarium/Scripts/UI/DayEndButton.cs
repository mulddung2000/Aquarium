using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Aquarium
{
    public class DayEndButton : MonoBehaviour
    {
        [SerializeField] private string targetSceneName = "Week01";

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (SceneFader.Instance == null)
            {
                SceneManager.LoadScene(targetSceneName);
                return;
            }

            SceneFader.Instance.FadeOut(() =>
            {
                SceneManager.LoadScene(targetSceneName);
            });
        }
    }
}
