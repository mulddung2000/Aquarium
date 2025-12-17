using TMPro;
using UnityEngine;
using System.Collections;
using System.Linq;

namespace Test
{
    public abstract class Interactive : MonoBehaviour
    {        
        //추상 메서드
        #region abstract
        protected abstract void DoAction();
        #endregion

        #region Variables
        //스크립트 불러오기
        private OutlineNDosth outlineNDosth;

        //참조
        protected Collider collider;

        //머티리얼 관련
        private Material[] baseMaterial;    //저장된 원본 메테리얼
        private Renderer rend;              //읽기
        public Material outlineMaterial;    //아웃라인 메테리얼

        private bool isActive;

        //인터랙티브 UI
        [Header("Interactive UI")]        

        //액션 UI
        //public GameObject actionUI;
        //public TextMeshProUGUI actionText;

        [SerializeField]
        protected string action = "Do Action";
        #endregion

        #region Unity Event Method
        protected virtual void Awake()
        {
            //참조
            collider = GetComponent<Collider>();
        }
        private void Start()
        {
            rend = GetComponent<Renderer>();
            baseMaterial = rend.materials;

            // 시작할 때 아웃라인 끄기
            //rend.materials = baseMaterial;
        }

        protected virtual void OnMouseOver()
        {          
            ShowActionUI();            

            //만약 Action 버튼을 누르면
            if (Input.GetMouseButtonDown(1))
            {
                //우클릭시 > UI로 이동
                DoAction();
            }
        }

        protected virtual void OnMouseExit()
        {
            HideActionUI();
        }
        #endregion

        #region Custom Method
        protected virtual void ShowActionUI()
        {
            //머티리얼 온오프
            isActive = true;
            StartCoroutine(Toggle());
            //StartCoroutine 넣는거 잊지말기. Toggle() 이렇게만넣으면 작동 X
        }

        protected virtual void HideActionUI()
        {
            //머티리얼 온오프
            isActive = false;
            StartCoroutine(Toggle());
        }

        IEnumerator Toggle()
        { 
            if(isActive == true)
            {
                rend.materials = baseMaterial.Append(outlineMaterial).ToArray();
            }
            else
            {
                rend.materials = baseMaterial;
            }
            yield return null;
        }

        #endregion

    }
}