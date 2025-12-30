/*using UnityEngine;
using System.Collections;

namespace Aquarium
{
    public class TeleportManager : MonoBehaviour
    {
        #region Singleton
        public static TeleportManager Instance;
        #endregion

        #region Variables
        [Header("References")]
        [SerializeField] private SceneFader sceneFader;
        [SerializeField] private GameObject player;

        private bool isTeleporting = false;
        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        #endregion

        #region Teleport
        /// <summary>
        /// Door Interaction에서 호출
        /// </summary>
        public void TeleportTo(Transform targetSpawnPos)
        {
            if (isTeleporting)
                return;

            StartCoroutine(TeleportRoutine(targetSpawnPos));
        }

        private IEnumerator TeleportRoutine(Transform targetSpawnPos)
        {
            isTeleporting = true;

            // 🔒 ACon 잠금 (이동 / Interaction 완전 차단)
            ACon.Instance.enabled = false;

            // 🔹 Fade Out
            yield return sceneFader.FadeOut(string.Empty);

            // 🔹 NavMeshAgent 안전하게 비활성화
            var agent = player.GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.enabled = false;

            // 🔹 위치 이동 (월드 좌표 기준)
            player.transform.position = targetSpawnPos.position;

            // 🔹 Agent 복구
            agent.enabled = true;
            agent.ResetPath();

            // 🔹 Fade In
            yield return sceneFader.FadeIn();

            // 🔓 ACon 복구
            ACon.Instance.enabled = true;
            isTeleporting = false;
        }
        #endregion
    }
}*/
/*using UnityEngine;
using System;
using UnityEngine.AI;

namespace Aquarium
{
    public class TeleportManager : MonoBehaviour
    {
        public static TeleportManager Instance;

        [SerializeField] private SceneFader sceneFader;
        [SerializeField] private NavMeshAgent playerAgent;
        [SerializeField] private Transform player;

        private Action onComplete;

        private void Awake()
        {
            Instance = this;
        }

        public void Teleport(Transform spawnPoint, Action onFinish)
        {
            onComplete = onFinish;
            StartCoroutine(TeleportRoutine(spawnPoint));
        }

        private System.Collections.IEnumerator TeleportRoutine(Transform spawn)
        {
            sceneFader.FadeStart();
            yield return new WaitForSeconds(1f);

            playerAgent.enabled = false;
            player.position = spawn.position;
            player.rotation = spawn.rotation;
            playerAgent.enabled = true;

            UIManager.Instance.SetState(UIState.None);
            onComplete?.Invoke();
        }
    }
}
*/
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Aquarium
{
    public class TeleportManager : MonoBehaviour
    {
        public static TeleportManager Instance;

        [SerializeField] private SceneFader sceneFader;
        [SerializeField] private NavMeshAgent playerAgent;

        private void Awake()
        {
            Instance = this;
        }

        public void Teleport(Transform targetSpawnPos, System.Action onComplete)
        {
            StartCoroutine(TeleportRoutine(targetSpawnPos, onComplete));
        }

        private IEnumerator TeleportRoutine(Transform target, System.Action onComplete)
        {
            // 🔹 UI 차단
            UIManager.Instance.SetState(UIState.Teleport);

            // 🔹 Fade Out
            yield return sceneFader.FadeOut(string.Empty);

            // 🔹 NavMesh 영향 제거
            playerAgent.enabled = false;
            playerAgent.transform.position = target.position;
            playerAgent.enabled = true;

            // 🔹 Fade In
            yield return sceneFader.FadeIn();

            // 🔹 UI 복구
            UIManager.Instance.SetState(UIState.None);

            onComplete?.Invoke();
        }
    }
}
