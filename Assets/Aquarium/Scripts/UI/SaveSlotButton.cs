using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Aquarium
{
    public class SaveSlotButton : MonoBehaviour
    {
        [SerializeField] private int slotIndex;
        [SerializeField] private string targetSceneName = "Week01";

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.SetCurrentSlot(slotIndex);
                SaveManager.Instance.PrepareLoad(slotIndex);
            }

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
