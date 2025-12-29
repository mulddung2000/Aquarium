/*using UnityEngine;

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
            UIManager.Instance.ShowDialogue(dialogueLines, FinishInteraction);
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
}*/
/*
using UnityEngine;

namespace Aquarium
{
    public class InteractiveObject : MonoBehaviour
    {
        #region Inspector

        [Header("Basic Info")]
        [SerializeField] private string interactionName;
        [SerializeField] private string goalText;

        [Header("Dialogue (Door는 비워둠)")]
        [SerializeField] private string[] dialogueLines;

        [Header("Movement")]
        [SerializeField] private Transform interactionPoint;
        [SerializeField] private float interactionRadius = 1.2f;

        [Header("Door Option")]
        [SerializeField] private bool isDoor = false;
        [SerializeField] private Transform targetSpawnPos;

        [Header("Next Interaction")]
        [SerializeField] private GameObject nextInteraction;

        #endregion

        #region Properties (ACon용)

        public Transform InteractionPoint => interactionPoint;
        public float InteractionRadius => interactionRadius;

        #endregion

        #region Unity Events

        private void OnEnable()
        {
            if (!string.IsNullOrEmpty(goalText))
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

        #region Interaction Core

        public void ExecuteInteraction()
        {
            // 🔹 Door 분기
            if (isDoor)
            {
                TeleportManager.Instance.TeleportTo(targetSpawnPos);
                FinishInteraction();
                return;
            }

            // 🔹 Dialogue Interaction
            if (dialogueLines == null || dialogueLines.Length == 0)
            {
                FinishInteraction();
                return;
            }

            UIManager.Instance.ShowDialogue(dialogueLines, FinishInteraction);
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
    }
}*/
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

        [Header("Movement")]
        [SerializeField] private Transform interactionPoint;
        [SerializeField] private float interactionRadius = 1.2f;

        [Header("Next Interaction")]
        [SerializeField] private GameObject nextInteraction;
        #endregion

        #region Properties
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
            UIManager.Instance.ShowDialogue(dialogueLines, FinishInteraction);
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
