/*using System;
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
            if (!string.IsNullOrEmpty(locationID))
            {
                InteractionRegistry.SetCurrentLocation(locationID);
            }

            if (!string.IsNullOrEmpty(goalText) && UIManager.Instance != null)
            {
                UIManager.Instance.SetGoal(goalText);
            }
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
            // Door Interaction
            if (isDoor)
            {
                UIManager.Instance.SetState(UIState.Teleport);

                TeleportManager.Instance.Teleport(targetSpawnPoint,locationID,OnTeleportFinished);
                return;
            }

            // Dialogue Interaction
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
                {
                    InteractionRegistry.SetCurrentInteraction(nextIO.InteractionID);
                }
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
*/
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
        [SerializeField] private string targetLocationID; // ✅ 목적지 Location

        [Header("Movement")]
        [SerializeField] private Transform interactionPoint;
        [SerializeField] private float interactionRadius = 1.2f;

        [Header("Next Interaction")]
        [SerializeField] private GameObject nextInteraction;

        [Header("Interaction ID (Save/Load)")]
        [SerializeField] private string interactionID;

        [Header("Location ID")]
        [Tooltip("이 Interaction이 속한 장소 ID")]
        [SerializeField] private string locationID;
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
            // 현재 Interaction 활성화 시 현재 위치 확정
            if (!string.IsNullOrEmpty(locationID))
            {
                InteractionRegistry.SetCurrentLocation(locationID);
            }

            if (!string.IsNullOrEmpty(goalText) && UIManager.Instance != null)
            {
                UIManager.Instance.SetGoal(goalText);
            }
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
                    OnTeleportFinished
                );
                return;
            }

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
            // ✅ 목적지 Location 기준으로 카메라 전환
            if (!string.IsNullOrEmpty(targetLocationID))
            {
                LocationEventHub.SetLocation(targetLocationID);
            }

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
                {
                    InteractionRegistry.SetCurrentInteraction(nextIO.InteractionID);
                }
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
