using UnityEngine;
using UnityEngine.UIElements;
using BusesAway.Managers;

namespace BusesAway.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private bool useUIToolkit = true;

        private Label moveCountLabel;
        private Label remainingMovesLabel;
        private VisualElement winScreen;
        private VisualElement loseScreen;
        private Button restartButton;
        private Button nextLevelButton;

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnWin += ShowWinScreen;
                GameManager.Instance.OnLose += ShowLoseScreen;
                GameManager.Instance.OnMoveMade += UpdateUI;
            }

            if (useUIToolkit && uiDocument != null)
            {
                SetupUIToolkit();
            }
        }

        private void SetupUIToolkit()
        {
            var root = uiDocument.rootVisualElement;

            moveCountLabel = root.Q<Label>("move-count");
            remainingMovesLabel = root.Q<Label>("remaining-moves");
            winScreen = root.Q<VisualElement>("win-screen");
            loseScreen = root.Q<VisualElement>("lose-screen");
            restartButton = root.Q<Button>("restart-button");
            nextLevelButton = root.Q<Button>("next-level-button");

            if (restartButton != null)
                restartButton.clicked += OnRestartClicked;

            if (nextLevelButton != null)
                nextLevelButton.clicked += OnNextLevelClicked;

            if (winScreen != null)
                winScreen.style.display = DisplayStyle.None;

            if (loseScreen != null)
                loseScreen.style.display = DisplayStyle.None;
        }

        private void Update()
        {
            if (!useUIToolkit || uiDocument == null)
            {
                UpdateLegacyUI();
            }
        }

        private void UpdateLegacyUI()
        {
            // For legacy UI (Unity UI), update here
        }

        private void UpdateUI()
        {
            if (GameManager.Instance == null) return;

            if (moveCountLabel != null)
                moveCountLabel.text = $"Moves: {GameManager.Instance.MoveCount}";

            if (remainingMovesLabel != null)
            {
                if (GameManager.Instance.HasMoveLimit)
                {
                    remainingMovesLabel.text = $"Remaining: {GameManager.Instance.RemainingMoves}";
                }
                else
                {
                    remainingMovesLabel.style.display = DisplayStyle.None;
                }
            }
        }

        private void ShowWinScreen()
        {
            if (winScreen != null)
            {
                winScreen.style.display = DisplayStyle.Flex;
            }
            else
            {
                Debug.Log("Level Complete!");
            }
        }

        private void ShowLoseScreen()
        {
            if (loseScreen != null)
            {
                loseScreen.style.display = DisplayStyle.Flex;
            }
            else
            {
                Debug.Log("Game Over!");
            }
        }

        private void OnRestartClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RestartLevel();
            }
        }

        private void OnNextLevelClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadNextLevel();
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnWin -= ShowWinScreen;
                GameManager.Instance.OnLose -= ShowLoseScreen;
                GameManager.Instance.OnMoveMade -= UpdateUI;
            }
        }
    }
}
