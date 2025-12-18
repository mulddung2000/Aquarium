using UnityEngine;

namespace Aquarium
{
    public class InteractiveObject : MonoBehaviour
    {
        // intentionally empty (prototype stage)
        #region Variables
        private bool isActive = true;
        #endregion

        #region Unity Event Methods
        private void OnMouseDown()
        {
            // 좌클릭만 반응
            if (!isActive)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log($"[Interaction] Clicked : {gameObject.name}");
            }
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// 이 Interaction을 완료 상태로 만든다
        /// (지금은 호출하지 않음)
        /// </summary>
        public void CompleteInteraction()
        {
            isActive = false;
        }
        #endregion
    }
}
