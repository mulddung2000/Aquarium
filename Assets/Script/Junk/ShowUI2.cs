using UnityEngine;
using TMPro;

namespace Test
{
    /// <summary>
    /// 인터랙티브 UI 관리 (show, hide) 하는 클래스
    /// </summary>
    public class ShowUI2 : MonoBehaviour
    {
        #region Variablse
        //인터랙티브 UI 오브젝트
        public GameObject commentUI;
        public TextMeshProUGUI comment;


        #endregion

        #region Unity Event Method
        public void Start()
        {
            commentUI.SetActive(false);
        }
        #endregion


        #region Custom Method
        public void ShowComment(string message)
        {
            commentUI.SetActive(true);
            comment.text = message;

            //터치or스페이스바 누르면
            //텍스트 내용 바뀌도록 어케하지
        }

        public void HideComment()
        {
            commentUI.SetActive(false);
            comment.text = "";
        }
        #endregion
    }
}