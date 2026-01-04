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

            currentLocationID = locationID;
            Debug.Log($"[InteractionRegistry] Current Location set: {locationID}");
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
