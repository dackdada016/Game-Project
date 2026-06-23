using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPageController : MonoBehaviour
{
    [Header("Google Form")]
    [SerializeField] private GoogleFormUploader googleFormUploader;

    [Header("Result UI")]
    [SerializeField] private Text userNameLabel;
    [SerializeField] private Text difficultyText;
    [SerializeField] private Text attemptText;
    [SerializeField] private Text timeText;

    [Header("Result Animation")]
    [SerializeField] private CanvasGroup userNameCanvasGroup;
    [SerializeField] private CanvasGroup resultPanelCanvasGroup;
    [SerializeField] private float fadeDuration = 0.45f;
    [SerializeField] private float panelDelay = 0.2f;

    private const string LevelSceneName = "LevelPage";

    private void Awake()
    {
        if (userNameCanvasGroup != null)
        {
            userNameCanvasGroup.alpha = 0f;
        }

        if (resultPanelCanvasGroup != null)
        {
            resultPanelCanvasGroup.alpha = 0f;
        }
    }

    private void Start()
    {
        ShowResult();

        if (googleFormUploader == null)
        {
            Debug.LogError("尚未綁定 GoogleFormUploader。");
        }
        else
        {
            googleFormUploader.UploadCurrentResult();
        }

        StartCoroutine(PlayResultFadeIn());
    }

    private void ShowResult()
    {
        if (Global.Instance == null)
        {
            Debug.LogError("找不到 Global。請從 Main Scene 開始執行完整流程。");
            return;
        }

        userNameLabel.text = Global.Instance.playerName;
        difficultyText.text = GetDifficultyName(Global.Instance.selectedDifficulty);
        attemptText.text = Global.Instance.attemptCount.ToString();
        timeText.text = Global.Instance.GetFormattedElapsedTime();
    }

    private IEnumerator PlayResultFadeIn()
    {
        yield return FadeCanvasGroup(userNameCanvasGroup, 0f, 1f);

        yield return new WaitForSeconds(panelDelay);

        yield return FadeCanvasGroup(resultPanelCanvasGroup, 0f, 1f);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha)
    {
        if (canvasGroup == null)
        {
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / fadeDuration);

            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }

    public void RestartGame()
    {
        if (Global.Instance == null)
        {
            Debug.LogError("找不到 Global。");
            return;
        }

        Global.Instance.ResetGameResult();

        SceneManager.LoadScene(LevelSceneName);
    }

    private string GetDifficultyName(GameDifficulty difficulty)
    {
        switch (difficulty)
        {
            case GameDifficulty.Easy:
                return "簡易";

            case GameDifficulty.Normal:
                return "普通";

            case GameDifficulty.Hard:
                return "困難";

            default:
                return "簡易";
        }
    }
}