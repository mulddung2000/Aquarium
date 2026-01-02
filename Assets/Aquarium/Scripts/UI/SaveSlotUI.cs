using UnityEngine;
using TMPro;

namespace Aquarium
{
    public class SaveSlotUI : MonoBehaviour
    {
        [Header("Slot Info")]
        [SerializeField] private int slotIndex = 1;

        [Header("Roots")]
        [SerializeField] private GameObject filledRoot;
        [SerializeField] private GameObject emptyRoot;

        [Header("Filled Texts")]
        [SerializeField] private TextMeshProUGUI weekText;
        [SerializeField] private TextMeshProUGUI locationText;
        [SerializeField] private TextMeshProUGUI goalText;
        [SerializeField] private TextMeshProUGUI dateText;

        private void Start()
        {
            Debug.Log($"[SaveSlotUI] Start Slot {slotIndex}");
            Refresh();
        }

        public void Refresh()
        {
            Debug.Log($"[SaveSlotUI] Refresh Slot {slotIndex}");
            if (SaveManager.Instance == null)
            {
                Debug.LogError("[SaveSlotUI] SaveManager not found.");
                ShowEmpty();
                return;
            }

            SaveData data = SaveManager.Instance.GetSaveData(slotIndex);

            if (data == null)
            {
                ShowEmpty();
                return;
            }

            ShowFilled(data);
        }

        private void ShowFilled(SaveData data)
        {
            filledRoot.SetActive(true);
            emptyRoot.SetActive(false);

            weekText.text = $"Week {data.currentWeek}";
            locationText.text = ConvertSceneName(data.sceneName);
            goalText.text = string.IsNullOrEmpty(data.goalText) ? "-" : data.goalText;
            dateText.text = data.saveDateTime;
        }

        private void ShowEmpty()
        {
            filledRoot.SetActive(false);
            emptyRoot.SetActive(true);
        }

        private string ConvertSceneName(string sceneName)
        {
            // UI 표시용 변환 (나중에 LocationID 기반으로 교체 가능)
            switch (sceneName)
            {
                case "Week01Scene": return "Bedroom";
                case "SchoolScene": return "School";
                default: return sceneName;
            }
        }
    }
}
