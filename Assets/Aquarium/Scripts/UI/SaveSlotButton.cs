using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        private SceneFader sceneFader;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClickSlot);

            sceneFader = Object.FindFirstObjectByType<SceneFader>();

            if (sceneFader == null)
                Debug.LogError("[SaveSlotButton] SceneFader not found.");
        }

        private void OnClickSlot()
        {
            SaveManager.Instance.SetCurrentSlot(slotIndex);

            if (SaveManager.Instance.HasSave(slotIndex))
            {
                SaveData data = SaveManager.Instance.GetSaveData(slotIndex);
                SaveManager.Instance.PrepareLoad(slotIndex);

                Debug.Log($"[SaveSlotButton] Load → {data.sceneName}");

                StartCoroutine(LoadSceneWithFade(data.sceneName));
            }
            else
            {
                Debug.Log("[SaveSlotButton] New Game Start");

                StartCoroutine(LoadSceneWithFade(newGameSceneName));
            }
        }

        private System.Collections.IEnumerator LoadSceneWithFade(string sceneName)
        {
            if (sceneFader != null)
                sceneFader.FadeStart();

            yield return new WaitForSeconds(1f); // SceneFader 페이드 시간과 동일해야 함

            SceneManager.LoadScene(sceneName);
        }
    }
}
