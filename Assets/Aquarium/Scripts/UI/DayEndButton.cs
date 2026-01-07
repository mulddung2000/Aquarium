using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Aquarium
{
    public class DayEndButton : MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;

            // 🔥 빌드 인덱스 초과 방지 (프로토타입 안전장치)
            if (nextIndex >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogWarning("[DayEndButton] Next scene index out of range");
                return;
            }

            if (SceneFader.Instance != null)
            {
                SceneFader.Instance.FadeOut(() =>
                {
                    SceneManager.LoadScene(nextIndex);
                });
            }
            else
            {
                SceneManager.LoadScene(nextIndex);
            }
        }
    }
}
