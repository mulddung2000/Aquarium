using UnityEngine;
using UnityEngine.AI;

namespace MySample
{
    public class ACon : MonoBehaviour
    {
        #region Variables
        NavMeshAgent m_Agent;
        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            m_Agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            //마우스 우클릭한 지점으로 이동
            if(Input.GetMouseButton(1))
            {
                RayToWorld();                
            }
        }
        #endregion

        #region Custom Methods
        //마우스 포인터 위치에서 레이를 쏘아 히트한 지점의 위치를 반환한다
        private void RayToWorld()
        {
            Vector3 worldPos = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //hit.poin를 목적지로 설정(SetDestination)
                m_Agent.SetDestination(hit.point);
            }            
        }
        #endregion
    }
}