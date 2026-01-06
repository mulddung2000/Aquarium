using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

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

        [Header("Remove Button")]
        [SerializeField] private Button removeDataButton;

        private void Awake()
        {
            // RemoveData 버튼 자동 연결 (Inspector 미지정 시)
            if (removeDataButton == null)
            {
                Transform t = transform.Find("RemoveData");
                if (t != null)
                    removeDataButton = t.GetComponent<Button>();
            }

            if (removeDataButton != null)
            {
                removeDataButton.onClick.AddListener(RemoveSaveData);
            }
        }

        private void Start()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (SaveManager.Instance == null)
            {
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
            locationText.text = ConvertLocationIDToKorean(data.locationID);
            goalText.text = string.IsNullOrEmpty(data.goalText) ? "-" : data.goalText;
            dateText.text = data.saveDateTime;

            if (removeDataButton != null)
                removeDataButton.gameObject.SetActive(true);
        }

        private void ShowEmpty()
        {
            filledRoot.SetActive(false);
            emptyRoot.SetActive(true);

            if (removeDataButton != null)
                removeDataButton.gameObject.SetActive(false);
        }

        private void RemoveSaveData()
        {
            if (SaveManager.Instance == null)
                return;

            string path = GetSlotPath(slotIndex);

            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[SaveSlotUI] Slot {slotIndex} save data removed.");
            }

            Refresh();
        }

        private string GetSlotPath(int index)
        {
            string dir = Path.Combine(Application.persistentDataPath, "saves");
            return Path.Combine(dir, $"slot_{index}.json");
        }
        /// <summary>
        /// LocationID를 한글 장소명으로 변환한다
        /// 규칙:
        /// - LocationID는 "W01_Room" 같은 형식
        /// - 마지막 '_' 뒤 토큰을 장소 코드로 사용
        /// </summary>

        private string ConvertLocationIDToKorean(string locationID)
        {
            if (string.IsNullOrEmpty(locationID))
                return "-";
            // W01_Room -> Room
            string[] tokens = locationID.Split('_');
            string placeCode = tokens[tokens.Length - 1];

            switch (placeCode)
            {
                case "Room":
                    return "방";
                case "School":
                    return "학교";
                case "BRoom":
                    return "발레 연습실";
                case "Kitchen":
                    return "거실";
                case "School02":
                    return "학교";
                default:
                    // 정의되지 않은 장소는 그대로 노출 (디버그 목적)
                    return placeCode;
            }
        }
    }
}
