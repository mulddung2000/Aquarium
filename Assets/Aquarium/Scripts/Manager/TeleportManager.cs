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
