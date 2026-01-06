using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

namespace Aquarium
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        [System.Serializable]
        public class LocationCamera
        {
            public string locationID;                 // ex) W01_Room
            public CinemachineCamera camera;          // Cinemachine 3.x
        }

        [Header("Camera Settings")]
        [SerializeField] private int activePriority = 20;
        [SerializeField] private int inactivePriority = 0;

        [SerializeField] private List<LocationCamera> cameras;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        /// <summary>
        /// LocationID 기준으로 CinemachineCamera Priority 전환
        /// FadeOut ~ FadeIn 사이에서 호출될 것
        /// </summary>
        public void SwitchCamera(string locationID)
        {
            if (string.IsNullOrEmpty(locationID))
            {
                Debug.LogWarning("[CameraManager] locationID is null or empty");
                return;
            }

            bool found = false;

            foreach (var entry in cameras)
            {
                if (entry.camera == null)
                {
                    Debug.LogError("[CameraManager] CinemachineCamera reference is NULL");
                    continue;
                }

                bool isTarget = entry.locationID == locationID;
                entry.camera.Priority = isTarget ? activePriority : inactivePriority;

                if (isTarget)
                    found = true;
            }

            if (!found)
            {
                Debug.LogWarning($"[CameraManager] No camera matched locationID = {locationID}");
            }
        }
    }
}
