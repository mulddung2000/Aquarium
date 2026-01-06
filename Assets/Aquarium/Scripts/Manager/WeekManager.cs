using UnityEngine;

namespace Aquarium
{
    public class WeekManager : MonoBehaviour
    {
        public static WeekManager Instance;

        [SerializeField] private int currentWeek = 1;
        [SerializeField] private GameObject firstInteraction;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            // 🔥 로드 중이면 첫 Interaction 자동 활성화 금지
            if (SaveManager.Instance != null &&
                SaveManager.Instance.HasPendingLoad())
            {
                return;
            }

            if (firstInteraction != null)
                firstInteraction.SetActive(true);
        }

        public int GetCurrentWeek()
        {
            return currentWeek;
        }

        public void AdvanceWeek()
        {
            currentWeek++;
        }

        public void SetWeek(int week)
        {
            currentWeek = Mathf.Max(1, week);
        }
    }
}
