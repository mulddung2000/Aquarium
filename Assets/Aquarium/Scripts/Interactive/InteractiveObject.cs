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
        #endregion

        #region Unity Events
        private void OnEnable()
        {
            if (!string.IsNullOrEmpty(goalText))
            {
                UIManager.Instance.SetGoal(goalText);
            }
        }

        private void OnMouseEnter()
        {
            UIManager.Instance.ShowHover(interactionName);
        }

        private void OnMouseExit()
        {
            UIManager.Instance.HideHover();
        }

        private void OnMouseDown()
        {
            ACon.Instance.SetTargetInteraction(this);
        }
        #endregion
        
        #region Interaction
        public void ExecuteInteraction()
        {
            // 현재 Interaction ID 등록 (Save 기준)
            InteractionRegistry.SetCurrentInteraction(interactionID);

            // 🔹 Door Interaction
            if (isDoor)
            {
                UIManager.Instance.SetState(UIState.Teleport);
                TeleportManager.Instance.Teleport(
                    targetSpawnPoint,
                    OnTeleportFinished
                );
                return;
            }

            // 🔹 Dialogue Interaction
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
            UIManager.Instance.SetState(UIState.None);

            if (nextInteraction != null)
            {
                nextInteraction.SetActive(true);
            }
            else
            {
                UIManager.Instance.ShowDayEnd();
                return;
            }

            gameObject.SetActive(false);
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

