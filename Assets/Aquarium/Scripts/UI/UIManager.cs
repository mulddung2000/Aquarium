using UnityEngine;
using TMPro;
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
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI dialogueText;

        [Header("Day End UI")]
        [SerializeField] private GameObject dayEndPanel;

        private UIState currentState = UIState.None;

        private string[] currentDialogueLines;
        private int currentDialogueIndex;
        private Action onDialogueEnd;
        #endregion

        #region Unity
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Update()
        {
            //  Dialogue 진행 입력 (좌클릭)
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

            // 전부 끄기
            hoverPanel.SetActive(false);
            dialoguePanel.SetActive(false);
            dayEndPanel.SetActive(false);

            // Goal 기본 규칙
            goalPanel.SetActive(newState != UIState.Dialogue);

            switch (newState)
            {
                case UIState.Hover:
                    // Hover는 ShowHover에서 제어
                    break;

                case UIState.Dialogue:
                    dialoguePanel.SetActive(true);
                    break;

                case UIState.DayEnd:
                    dayEndPanel.SetActive(true);
                    break;

                case UIState.Teleport:
                    // UI 없음 (입력 차단용)
                    break;

                case UIState.None:
                default:
                    break;
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

        #region Goal
        public void SetGoal(string text)
        {
            goalText.text = text;
            goalPanel.SetActive(true);
        }
        public string GetCurrentGoalText()
        {
            return goalText != null ? goalText.text : "";
        }

        #endregion

        #region Dialogue
        public void ShowDialogue(string[] lines, Action onEnd)
        {
            if (lines == null || lines.Length == 0)
                return;

            currentDialogueLines = lines;
            currentDialogueIndex = 0;
            onDialogueEnd = onEnd;

            SetState(UIState.Dialogue);

            dialoguePanel.SetActive(true);
            dialogueText.text = currentDialogueLines[currentDialogueIndex];
        }

        private void AdvanceDialogue()
        {
            currentDialogueIndex++;

            if (currentDialogueIndex < currentDialogueLines.Length)
            {
                dialogueText.text = currentDialogueLines[currentDialogueIndex];
            }
            else
            {
                EndDialogue();
            }
        }
        private void EndDialogue()
        {
            dialoguePanel.SetActive(false);
            currentDialogueLines = null;

            var callback = onDialogueEnd;
            onDialogueEnd = null;

            callback?.Invoke();
        }
        #endregion

        #region DayEnd
        /*public void ShowDayEnd()
        {
            SetState(UIState.DayEnd);
            SetGoal("End Week");
            dayEndPanel.SetActive(true);
        }*/
        public void ShowDayEnd()
        {
            SetGoal("End Week");
            SetState(UIState.DayEnd);
        }
        #endregion
    }
}