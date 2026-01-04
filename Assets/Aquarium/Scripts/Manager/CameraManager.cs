using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

namespace Aquarium
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        // key: Room / School / BRoom
        private Dictionary<string, GameObject> locationCameras =
            new Dictionary<string, GameObject>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            CacheAllLocationCameras();
        }

        private void OnEnable()
        {
            LocationEventHub.OnLocationChanged += OnLocationChanged;
        }

        private void OnDisable()
        {
            LocationEventHub.OnLocationChanged -= OnLocationChanged;
        }

        // Hierarchy 내 모든 Camera_* Cinemachine Camera 수집
        private void CacheAllLocationCameras()
        {
            locationCameras.Clear();

            var cameras = Object.FindObjectsByType<CinemachineCamera>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

            foreach (var cam in cameras)
            {
                GameObject camObject = cam.gameObject;

                if (!camObject.name.StartsWith("Camera_"))
                    continue;

                // Camera_Room -> Room
                string locationKey = camObject.name.Replace("Camera_", "");

                if (!locationCameras.ContainsKey(locationKey))
                {
                    locationCameras.Add(locationKey, camObject);
                }

                // 기본은 전부 비활성
                camObject.SetActive(false);
            }
        }

        // LocationEventHub 이벤트 수신
        private void OnLocationChanged(string locationID)
        {
            string locationKey = ExtractLocationKey(locationID);
            ActivateCamera(locationKey);
        }

        // W01_Room -> Room
        private string ExtractLocationKey(string locationID)
        {
            if (string.IsNullOrEmpty(locationID))
                return string.Empty;

            int underscoreIndex = locationID.LastIndexOf('_');
            if (underscoreIndex < 0)
                return locationID;

            return locationID.Substring(underscoreIndex + 1);
        }

        // 해당 Location 카메라만 활성화
        private void ActivateCamera(string locationKey)
        {
            if (string.IsNullOrEmpty(locationKey))
                return;

            foreach (var pair in locationCameras)
            {
                pair.Value.SetActive(false);
            }

            if (!locationCameras.ContainsKey(locationKey))
            {
                Debug.LogWarning(
                    $"CameraManager: Camera_{locationKey} 를 찾을 수 없습니다."
                );
                return;
            }

            locationCameras[locationKey].SetActive(true);
        }

        // Load 직후 / Fade 이전 강제 세팅용
        public void ForceSetLocation(string locationID)
        {
            string locationKey = ExtractLocationKey(locationID);
            ActivateCamera(locationKey);
        }
    }
}
