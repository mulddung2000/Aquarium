using UnityEngine;
using System.Collections;

namespace Aquarium
{
    public class TeleportManager : MonoBehaviour
    {
        public static TeleportManager Instance;

        [SerializeField] private SceneFader sceneFader;
        [SerializeField] private Transform player;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void RequestTeleport(Transform targetSpawn)
        {
            if (targetSpawn == null)
            {
                Debug.LogError("[Teleport] TargetSpawn is NULL");
                return;
            }

            StartCoroutine(TeleportRoutine(targetSpawn));
        }

        private IEnumerator TeleportRoutine(Transform targetSpawn)
        {
            Debug.Log("[Teleport] Fade Out Start");
            yield return sceneFader.FadeOut(null);

            Debug.Log("[Teleport] Move Player");
            player.position = targetSpawn.position;
            player.rotation = targetSpawn.rotation;

            Debug.Log("[Teleport] Fade In Start");
            sceneFader.FadeStart(0f);
        }
    }
}
