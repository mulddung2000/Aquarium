using UnityEngine;
using UnityEngine.UI;

namespace Aquarium
{
    public class SaveSlotButton : MonoBehaviour
    {
        [SerializeField] private int slotIndex = 1;
        [SerializeField] private string week01SceneName = "Week01";
        [SerializeField] private SceneFader sceneFader;

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
                SaveManager.Instance.PrepareLoad(slotIndex);
            }

            // New / Load 구분 없이 씬 이동
            sceneFader.FadeTo(week01SceneName);
        }
    }
}
