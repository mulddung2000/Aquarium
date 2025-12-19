using UnityEngine;
using System.Collections;

namespace Aquarium
{
    public class WeekManager : MonoBehaviour
    {
        #region Variables

        [Header("Week Settings")]
        [SerializeField] private int currentWeek = 1;

        [Header("First Interaction")]
        // 이번 주차의 첫 Interaction
        [SerializeField] private GameObject firstInteraction;
        [SerializeField] private SceneFader sceneFader;

        #endregion


        #region Unity Event Methods

        private void Start()
        {
            StartCoroutine(StartWeek());
        }

        #endregion


        #region Week Flow

        private IEnumerator StartWeek()
        {
            Debug.Log("[WeekManager] Week Start");

            // 1️⃣ FadeIn 연출이 끝날 때까지 대기
            yield return StartCoroutine(sceneFader.FadeIn());

            // 2️⃣ 연출이 끝난 뒤에만
            //    Interaction 활성 + Goal 표시
            ActivateFirstInteraction();
        }

        private IEnumerator PlayWeekIntro()
        {
            // SceneFader의 페이드 아웃이 끝날 때까지 대기
            sceneFader.FadeStart();   // FadeIn 시작
            yield return null;        // (또는 연출 완료까지 기다리도록 나중에 개선)
        }

        private void ActivateFirstInteraction()
        {
            if (firstInteraction == null)
            {
                Debug.LogWarning("[WeekManager] First Interaction is not assigned.");
                return;
            }

            firstInteraction.SetActive(true);

            // ✅ 첫 목표 UI 표시
            InteractiveObject interaction = firstInteraction.GetComponent<InteractiveObject>();
            if (interaction != null)
            {
                UIManager.Instance.ShowGoal(interactionGoalText(interaction));
            }
        }

        // ❗ goalText 접근을 명확히 분리 (가독성용)
        private string interactionGoalText(InteractiveObject interaction)
        {
            // 현재는 단순 반환
            return interaction.GetGoalText();
        }

        #endregion
    }
}
