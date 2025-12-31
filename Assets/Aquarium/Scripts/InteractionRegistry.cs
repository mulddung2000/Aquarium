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

        public static string GetCurrentInteractionID()
        {
            return currentInteractionID;
        }

        public static void RestoreInteraction(string id)
        {
            currentInteractionID = id;
            // 실제 활성화는 InteractiveObjectManager 단계에서 처리
        }
    }
}
