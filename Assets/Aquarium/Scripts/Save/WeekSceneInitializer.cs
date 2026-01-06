using UnityEngine;

namespace Aquarium
{
    public class WeekSceneInitializer : MonoBehaviour
    {
        [SerializeField] private string defaultLocationID = "W01_Room";

        private void Start()
        {
            // 🔥 Load 중이면 LoadApplier가 전부 책임진다
            if (SaveManager.Instance != null &&
                SaveManager.Instance.HasPendingLoad())
            {
                return;
            }

            // ==============================
            // 1️⃣ 기본 Location 등록
            // ==============================
            InteractionRegistry.SetCurrentLocation(defaultLocationID);

            // ==============================
            // 2️⃣ Camera 먼저 전환 (Fade 전에!)
            // ==============================
            if (CameraManager.Instance != null)
            {
                CameraManager.Instance.SwitchCamera(defaultLocationID);
            }

            // ==============================
            // 3️⃣ Fade 상태 보정 후 FadeIn
            // ==============================
            if (SceneFader.Instance != null)
            {
                // 🔥 이전 씬에서 Alpha가 1로 남아있는 경우 대비
                var fader = SceneFader.Instance;
                var canvasGroupField =
                    typeof(SceneFader)
                    .GetField("canvasGroup",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

                if (canvasGroupField != null)
                {
                    var canvasGroup =
                        canvasGroupField.GetValue(fader) as CanvasGroup;

                    if (canvasGroup != null)
                        canvasGroup.alpha = 1f;
                }

                SceneFader.Instance.FadeIn();
            }
        }
    }
}
