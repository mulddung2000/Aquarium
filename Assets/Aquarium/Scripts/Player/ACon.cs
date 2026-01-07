using UnityEngine;
using UnityEngine.AI;

namespace Aquarium
{
    public class ACon : MonoBehaviour
    {
        public static ACon Instance;

        private NavMeshAgent agent;
        private Animator animator;

        private InteractiveObject currentTarget;
        private bool isMovingToInteraction;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
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

            MoveSpeed();
        }

        public void BeginTeleport()
        {
            if (agent != null)
            {
                agent.ResetPath();
                agent.enabled = false;
            }
        }

        public void EndTeleport(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;

            if (agent != null)
            {
                agent.enabled = true;
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

        private void MoveSpeed()
        {
            float currentSpeed = agent.velocity.magnitude;
            animator.SetFloat("Speed", currentSpeed);
        }
    }
}

