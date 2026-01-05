using System.Collections;
using UnityEngine;

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
            StartCoroutine(SceneStartFlow());
        }

        private IEnumerator SceneStartFlow()
        {
            // ❌ sceneFader.FadeStart(); 제거

            yield return null;

            if (SaveManager.Instance != null && SaveManager.Instance.HasPendingLoad())
            {
                ApplyLoadedData(SaveManager.Instance.ConsumeLoadData());
                yield break;
            }

            if (firstInteraction != null)
                firstInteraction.SetActive(true);

            DeclareInitialLocation();
        }

        private void ApplyLoadedData(SaveData data)
        {
            currentWeek = data.currentWeek;

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                var agent = player.GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (agent != null)
                    agent.Warp(data.playerPosition);
                else
                    player.transform.position = data.playerPosition;
            }

            // Interaction 복원
            var interactions = Object.FindObjectsByType<InteractiveObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

            foreach (var io in interactions)
                io.gameObject.SetActive(io.InteractionID == data.nextInteractionID);

            InteractionRegistry.SetCurrentInteraction(data.nextInteractionID);

            if (!string.IsNullOrEmpty(data.goalText))
                UIManager.Instance.SetGoal(data.goalText);

            DeclareInitialLocation();

            UIManager.Instance.SetState(UIState.None);
        }

        public void DeclareInitialLocation()
        {
            string initialLocationID = InteractionRegistry.GetCurrentLocation();
            if (string.IsNullOrEmpty(initialLocationID))
            {
                initialLocationID = $"W{currentWeek:D2}_Room";
                InteractionRegistry.SetCurrentLocation(initialLocationID);
            }

            LocationEventHub.SetLocation(initialLocationID);

            if (CameraManager.Instance != null)
                CameraManager.Instance.ForceSetLocation(initialLocationID);
        }

        public int GetCurrentWeek() => currentWeek;
    }
}
