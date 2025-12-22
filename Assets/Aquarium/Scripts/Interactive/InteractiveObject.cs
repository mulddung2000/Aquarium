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
        // ❗ TextArea 제거 (배열에 사용하면 직렬화 깨짐)
        [SerializeField] private string[] dialogueLines;

        [Header("Movement")]
        [SerializeField] private Transform interactionPoint;
        [SerializeField] private float interactionRadius = 1.2f;

        [Header("Next Interaction")]
        [SerializeField] private GameObject nextInteraction;
        #endregion

        #region Properties (ACon 사용)
        public Transform InteractionPoint => interactionPoint;
        public float InteractionRadius => interactionRadius;
        #endregion

        #region Unity Events
        private void OnEnable()
        {
            UIManager.Instance.SetGoal(goalText);
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
            Debug.Log($"[DialogueLines] Count = {dialogueLines.Length}");
            UIManager.Instance.ShowDialogue(
                dialogueLines,
                FinishInteraction
            );
        }

        private void FinishInteraction()
        {
            if (nextInteraction != null)
            {
                nextInteraction.SetActive(true);
            }
            else
            {
                UIManager.Instance.ShowDayEnd();
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
/*using UnityEngine;

namespace Aquarium
{
    public class InteractiveObject : MonoBehaviour
    {
        [Header("Interaction")]
        [SerializeField] private Transform interactionPoint;
        [SerializeField] private float interactionRadius = 1.2f;

        [Header("Dialogue")]
        [SerializeField] private string[] dialogueLines;

        private int currentIndex;
        private bool isDialogueActive;

        public Transform InteractionPoint => interactionPoint;
        public float InteractionRadius => interactionRadius;

        private void OnMouseEnter()
        {
            UIManager.Instance.ShowHover(gameObject.name);
        }

        private void OnMouseExit()
        {
            UIManager.Instance.HideHover();
        }

        private void OnMouseDown()
        {
            ACon.Instance.MoveToInteraction(this);
        }

        public void ExecuteInteraction()
        {
            if (dialogueLines == null || dialogueLines.Length == 0)
                return;

            isDialogueActive = true;
            currentIndex = 0;

            UIManager.Instance.SetDialogueState(true);
            UIManager.Instance.ShowDialogue(dialogueLines[currentIndex]);
        }

        public void AdvanceDialogue()
        {
            if (!isDialogueActive)
                return;

            currentIndex++;

            if (currentIndex < dialogueLines.Length)
            {
                UIManager.Instance.ShowDialogue(dialogueLines[currentIndex]);
            }
            else
            {
                EndDialogue();
            }
        }

        private void EndDialogue()
        {
            isDialogueActive = false;

            UIManager.Instance.HideDialogue();
            UIManager.Instance.SetDialogueState(false);
        }
    }
}
*/