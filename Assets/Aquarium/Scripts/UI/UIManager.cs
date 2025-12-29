/*using UnityEngine;
using TMPro;
using System;

namespace Aquarium
{
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

        private bool isDialogueActive = false;

        // Dialogue control
        private string[] currentLines;
        private int currentIndex;
        private Action onDialogueFinished;
        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Update()
        {
            if (!isDialogueActive)
                return;

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                AdvanceDialogue();
            }
        }
        #endregion

        #region Hover UI
        public void ShowHover(string text)
        {
            if (isDialogueActive)
                return;

            hoverText.text = text;
            hoverPanel.SetActive(true);
        }

        public void HideHover()
        {
            hoverPanel.SetActive(false);
        }
        #endregion

        #region Goal UI
        public void SetGoal(string text)
        {
            goalText.text = text;
            goalPanel.SetActive(true);
        }

        public void HideGoal()
        {
            goalPanel.SetActive(false);
        }
        #endregion

        #region Dialogue UI
        public void ShowDialogue(string[] lines, Action onFinished)
        {
            if (lines == null || lines.Length == 0)
                return;

            currentLines = lines;
            currentIndex = 0;
            onDialogueFinished = onFinished;

            isDialogueActive = true;

            HideHover();
            HideGoal();

            dialoguePanel.SetActive(true);
            dialogueText.text = currentLines[currentIndex];
        }

        private void AdvanceDialogue()
        {
            currentIndex++;

            if (currentIndex < currentLines.Length)
            {
                dialogueText.text = currentLines[currentIndex];
            }
            else
            {
                HideDialogue();
                onDialogueFinished?.Invoke();
            }
        }

        public void HideDialogue()
        {
            isDialogueActive = false;
            dialoguePanel.SetActive(false);
        }
        #endregion

        #region Day End
        public void ShowDayEnd()
        {
            HideHover();
            SetGoal("End Week");
            dayEndPanel.SetActive(true);
        }
        #endregion
    }
}*/
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
            //  Dialogue ÁøÇà ÀÔ·Â (ÁÂÅ¬¸¯)
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

            // °øÅë Á¤¸®
            hoverPanel.SetActive(false);
            dialoguePanel.SetActive(false);
            dayEndPanel.SetActive(false);

            //  ÇÙ½É: Dialogue Áß Goal ¼û±è
            if (newState == UIState.Dialogue)
            {
                goalPanel.SetActive(false);
            }
            else
            {
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

        #region Goal
        public void SetGoal(string text)
        {
            goalText.text = text;
            goalPanel.SetActive(true);
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
            onDialogueEnd?.Invoke();
            onDialogueEnd = null;

            SetState(UIState.None);
        }
        #endregion

        #region DayEnd
        public void ShowDayEnd()
        {
            SetState(UIState.DayEnd);
            SetGoal("End Week");
            dayEndPanel.SetActive(true);
        }
        #endregion
    }
}
