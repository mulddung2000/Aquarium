using UnityEngine;

namespace Aquarium
{
    public class InteractiveObject : MonoBehaviour
    {
        #region Variables
        [Header("Interaction Info")]
        [SerializeField] private string interactionName;
        [SerializeField] private string goalText;

        [Header("Dialogue")]
        [SerializeField] private string[] dialogueLines;

        [Header("Door (Teleport)")]
        [SerializeField] private bool isDoor;
        [SerializeField] private Transform targetSpawnPoint;

        [Header("Movement")]
        [SerializeField] private Transform interactionPoint;
        [SerializeField] private float interactionRadius = 1.2f;

        [Header("Next Interaction")]
        [SerializeField] private GameObject nextInteraction;

        [Header("Interaction ID (Save/Load)")]
        [SerializeField] private string interactionID;
        #endregion

        #region Properties
        public Transform InteractionPoint => interactionPoint;
        public float InteractionRadius => interactionRadius;
        public string InteractionID => interactionID;
        #endregion

        #region Unity Events
        private void OnEnable()
        {
            // 🔹 이 Interaction이 "현재 목표"일 때만 Goal 갱신
            if (!string.IsNullOrEmpty(goalText) && UIManager.Instance != null)
            {
                UIManager.Instance.SetGoal(goalText);
            }
        }

        private void OnMouseEnter()
        {
            if (UIManager.Instance != null)
                UIManager.Instance.ShowHover(interactionName);
        }

        private void OnMouseExit()
        {
            if (UIManager.Instance != null)
                UIManager.Instance.HideHover();
        }

        private void OnMouseDown()
        {
            if (ACon.Instance != null)
                ACon.Instance.SetTargetInteraction(this);
        }
        #endregion

        #region Interaction
        public void ExecuteInteraction()
        {
            // Door Interaction
            if (isDoor)
            {
                UIManager.Instance.SetState(UIState.Teleport);

                TeleportManager.Instance.Teleport(
                    targetSpawnPoint,
                    OnTeleportFinished
                );
                return;
            }

            // Dialogue Interaction
            if (dialogueLines == null || dialogueLines.Length == 0)
            {
                FinishInteraction();
                return;
            }

            UIManager.Instance.ShowDialogue(dialogueLines, OnDialogueFinished);
        }

        private void OnDialogueFinished()
        {
            FinishInteraction();
        }

        private void OnTeleportFinished()
        {
            FinishInteraction();
        }

        private void FinishInteraction()
        {
            // 🔹 UI 정리
            UIManager.Instance.SetState(UIState.None);

            // 🔹 다음 Interaction 처리
            if (nextInteraction != null)
            {
                nextInteraction.SetActive(true);

                // ✅ 저장 기준 Interaction ID는 "다음 Interaction"
                var nextIO = nextInteraction.GetComponent<InteractiveObject>();
                if (nextIO != null)
                {
                    InteractionRegistry.SetCurrentInteraction(nextIO.InteractionID);
                }
            }
            else
            {
                // 다음 Interaction이 없으면 DayEnd
                UIManager.Instance.ShowDayEnd();
                return;
            }

            // 🔹 현재 Interaction 비활성화
            gameObject.SetActive(false);

            // 🔹 저장 (Interaction 완료 시점)
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.SaveGame();
            }
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (interactionPoint == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
        }
#endif
    }
}
