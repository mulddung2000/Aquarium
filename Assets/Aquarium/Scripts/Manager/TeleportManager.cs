using UnityEngine;
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
}