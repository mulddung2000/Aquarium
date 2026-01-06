using UnityEngine;

namespace Aquarium
{
    public static class InteractionRegistry
    {
        private static string currentInteractionID;
        private static string currentLocationID;

        public static void SetCurrentInteraction(string id)
        {
            currentInteractionID = id;
        }

        public static string GetCurrentInteraction()
        {
            return currentInteractionID;
        }

        public static void SetCurrentLocation(string locationID)
        {
            if (string.IsNullOrEmpty(locationID))
                return;

            if (currentLocationID == locationID)
                return; // 🔥 중복 방지

            currentLocationID = locationID;
            Debug.Log($"[InteractionRegistry] Current Location set: {locationID}");

            // 🔥 여기서 카메라 전환
            if (CameraManager.Instance != null)
            {
                CameraManager.Instance.SwitchCamera(locationID);
            }
        }


        public static string GetCurrentLocation()
        {
            return currentLocationID;
        }

        public static void Reset()
        {
            currentInteractionID = null;
            currentLocationID = null;
        }
    }
}
