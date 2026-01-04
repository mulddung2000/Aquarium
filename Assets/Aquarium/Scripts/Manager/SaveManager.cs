using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

namespace Aquarium
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance;

        private int currentSlotIndex = -1;
        private SaveData pendingLoadData;

        public bool IsLoading { get; private set; }

        private string SavePath =>
            Path.Combine(Application.persistentDataPath, "saves");

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #region Slot
        public void SetCurrentSlot(int slotIndex)
        {
            currentSlotIndex = slotIndex;
        }

        public bool HasSave(int slotIndex)
        {
            return File.Exists(GetSlotPath(slotIndex));
        }
        #endregion

        #region Save
        public void SaveGame()
        {
            if (currentSlotIndex < 0)
            {
                Debug.LogError("[SaveManager] No slot selected.");
                return;
            }

            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);

            WeekManager weekManager = Object.FindFirstObjectByType<WeekManager>();
            GameObject player = GameObject.FindWithTag("Player");

            string goalText = UIManager.Instance != null
                ? UIManager.Instance.GetCurrentGoalText()
                : "";

            SaveData data = new SaveData
            {
                sceneName = SceneManager.GetActiveScene().name,
                currentWeek = weekManager != null ? weekManager.GetCurrentWeek() : 1,
                nextInteractionID = InteractionRegistry.GetCurrentInteraction(),
                locationID = InteractionRegistry.GetCurrentLocation(),
                playerPosition = player != null ? player.transform.position : Vector3.zero,
                goalText = goalText,
                saveDateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            };

            File.WriteAllText(
                GetSlotPath(currentSlotIndex),
                JsonUtility.ToJson(data, true)
            );

            Debug.Log($"[SaveManager] Game Saved (Slot {currentSlotIndex})");
        }
        #endregion

        #region Load (Prepare Only)
        public void PrepareLoad(int slotIndex)
        {
            if (!HasSave(slotIndex))
                return;

            currentSlotIndex = slotIndex;

            string json = File.ReadAllText(GetSlotPath(slotIndex));
            pendingLoadData = JsonUtility.FromJson<SaveData>(json);

            IsLoading = true;
        }

        public bool HasPendingLoad()
        {
            return IsLoading && pendingLoadData != null;
        }

        public SaveData ConsumeLoadData()
        {
            SaveData data = pendingLoadData;
            pendingLoadData = null;
            IsLoading = false;
            return data;
        }
        #endregion

        public SaveData GetSaveData(int slotIndex)
        {
            string path = GetSlotPath(slotIndex);

            if (!File.Exists(path))
                return null;

            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }

        private string GetSlotPath(int slotIndex)
        {
            return Path.Combine(SavePath, $"slot_{slotIndex}.json");
        }
    }
}
