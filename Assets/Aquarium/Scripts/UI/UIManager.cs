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
        public GameObject textUI;               // GoalPanel (컨테이너)
        public TextMeshProUGUI interactionText; // GoalText (실제 텍스트)
        #endregion

        #region Unity Event Methods
        void Awake()
        {
            // Singleton 설정
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        #endregion

        #region Custom Methods

        /// <summary>
        /// 현재 해야 할 일(목표)을 UI에 표시
        /// </summary>
        public void ShowGoal(string message)
        {
            interactionText.text = message;
            textUI.SetActive(true);
        }

        /// <summary>
        /// 목표 UI 숨김
        /// </summary>
        public void HideGoal()
        {
            textUI.SetActive(false);
        }

        #endregion
    }
}