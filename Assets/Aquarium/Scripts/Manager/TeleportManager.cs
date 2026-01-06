using System;
using System.Collections;
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
            ACon.Instance?.BeginTeleport();

            SceneFader.Instance.FadeOut(() =>
            {
                StartCoroutine(TeleportSequence(
                    targetSpawnPoint,
                    targetLocationID,
                    onComplete
                ));
            });
        }

        private IEnumerator TeleportSequence(
            Transform targetSpawnPoint,
            string targetLocationID,
            Action onComplete)
        {
            // 🔒 1️⃣ FadeOut 완전 종료 보장
            yield return new WaitForEndOfFrame();

            // 🔥 2️⃣ 카메라 Priority 변경 (화면은 이미 검정)
            CameraManager.Instance.SwitchCamera(targetLocationID);

            // 🔒 3️⃣ Cinemachine Brain이 Priority 평가할 프레임
            yield return null;

            // 🔒 4️⃣ Player 이동
            var player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = targetSpawnPoint.position;
                player.transform.rotation = targetSpawnPoint.rotation;
            }

            // 🔒 5️⃣ 모든 상태 완료 후 FadeIn
            SceneFader.Instance.FadeIn(() =>
            {
                onComplete?.Invoke();
            });
        }
    }
}
