using UnityEngine;
using System.Collections;

namespace Aquarium
{
    public class WeekManager : MonoBehaviour
    {
        #region Variables
        [Header("Week Info")]
        [SerializeField] private int weekIndex = 1;

        [Header("References")]
        [SerializeField] private SceneFader sceneFader;

        [Header("Week Start Settings")]
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private GameObject firstInteraction;
        #endregion

        #region Unity Event Methods
        private void Start()
        {
            StartWeek();
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// 주차 시작 진입점
        /// </summary>
        private void StartWeek()
        {
            Debug.Log($"[WeekManager] Week {weekIndex} Start");

            if (sceneFader != null)
            {
                sceneFader.FadeStart();
            }
            else
            {
                Debug.LogWarning("[WeekManager] SceneFader reference is missing.");
            }

            // 페이드 종료 이후 첫 Interaction 활성화
            StartCoroutine(WaitForFadeAndStartInteraction());
        }

        /// <summary>
        /// 페이드 연출 종료를 기다린 후
        /// 첫 Interaction을 활성화한다
        /// </summary>
        private IEnumerator WaitForFadeAndStartInteraction()
        {
            yield return new WaitForSeconds(fadeDuration);

            if (firstInteraction != null)
            {
                firstInteraction.SetActive(true);
                Debug.Log("[WeekManager] First Interaction Activated");
            }
            else
            {
                Debug.LogWarning("[WeekManager] First Interaction is not assigned.");
            }
        }
        #endregion
    }
}
