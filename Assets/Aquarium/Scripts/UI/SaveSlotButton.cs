/*using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Aquarium
{
    public class SaveSlotButton : MonoBehaviour
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
*/
using UnityEngine;
using UnityEngine.UI;

namespace Aquarium
{
    [RequireComponent(typeof(Button))]
    public class SaveSlotButton : MonoBehaviour
    {
        [Header("Slot Info")]
        [SerializeField] private int slotIndex = 0;

        [Header("Scene")]
        [SerializeField] private string newGameSceneName = "Week01";

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClickSlot);
        }

        private void OnClickSlot()
        {
            SaveManager.Instance.SetCurrentSlot(slotIndex);

            if (SaveManager.Instance.HasSave(slotIndex))
            {
                SaveData data = SaveManager.Instance.GetSaveData(slotIndex);
                SaveManager.Instance.PrepareLoad(slotIndex);

                Debug.Log($"[SaveSlotButton] Load → {data.sceneName}");

                SceneFader.Instance.FadeToScene(data.sceneName);
            }
            else
            {
                Debug.Log("[SaveSlotButton] New Game Start");

                SceneFader.Instance.FadeToScene(newGameSceneName);
            }
        }
    }
}
