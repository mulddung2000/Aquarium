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

        [Header("Dialogue Lines")]
        [TextArea]
        [SerializeField] private string[] dialogueLines;

        [Header("Goal Text")]
        // 이 Interaction이 "다음 목표"가 되었을 때 표시될 문구
        [SerializeField] private string goalText;

        [Header("Next Interaction")]
        [SerializeField] private GameObject nextInteraction;

        private int currentLineIndex = 0;
        private bool isDialogueActive = false;

        // 대화 시작 클릭과 대화 진행 클릭 분리용
        private bool canAdvanceDialogue = false;

        #endregion


        #region Unity Event Methods

        private void OnMouseDown()
        {
            // 좌클릭만 허용
            if (!Input.GetMouseButtonDown(0))
                return;

            // 대화 중이 아닐 때만 시작
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

            currentLineIndex = 0;

            dialoguePanel.SetActive(true);

            if (dialogueLines == null || dialogueLines.Length == 0)
            {
                EndDialogue();
                return;
            }

            dialogueText.text = dialogueLines[currentLineIndex];

            // 다음 프레임부터 입력 허용
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

            // ✅ 다음 Interaction 활성화
            if (nextInteraction != null)
            {
                nextInteraction.SetActive(true);

                // ✅ 다음 Interaction의 goalText를 UI에 표시
                InteractiveObject next = nextInteraction.GetComponent<InteractiveObject>();
                if (next != null)
                {
                    UIManager.Instance.ShowGoal(next.goalText);
                }
            }

            // 현재 Interaction은 완료되었으므로 비활성화
            gameObject.SetActive(false);
        }

        #endregion

        #region Getter

        public string GetGoalText()
        {
            return goalText;
        }

        #endregion
    }
}