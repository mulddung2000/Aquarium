using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Aquarium
{
    public class UIManager : MonoBehaviour
    {
        #region Singleton
        public static UIManager Instance;
        #endregion

        #region Variables
        public GameObject textUI;                       //Text컨테이너 오브젝트 연결
        public TextMeshProUGUI interactionText;         //Text 연결
        #endregion

        #region Unity Event Methods
        void Awake()
        {
            Instance = this;
        }
        #endregion

        #region Custom Methods
        public void ShowText(string message)
        {
            interactionText.text = message;
            textUI.SetActive(true);
        }

        public void HideText()
        {
            textUI.SetActive(false);
        }
        #endregion
    }
}