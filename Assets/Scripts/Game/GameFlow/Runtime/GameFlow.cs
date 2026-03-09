using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameFlow
{
    internal class GameFlow : MonoBehaviour
    {
        private enum State
        {
            None,
            Loading,
            Playing,
            Paused,
            Won,
            Lost
        }

        private int _startLevelIndex = 1;

        private State _state;
        private LevelController _level;
        private int _currentLevelIndex;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        private void Start()
        {
            StartGame();
        }

        internal void StartGame()
        {
            LoadLevel(_startLevelIndex);
        }

        internal void LoadLevel(int levelIndex)
        {
            CleanupLevel();

            _state = State.Loading;
            _currentLevelIndex = levelIndex;

            SceneManager.sceneLoaded -= OnLevelSceneLoaded;
            SceneManager.sceneLoaded += OnLevelSceneLoaded;

            SceneManager.LoadScene($"Level_{levelIndex}");
        }

        internal void RestartLevel()
        {
            if (_state == State.Loading)
                return;

            LoadLevel(_currentLevelIndex);
        }

        internal void Pause(bool isPaused)
        {
            if (_state != State.Playing && _state != State.Paused)
                return;

            _state = isPaused ? State.Paused : State.Playing;
            Time.timeScale = isPaused ? 0f : 1f;
        }

        internal void ContinueAfterWin()
        {
            if (_state != State.Won)
                return;

            LoadLevel(_currentLevelIndex + 1);
        }

        internal void RetryAfterLose()
        {
            if (_state != State.Lost)
                return;

            RestartLevel();
        }

        private void OnLevelSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnLevelSceneLoaded;

            LevelInstaller installer = FindFirstObjectByType<LevelInstaller>();
            
            if (installer == null)
            {
                Debug.LogError("LevelInstaller not found in scene.");
                _state = State.None;
                return;
            }

            _level = installer.BuildLevel();
            BindLevel(_level);

            _state = State.Playing;
            _level.StartLevel();
        }

        private void BindLevel(LevelController level)
        {
            level.Won += OnLevelWon;
            level.Lost += OnLevelLost;
        }

        private void UnbindLevel(LevelController level)
        {
            level.Won -= OnLevelWon;
            level.Lost -= OnLevelLost;
        }

        private void CleanupLevel()
        {
            Time.timeScale = 1f;

            if (_level == null)
                return;

            UnbindLevel(_level);
            _level.StopLevel();
            _level = null;
        }

        private void OnLevelWon()
        {
            Debug.Log("Победа!");
            
            //_state = State.Won;
            // UI покажет экран победы
        }

        private void OnLevelLost()
        {
            Debug.Log("Поражение :(");
           // _state = State.Lost;
            // UI покажет экран поражения
        }
    }
}