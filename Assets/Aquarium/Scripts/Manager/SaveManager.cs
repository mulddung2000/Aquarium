using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System.IO;

namespace Aquarium
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance;

        private SaveData pendingLoadData;

        private const int SLOT_COUNT = 3;
        private const string SAVE_FOLDER = "Saves";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #region Path

        private string GetSaveFolderPath()
        {
            return Path.Combine(Application.persistentDataPath, SAVE_FOLDER);
        }

        private string GetSavePath(int slot)
        {
            return Path.Combine(GetSaveFolderPath(), $"slot_{slot}.json");
        }

        #endregion

        #region Save

        public void SaveGame(int slot)
        {
            if (slot < 1 || slot > SLOT_COUNT)
            {
                Debug.LogError("[SaveManager] Invalid slot index");
                return;
            }

            if (!Directory.Exists(GetSaveFolderPath()))
                Directory.CreateDirectory(GetSaveFolderPath());

            WeekManager weekManager = FindFirstObjectByType<WeekManager>();

            SaveData data = new SaveData
            {
                week = weekManager != null ? weekManager.GetCurrentWeek() : 1,
                sceneName = SceneManager.GetActiveScene().name,
                playerPosition = ACon.Instance.transform.position,
                interactionID = InteractionRegistry.GetCurrentInteractionID(),
                locationID = LocationRegistry.CurrentLocationID,
                uiState = UIState.None
            };

            File.WriteAllText(GetSavePath(slot), JsonUtility.ToJson(data, true));

            Debug.Log($"[SaveManager] Saved Slot {slot}");
        }

        #endregion

        #region Load

        public bool HasSave(int slot)
        {
            return File.Exists(GetSavePath(slot));
        }

        public void LoadGame(int slot)
        {
            if (!HasSave(slot))
            {
                Debug.LogError("[SaveManager] No save data");
                return;
            }

            pendingLoadData = JsonUtility.FromJson<SaveData>(
                File.ReadAllText(GetSavePath(slot))
            );

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(pendingLoadData.sceneName);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            RestorePlayerPosition(pendingLoadData.playerPosition);

            InteractionRegistry.RestoreInteraction(pendingLoadData.interactionID);
            LocationRegistry.CurrentLocationID = pendingLoadData.locationID;

            UIManager.Instance.SetState(pendingLoadData.uiState);

            pendingLoadData = null;

            Debug.Log("[SaveManager] Load Complete");
        }

        private void RestorePlayerPosition(Vector3 savedPos)
        {
            NavMeshAgent agent = ACon.Instance.GetComponent<NavMeshAgent>();

            agent.enabled = false;

            if (NavMesh.SamplePosition(savedPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                agent.transform.position = hit.position;
            }
            else
            {
                agent.transform.position = savedPos;
            }

            agent.enabled = true;
            agent.ResetPath();
        }

        #endregion
    }
}
