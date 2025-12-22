using UnityEngine;
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
}


/*using UnityEngine;
using TMPro;

namespace Aquarium
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [Header("Dialogue UI")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI dialogueText;

        [Header("Hover UI")]
        [SerializeField] private GameObject hoverPanel;
        [SerializeField] private TextMeshProUGUI hoverText;

        [Header("Goal UI")]
        [SerializeField] private GameObject goalPanel;

        private bool isDialogueActive;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (isDialogueActive && Input.GetMouseButtonDown(0))
            {
                FindAnyObjectByType<InteractiveObject>()?.AdvanceDialogue();
            }
        }

        #region Dialogue
        public void ShowDialogue(string text)
        {
            dialogueText.text = text;
            dialoguePanel.SetActive(true);
        }

        public void HideDialogue()
        {
            dialoguePanel.SetActive(false);
        }

        public void SetDialogueState(bool active)
        {
            isDialogueActive = active;

            if (active)
            {
                hoverPanel.SetActive(false);
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
            if (isDialogueActive) return;

            hoverText.text = text;
            hoverPanel.SetActive(true);
        }

        public void HideHover()
        {
            hoverPanel.SetActive(false);
        }
        #endregion
    }
}
*/