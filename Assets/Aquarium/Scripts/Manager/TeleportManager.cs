/*using System;
using UnityEngine;

namespace Aquarium
{
    public class TeleportManager : MonoBehaviour
    {
        public static TeleportManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void Teleport(
            Transform targetSpawnPoint,
            string targetLocationID,
            Action onComplete = null)
        {
            // 1️⃣ FadeOut 시작
            SceneFader.Instance.FadeOut(() =>
            {
                // 2️⃣ Camera 먼저 전환 (Fade 중이라 화면 안 보임)
                CameraManager.Instance.SwitchCamera(targetLocationID);

                // 3️⃣ Player 이동
                var player = GameObject.FindWithTag("Player");
                player.transform.position = targetSpawnPoint.position;
                player.transform.rotation = targetSpawnPoint.rotation;

                // 4️⃣ FadeIn
                SceneFader.Instance.FadeIn(() =>
                {
                    onComplete?.Invoke();
                });
            });
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
            UIManager.Instance.SetState(UIState.Teleport);

            // 🔹 FadeOut
            yield return SceneFader.Instance.StartCoroutine(
                SceneFader.Instance
                    .GetType()
                    .GetMethod("FadeOut", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Invoke(SceneFader.Instance, null) as IEnumerator
            );

            // 🔹 위치 이동
            playerAgent.enabled = false;
            playerAgent.transform.position = target.position;
            playerAgent.enabled = true;

            // 🔹 Location 기준 카메라 재세팅
            if (!string.IsNullOrEmpty(InteractionRegistry.GetCurrentLocation()))
            {
                CameraManager.Instance.ForceSetLocation(
                    InteractionRegistry.GetCurrentLocation()
                );
            }

            // 🔹 FadeIn
            yield return SceneFader.Instance.StartCoroutine(
                SceneFader.Instance
                    .GetType()
                    .GetMethod("FadeIn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Invoke(SceneFader.Instance, null) as IEnumerator
            );

            UIManager.Instance.SetState(UIState.None);

            onComplete?.Invoke();
        }
    }
}
