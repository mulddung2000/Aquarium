using TMPro;
using UnityEngine;
using System.Collections;

namespace Aquarium
{
    public class InteractiveObject : MonoBehaviour
    {
        #region Variables
        [Header("Dialogue UI")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI dialogueText;

        [Header("Dialogue Lines (Inspector Input)")]
        [SerializeField] private string[] dialogueLines;   // 🔹 Inspector에서 입력

        [Header("Goal")]
        [SerializeField] private string goalText;          // 🔹 이 Interaction이 활성화되면 표시될 목표

        [Header("Hover")]
        [SerializeField] private string hoverText = "Interact";

        [Header("Next Interaction")]
        [SerializeField] private GameObject nextInteraction;

        private int currentLineIndex = 0;
        private bool isDialogueActive = false;
        private bool canAdvanceDialogue = false;
        #endregion

        #region Unity Event Methods
        private void OnEnable()
        {
            // 🔹 Interaction이 활성화되는 순간 = 현재 목표
            if (!string.IsNullOrEmpty(goalText))
            {
                UIManager.Instance.SetGoal(goalText);
            }
        }

        private void OnMouseEnter()
        {
            if (!gameObject.activeSelf)
                return;

            UIManager.Instance.ShowHover(hoverText);
        }

        private void OnMouseExit()
        {
            UIManager.Instance.HideHover();
        }

        private void OnMouseDown()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            if (!isDialogueActive)
            {
                StartDialogue();
            }
        }

        private void Update()
        {
            if (!isDialogueActive || !canAdvanceDialogue)
                return;

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                AdvanceDialogue();
            }
        }
        #endregion

        #region Dialogue Flow
        private void StartDialogue()
        {
            isDialogueActive = true;
            canAdvanceDialogue = false;

            UIManager.Instance.SetDialogueState(true);
            UIManager.Instance.HideHover();

            currentLineIndex = 0;

            dialoguePanel.SetActive(true);
            dialogueText.text = dialogueLines[currentLineIndex];

            StartCoroutine(EnableAdvanceNextFrame());
        }

        private IEnumerator EnableAdvanceNextFrame()
        {
            yield return null;
            canAdvanceDialogue = true;
        }

        private void AdvanceDialogue()
        {
            currentLineIndex++;

            if (currentLineIndex < dialogueLines.Length)
            {
                dialogueText.text = dialogueLines[currentLineIndex];
            }
            else
            {
                EndDialogue();
            }
        }

        private void EndDialogue()
        {
            isDialogueActive = false;
            canAdvanceDialogue = false;

            dialoguePanel.SetActive(false);
            UIManager.Instance.SetDialogueState(false);

            // 🔹 다음 Interaction이 있으면 계속 진행
            if (nextInteraction != null)
            {
                nextInteraction.SetActive(true);
            }
            else
            {
                // 🔹 마지막 Interaction → 주차 종료
                UIManager.Instance.ShowDayEnd();
            }

            gameObject.SetActive(false);
        }

        #endregion
    }
}
