using System.Collections;
using UnityEngine;

namespace Aquarium
{
    public class LoadApplier : MonoBehaviour
    {
        private IEnumerator Start()
        {
            // ==============================
            // 0️⃣ 로드 여부 체크
            // ==============================
            if (SaveManager.Instance == null)
                yield break;

            if (!SaveManager.Instance.HasPendingLoad())
                yield break;

            // 🔥 Awake / OnEnable / Scene Init 보장
            yield return null;
            yield return new WaitForEndOfFrame();

            SaveData data = SaveManager.Instance.ConsumeLoadData();
            Debug.Log($"✅ LoadData consumed: {data.nextInteractionID}");

            // ==============================
            // 1️⃣ Week 복원
            // ==============================
            var weekManager = FindFirstObjectByType<WeekManager>();
            if (weekManager != null)
            {
                weekManager.SetWeek(data.currentWeek);
            }

            // ==============================
            // 2️⃣ Player 위치 복원 (NavMesh 안전)
            // ==============================
            var player = GameObject.FindWithTag("Player");
            if (player != null && PlayerLoadRestorer.Instance != null)
            {
                // 임시 Transform 생성 (좌표만 필요)
                var temp = new GameObject("[Temp_LoadSpawn]");
                temp.transform.position = data.playerPosition;
                temp.transform.rotation = player.transform.rotation;

                PlayerLoadRestorer.Instance.RestoreTo(temp.transform);

                Destroy(temp);
            }

            // ==============================
            // 3️⃣ Location + Camera 복원 (🔥 FadeIn 이전)
            // ==============================
            InteractionRegistry.SetCurrentLocation(data.locationID);

            if (CameraManager.Instance != null)
            {
                CameraManager.Instance.SwitchCamera(data.locationID);
            }

            // 🔥 Camera Priority 평가 프레임 보장
            yield return new WaitForEndOfFrame();

            // ==============================
            // 4️⃣ Interaction 진행 상태 복원
            // ==============================
            RestoreInteractionState(data.nextInteractionID);

            // ==============================
            // 5️⃣ Goal 복원
            // ==============================
            if (UIManager.Instance != null)
            {
                UIManager.Instance.SetGoal(data.goalText);
                UIManager.Instance.ForceResetState();
            }

            // ==============================
            // 6️⃣ FadeIn (🔥 반드시 맨 마지막)
            // ==============================
            SceneFader.Instance?.FadeIn();

            Debug.Log("✅ Load Apply Finished");
        }

        private void RestoreInteractionState(string interactionID)
        {
            if (string.IsNullOrEmpty(interactionID))
                return;

            // 🔥 Unity 6 대응: 비활성 Interaction 포함 검색
            var allInteractions =
                Object.FindObjectsByType<InteractiveObject>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            InteractiveObject target = null;

            foreach (var io in allInteractions)
            {
                if (io.InteractionID == interactionID)
                {
                    target = io;
                    break;
                }
            }

            if (target == null)
            {
                Debug.LogError(
                    $"[LoadApplier] CRITICAL: Interaction not found even including inactive: {interactionID}"
                );
                return;
            }

            // 🔥 반드시 활성화
            if (!target.gameObject.activeSelf)
                target.gameObject.SetActive(true);

            InteractionRegistry.SetCurrentInteraction(interactionID);
        }
    }
}
