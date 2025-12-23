/*using UnityEngine;
using UnityEngine.AI;

namespace Aquarium
{
    public class ACon : MonoBehaviour
    {
        #region Singleton
        public static ACon Instance;
        #endregion

        #region Variables
        private NavMeshAgent agent;

        // 현재 이동 중인 Interaction
        private InteractiveObject currentTarget;
        private bool isMovingToInteraction;
        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            // 자유 이동 (우클릭)
            if (Input.GetMouseButton(1))
            {
                RayToWorld();
                ClearInteractionTarget();
            }

            // 🔹 Interaction으로 이동 중일 때만 도착 판정
            if (isMovingToInteraction && currentTarget != null)
            {
                CheckInteractionDistance();
            }
        }
        #endregion

        #region Interaction Control
        /// <summary>
        /// Interaction 클릭 시 호출
        /// → 이동 시작
        /// </summary>
        public void SetTargetInteraction(InteractiveObject target)
        {
            currentTarget = target;
            isMovingToInteraction = true;

            agent.SetDestination(target.InteractionPoint.position);
        }

        /// <summary>
        /// InteractionPoint까지 도착했는지 확인
        /// </summary>
        private void CheckInteractionDistance()
        {
            float distance = Vector3.Distance(
                transform.position,
                currentTarget.InteractionPoint.position
            );

            if (distance <= currentTarget.InteractionRadius)
            {
                agent.ResetPath();
                isMovingToInteraction = false;

                // 🔹 여기서 Interaction 실행
                currentTarget.ExecuteInteraction();
                currentTarget = null;
            }
        }

        private void ClearInteractionTarget()
        {
            currentTarget = null;
            isMovingToInteraction = false;
        }
        #endregion

        #region Movement
        private void RayToWorld()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
        #endregion
    }
}*/
using UnityEngine;
using UnityEngine.AI;

namespace Aquarium
{
    public class ACon : MonoBehaviour
    {
        public static ACon Instance;

        private NavMeshAgent agent;

        private InteractiveObject currentTarget;
        private bool isMovingToInteraction;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            // 자유 이동
            if (Input.GetMouseButton(1))
            {
                RayToWorld();
                ClearInteractionTarget();
            }

            if (isMovingToInteraction && currentTarget != null)
            {
                CheckInteractionDistance();
            }
        }

        public void SetTargetInteraction(InteractiveObject target)
        {
            currentTarget = target;
            isMovingToInteraction = true;

            agent.SetDestination(target.InteractionPoint.position);
        }

        private void CheckInteractionDistance()
        {
            float distance = Vector3.Distance(
                transform.position,
                currentTarget.InteractionPoint.position
            );

            if (distance <= currentTarget.InteractionRadius)
            {
                agent.ResetPath();
                isMovingToInteraction = false;

                currentTarget.ExecuteInteraction();
                currentTarget = null;
            }
        }

        private void ClearInteractionTarget()
        {
            currentTarget = null;
            isMovingToInteraction = false;
        }

        private void RayToWorld()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}

