using UnityEngine;
using TMPro;

namespace Test
{
    public class OutlineNDosth : Interactive
    {
        /// <summary>
        /// 아웃라인 시험용
        /// </summary>S

        #region Variables
        //스크립트 불러오기
        public ShowUI2 showUI2;
        #endregion

        #region Unity Event Method
        protected override void DoAction()
        {
            if (gameObject.tag == "OpenComment")
            {
                //UI를 실행시켜라
                showUI2.ShowComment("");
                Debug.Log("대화내용을 보여줍니다");

                //if (대화가 끝나면)
                //showUI2.HideComment(); 시켜라

            }
            else if (gameObject.tag == "MoveRoom")
            {
                //~로 이동해라
                Debug.Log("방으로 이동합니다");
            }
            else if (gameObject.tag == "SceneFade")
            {
                //씬 이동해라
                Debug.Log("다음주로 넘어가시겠습니까?");
            }
        }
        #endregion
    }
}