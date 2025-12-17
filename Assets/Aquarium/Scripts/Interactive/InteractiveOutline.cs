using UnityEngine;

namespace Aquarium
{
    public class InteractiveOutline : MonoBehaviour
    {
        #region Variables
        //참조
        InteractiveObject interactiveObject;

        public Camera cam;          //메인카메라 연결

        [SerializeField]
        float maxDistance = 100f;   //Raycast 최대거리
        #endregion

        #region Properties

        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            
        }
        void Start()
        {
            if (cam == null)
                cam = Camera.main;
        }

        void Update()
        {
            CheckObject();
            Click();
        }
        #endregion

        #region Custom Methods
        void CheckObject()
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);

            // maxDistance 안에 있는 InteractiveObject 스크립트가 붙은 Collider 찾기 
            if (Physics.Raycast(ray, out var hit, maxDistance))
            {
                var obj = hit.collider.GetComponentInParent<InteractiveObject>();

                if (obj != null)
                {
                    if (interactiveObject != obj)
                    {
                        SelectObject(obj);
                    }

                    return;
                }
            }

            DeselectObject();
        }

        void Click()    //좌클릭 시,이벤트 발생. 이후 이벤트 발생 시, UIManager.Instance.HideText() 실행 시키기
        {
            if (interactiveObject == null)
            {
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                DeselectObject();
            }
        }

        void SelectObject(InteractiveObject obj)    //InteractiveObject UI 켜기
        {
            interactiveObject = obj;

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowText(obj.interactionText);
            }
        }

        void DeselectObject()       //InteractiveObject 선택 해제하고 UI 끄기
        {
            if (interactiveObject != null)
            {
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.HideText();
                }

                interactiveObject = null;
            }
        }

        #endregion
    }
}