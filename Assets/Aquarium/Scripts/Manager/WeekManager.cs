using UnityEngine;
using System.Collections;

namespace Aquarium
{
    public class WeekManager : MonoBehaviour
    {
        [Header("Week Info")]
        [SerializeField] private int currentWeek = 1;

        [Header("Scene Fader")]
        [SerializeField] private SceneFader sceneFader;

        [Header("First Interaction (New Game Only)")]
        [SerializeField] private GameObject firstInteraction;

        private void Start()
        {
            Debug.Log("[WeekManager] Scene Start");

            // 🔒 Load 중이면 절대 초기화하지 않는다
            if (SaveManager.Instance != null && SaveManager.Instance.HasPendingLoad())
            {
                StartCoroutine(SceneStartFlow());
                return;
            }

            // 🔹 New Game 초기화
            if (firstInteraction != null)
                firstInteraction.SetActive(false);

            StartCoroutine(SceneStartFlow());
        }

        private IEnumerator SceneStartFlow()
        {
            // 🔹 Fade In
            if (sceneFader != null)
                sceneFader.FadeStart();

            yield return new WaitForSeconds(1f);

            // 🔹 Load 우선 처리
            if (SaveManager.Instance != null && SaveManager.Instance.HasPendingLoad())
            {
                ApplyLoadedData(SaveManager.Instance.ConsumeLoadData());
                yield break;
            }

            // 🔹 New Game Start
            Debug.Log("[WeekManager] New Game Start");

            if (firstInteraction != null)
                firstInteraction.SetActive(true);
        }

        private void ApplyLoadedData(SaveData data)
        {
            // Week
            currentWeek = data.currentWeek;
            Debug.Log($"[WeekManager] Week restored: {currentWeek}");

            // Player
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                var agent = player.GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (agent != null)
                    agent.Warp(data.playerPosition);
                else
                    player.transform.position = data.playerPosition;
            }

            // Interaction 복구 (비활성 포함 검색)
            var interactions =
                Object.FindObjectsByType<InteractiveObject>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            foreach (var io in interactions)
            {
                bool active = io.InteractionID == data.nextInteractionID;
                io.gameObject.SetActive(active);
            }

            // Registry 복구
            InteractionRegistry.SetCurrentInteraction(data.nextInteractionID);

            // Goal 복구
            if (!string.IsNullOrEmpty(data.goalText))
                UIManager.Instance.SetGoal(data.goalText);

            UIManager.Instance.SetState(UIState.None);
        }

        #region Save API
        public int GetCurrentWeek()
        {
            return currentWeek;
        }
        #endregion
    }
}
