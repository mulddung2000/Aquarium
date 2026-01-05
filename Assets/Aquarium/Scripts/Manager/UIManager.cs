using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

        private UIState currentState = UIState.None;

        private DialogueLine[] currentDialogueLines;
        private int currentDialogueIndex;
        private Action onDialogueEnd;
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

            // 초기 UI 상태
            hoverPanel.SetActive(false);
            dialogueContainer.SetActive(false);
            dayEndPanel.SetActive(false);
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

            goalPanel.SetActive(newState != UIState.Dialogue);

            switch (newState)
            {
                case UIState.Dialogue:
                    dialogueContainer.SetActive(true);
                    break;

                case UIState.DayEnd:
                    dayEndPanel.SetActive(true);
                    break;

                case UIState.Teleport:
                    // Teleport 시 Hover/Dialogue 끄기
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

            var callback = onDialogueEnd;
            onDialogueEnd = null;

            callback?.Invoke();
        }
        #endregion

        #region DayEnd
        public void ShowDayEnd()
        {
            SetGoal("End Week");
            SetState(UIState.DayEnd);
        }
        #endregion
    }
}
