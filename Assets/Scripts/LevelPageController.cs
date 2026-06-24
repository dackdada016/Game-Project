using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPageController : MonoBehaviour
{
  private const string MainSceneName = "MainPage";
  private const string GameSceneName = "GamePage";

  public void SelectEasy()
  {
    StartGame(GameDifficulty.Easy);
  }

  public void SelectNormal()
  {
    StartGame(GameDifficulty.Normal);
  }

  public void SelectHard()
  {
    StartGame(GameDifficulty.Hard);
  }

  private void StartGame(GameDifficulty difficulty)
  {
    if (Global.Instance == null)
    {
      Debug.LogError("找不到 Global。請從第一個包含 Global 的 Scene 開始執行。");
      return;
    }
    
    Global.Instance.selectedDifficulty = difficulty;
    Global.Instance.ResetGameResult();

    Debug.Log($"目前難度：{difficulty}");

    SceneManager.LoadScene(GameSceneName);
  }
}
