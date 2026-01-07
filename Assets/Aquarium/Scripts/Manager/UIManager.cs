using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Aquarium
{
    public enum UIState
    {
        None,
        Hover,
        Dialogue,
        Teleport,
        DayEnd
    }

    public class UIManager : MonoBehaviour
    {
        #region Singleton
        public static UIManager Instance;
        #endregion

        #region Variables
        [Header("Goal UI")]
        [SerializeField] private GameObject goalPanel;
        [SerializeField] private TextMeshProUGUI goalText;

        [Header("Hover UI")]
        [SerializeField] private GameObject hoverPanel;
        [SerializeField] private TextMeshProUGUI hoverText;

        [Header("Dialogue UI")]
        [SerializeField] private GameObject dialogueContainer;
        [SerializeField] private Image dialogueCharacterImage;
        [SerializeField] private TextMeshProUGUI dialogueText;

        [Header("Day End UI")]
        [SerializeField] private GameObject dayEndPanel;
        [SerializeField] private TextMeshProUGUI dayEndText;

        private UIState currentState = UIState.None;

        private DialogueLine[] currentDialogueLines;
        private int currentDialogueIndex;
        private Action onDialogueEnd;

        // 🔑 현재 Goal 데이터 (UI와 분리)
        private string currentGoalText = "";
        #endregion

        #region Unity
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            dialogueContainer.SetActive(false);
            hoverPanel.SetActive(false);
            dayEndPanel.SetActive(false);
            goalPanel.SetActive(false);
        }

        private void Update()
        {
            if (currentState == UIState.Dialogue && Input.GetMouseButtonDown(0))
            {
                AdvanceDialogue();
            }
        }
        #endregion

        #region State
        public void SetState(UIState newState)
        {
            currentState = newState;

            hoverPanel.SetActive(false);
            dialogueContainer.SetActive(false);
            dayEndPanel.SetActive(false);

            switch (newState)
            {
                case UIState.Dialogue:
                    dialogueContainer.SetActive(true);
                    goalPanel.SetActive(false); // 🔥 Dialogue 중 Goal 숨김
                    break;

                case UIState.DayEnd:
                    dayEndPanel.SetActive(true);
                    break;

                default:
                    RestoreGoal(); // 🔥 일반 상태에서는 Goal 복원
                    break;
            }
        }
        #endregion

        #region Goal
        public void SetGoal(string text)
        {
            currentGoalText = text;

            if (goalText != null)
                goalText.text = text;

            if (currentState != UIState.Dialogue && !string.IsNullOrEmpty(text))
                goalPanel.SetActive(true);
        }

        public string GetCurrentGoalText()
        {
            return currentGoalText;
        }

        public void RestoreGoal()
        {
            if (!string.IsNullOrEmpty(currentGoalText))
            {
                goalText.text = currentGoalText;
                goalPanel.SetActive(true);
            }
        }
        #endregion

        #region Hover
        public void ShowHover(string text)
        {
            if (currentState != UIState.None && currentState != UIState.Hover)
                return;

            hoverText.text = text;
            hoverPanel.SetActive(true);
            currentState = UIState.Hover;
        }

        public void HideHover()
        {
            hoverPanel.SetActive(false);
            if (currentState == UIState.Hover)
                currentState = UIState.None;
        }
        #endregion

        #region Dialogue
        public void ShowDialogue(DialogueLine[] lines, Action onEnd)
        {
            if (lines == null || lines.Length == 0)
                return;

            currentDialogueLines = lines;
            currentDialogueIndex = 0;
            onDialogueEnd = onEnd;

            SetState(UIState.Dialogue);
            ShowCurrentDialogue();
        }

        private void ShowCurrentDialogue()
        {
            if (currentDialogueIndex >= currentDialogueLines.Length)
            {
                EndDialogue();
                return;
            }

            DialogueLine line = currentDialogueLines[currentDialogueIndex];
            dialogueText.text = line.text;

            if (line.speakerSprite != null)
            {
                dialogueCharacterImage.sprite = line.speakerSprite;
                dialogueCharacterImage.gameObject.SetActive(true);
            }
            else
            {
                dialogueCharacterImage.gameObject.SetActive(false);
            }
        }

        private void AdvanceDialogue()
        {
            currentDialogueIndex++;
            ShowCurrentDialogue();
        }

        private void EndDialogue()
        {
            dialogueContainer.SetActive(false);
            currentDialogueLines = null;

            SetState(UIState.None);

            var callback = onDialogueEnd;
            onDialogueEnd = null;
            callback?.Invoke();
        }
        #endregion

        #region DayEnd
        public void ShowDayEnd()
        {
            // 🔑 Goal은 플레이어 행동 가이드 (한글)
            SetGoal("다음 주로 넘어가기");

            // 🔑 DayEnd UI는 상태 이름 (영어)
            if (dayEndText != null)
                dayEndText.text = "End Week";

            SetState(UIState.DayEnd);
        }
        #endregion

        public void ForceResetState()
        {
            currentState = UIState.None;

            hoverPanel.SetActive(false);
            dialogueContainer.SetActive(false);
            dayEndPanel.SetActive(false);

            RestoreGoal();
        }
    }
}
