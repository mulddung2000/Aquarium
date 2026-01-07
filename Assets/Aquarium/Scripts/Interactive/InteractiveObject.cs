using System;
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
        [SerializeField] private DialogueLine[] dialogueLines;

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

        [Header("Location ID")]
        [SerializeField] private string locationID;

        [Header("Indicator")]
        [SerializeField] private InteractionIndicator interactionIndicator;
        #endregion

        #region Properties
        public Transform InteractionPoint => interactionPoint;
        public float InteractionRadius => interactionRadius;
        public string InteractionID => interactionID;
        public string LocationID => locationID;
        #endregion

        #region Unity Events
        private void OnEnable()
        {
            // Indicator는 항상 즉시 ON
            interactionIndicator?.Show();

            // 🔥 로드 중이면 UI / Registry 건드리지 않음
            if (SaveManager.Instance != null && SaveManager.Instance.IsLoading)
                return;

            if (!string.IsNullOrEmpty(interactionID))
                InteractionRegistry.SetCurrentInteraction(interactionID);

            if (!string.IsNullOrEmpty(locationID))
                InteractionRegistry.SetCurrentLocation(locationID);

            if (!string.IsNullOrEmpty(goalText) && UIManager.Instance != null)
                UIManager.Instance.SetGoal(goalText);
        }

        private void OnDisable()
        {
            interactionIndicator?.Hide();
        }

        private void OnMouseEnter()
        {
            UIManager.Instance?.ShowHover(interactionName);
        }

        private void OnMouseExit()
        {
            UIManager.Instance?.HideHover();
        }

        private void OnMouseDown()
        {
            ACon.Instance?.SetTargetInteraction(this);
        }
        #endregion

        #region Interaction
        public void ExecuteInteraction()
        {
            if (isDoor)
            {
                UIManager.Instance.SetState(UIState.Teleport);
                TeleportManager.Instance.Teleport(
                    targetSpawnPoint,
                    locationID,
                    OnTeleportFinished
                );
                return;
            }

            if (dialogueLines == null || dialogueLines.Length == 0)
            {
                FinishInteraction();
                return;
            }

            UIManager.Instance.ShowDialogue(
                dialogueLines,
                (Action)OnDialogueFinished
            );
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

                var nextIO = nextInteraction.GetComponent<InteractiveObject>();
                if (nextIO != null)
                    InteractionRegistry.SetCurrentInteraction(nextIO.InteractionID);
            }
            else
            {
                UIManager.Instance.ShowDayEnd();
                return;
            }

            gameObject.SetActive(false);
            SaveManager.Instance?.SaveGame();
        }
        #endregion
    }
}
