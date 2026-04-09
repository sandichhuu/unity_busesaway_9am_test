using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BusesAway.Bus;
using BusesAway.Managers;

namespace BusesAway.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private int currentLevel = 1;
        [SerializeField] private int moveCount = 0;
        [SerializeField] private int maxMoves = -1;

        private List<BusController> activeBuses = new List<BusController>();
        private bool gameEnded = false;

        public int CurrentLevel => currentLevel;
        public int MoveCount => moveCount;
        public int MaxMoves => maxMoves;
        public bool HasMoveLimit => maxMoves > 0;
        public int RemainingMoves => HasMoveLimit ? maxMoves - moveCount : -1;

        public delegate void GameEvent();
        public event GameEvent OnWin;
        public event GameEvent OnLose;
        public event GameEvent OnMoveMade;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void InitializeLevel()
        {
            RefreshBusList();
            moveCount = 0;
            gameEnded = false;
        }

        public void RefreshBusList()
        {
            activeBuses.Clear();
            BusController[] buses = FindObjectsByType<BusController>(FindObjectsSortMode.None);
            foreach (var bus in buses)
            {
                if (!bus.IsExiting)
                {
                    activeBuses.Add(bus);
                }
            }
        }

        public void OnMoveCompleted()
        {
            if (gameEnded) return;

            moveCount++;
            OnMoveMade?.Invoke();

            if (HasMoveLimit && moveCount >= maxMoves)
            {
                CheckDeadlock();
            }
        }

        public void OnBusExited(BusController bus)
        {
            if (gameEnded) return;

            activeBuses.Remove(bus);
            CheckWinCondition();
        }

        private void CheckWinCondition()
        {
            RefreshBusList();

            if (activeBuses.Count == 0)
            {
                GameWin();
            }
            else
            {
                CheckDeadlock();
            }
        }

        private void CheckDeadlock()
        {
            if (gameEnded) return;

            RefreshBusList();

            bool anyValidMove = false;
            foreach (var bus in activeBuses)
            {
                if (MovementManager.Instance != null && MovementManager.Instance.HasValidMove(bus))
                {
                    anyValidMove = true;
                    break;
                }
            }

            if (!anyValidMove)
            {
                GameLose();
            }
        }

        private void GameWin()
        {
            if (gameEnded) return;
            gameEnded = true;

            Debug.Log("Level Complete! Moves used: " + moveCount);
            OnWin?.Invoke();
        }

        private void GameLose()
        {
            if (gameEnded) return;
            gameEnded = true;

            Debug.Log("Game Over - Deadlock!");
            OnLose?.Invoke();
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadNextLevel()
        {
            currentLevel++;
            RestartLevel();
        }

        public void LoadLevel(int levelNumber)
        {
            currentLevel = levelNumber;
            RestartLevel();
        }
    }
}
