using UnityEngine;
using UnityEngine.AI;

namespace Aquarium
{
    public class PlayerLoadRestorer : MonoBehaviour
    {
        public static PlayerLoadRestorer Instance;

        private NavMeshAgent agent;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            agent = GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// Load 시 강제 위치 복원 (연출 없음)
        /// </summary>
        public void RestoreTo(Transform target)
        {
            if (target == null)
            {
                Debug.LogError("[PlayerLoadRestorer] Target is null");
                return;
            }

            // 🔥 NavMeshAgent 완전 차단
            if (agent != null)
                agent.enabled = false;

            transform.position = target.position;
            transform.rotation = target.rotation;

            // 🔥 1프레임 뒤 재활성화 (위치 덮어쓰기 방지)
            if (agent != null)
                StartCoroutine(ReenableAgent());
        }

        private System.Collections.IEnumerator ReenableAgent()
        {
            yield return null;
            agent.enabled = true;
        }
    }
}
