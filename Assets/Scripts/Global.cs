using UnityEngine;

public enum GameDifficulty
{
    Easy,
    Normal,
    Hard
}

public class Global : MonoBehaviour
{
    public static Global Instance { get; private set; }

    [Header("Player Data")]
    public string playerName;

    [Header("Game Result")]
    public GameDifficulty selectedDifficulty;
    public int attemptCount;
    public float elapsedSeconds;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void ResetAllData()
    {
        playerName = string.Empty;
        selectedDifficulty = GameDifficulty.Easy;
        attemptCount = 0;
        elapsedSeconds = 0f;
    }

    public void ResetGameResult()
    {
        attemptCount = 0;
        elapsedSeconds = 0f;
    }

    public void SetGameResult(int attempts, float seconds)
    {
        attemptCount = attempts;
        elapsedSeconds = seconds;
    }

    public string GetDifficultyText()
    {
        switch (selectedDifficulty)
        {
            case GameDifficulty.Easy:
                return "簡易";

            case GameDifficulty.Normal:
                return "中等";

            case GameDifficulty.Hard:
                return "困難";

            default:
                return string.Empty;
        }
    }

    public string GetFormattedElapsedTime()
    {
        int totalSeconds = Mathf.FloorToInt(elapsedSeconds);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return $"{minutes:00}:{seconds:00}";
    }
}