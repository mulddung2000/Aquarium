using UnityEngine;

namespace Aquarium
{
    public static class InteractionRegistry
    {
        private static string currentInteractionID;

        public static void SetCurrentInteraction(string id)
        {
            currentInteractionID = id;
        }

        public static string GetCurrentInteraction()
        {
            return currentInteractionID;
        }
    }
}
