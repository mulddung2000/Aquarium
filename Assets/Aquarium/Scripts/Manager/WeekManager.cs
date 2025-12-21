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

        [Header("First Interaction")]
        [SerializeField] private GameObject firstInteraction;

        private void Start()
        {
            Debug.Log($"[WeekManager] Week {currentWeek} Start");

            if (firstInteraction != null)
                firstInteraction.SetActive(false);

            StartCoroutine(WeekStartFlow());
        }

        private IEnumerator WeekStartFlow()
        {
            sceneFader.FadeStart();

            yield return new WaitForSeconds(1f);

            firstInteraction.SetActive(true);
        }
    }
}
