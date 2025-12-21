using UnityEngine;
using TMPro;

namespace Aquarium
{
    public class UIManager : MonoBehaviour
    {
        #region Singleton
        public static UIManager Instance;
        #endregion

        #region Variables
        [Header("Goal UI")]
        [SerializeField] private GameObject goalPanel;
        [SerializeField] private TextMeshProUGUI goalText;

        [Header("Hover UI")]
        [SerializeField] private GameObject hoverPanel;
        [SerializeField] private TextMeshProUGUI hoverText;

        [Header("Day End UI")]
        [SerializeField] private GameObject dayEndPanel;   // 🔹 추가

        private bool isDialogueActive = false;
        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        #endregion

        #region Hover UI
        public void ShowHover(string text)
        {
            if (isDialogueActive)
                return;

            hoverText.text = text;
            hoverPanel.SetActive(true);
        }

        public void HideHover()
        {
            hoverPanel.SetActive(false);
        }
        #endregion

        #region Goal UI
        public void SetGoal(string text)
        {
            goalText.text = text;
            goalPanel.SetActive(true);
        }

        public void HideGoal()
        {
            goalPanel.SetActive(false);
        }
        #endregion

        #region Dialogue State
        public void SetDialogueState(bool active)
        {
            isDialogueActive = active;

            if (active)
            {
                HideHover();
                HideGoal();
            }
            else
            {
                goalPanel.SetActive(true);
            }
        }
        #endregion

        #region Day End
        public void ShowDayEnd()
        {
            HideHover();

            // 🔹 마지막 목표 표시
            SetGoal("End Week");

            dayEndPanel.SetActive(true);
        }
        #endregion
    }
}
