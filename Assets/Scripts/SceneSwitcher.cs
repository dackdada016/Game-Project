using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
  private const string MainSceneName = "MainPage";
  private const string UserInfoSceneName = "UserInfoPage";
  private const string LevelSceneName = "LevelPage";
  private const string GameSceneName = "GamePage";
  private const string ResultSceneName = "ResultPage";
  private const string HelpSceneName = "HelpPage";

  public void GoToMain()
  {
    Debug.Log("GoToMain called");
    if (Global.Instance != null)
    {
      Global.Instance.ResetAllData();
    }

    SceneManager.LoadScene(MainSceneName);
  }

  public void GoToUserInfo()
  {
    SceneManager.LoadScene(UserInfoSceneName);
  }
  
  public void GoToLevel()
  {
    SceneManager.LoadScene(LevelSceneName);
  }

  public void GoToGame()
  {
    SceneManager.LoadScene(GameSceneName);
  }

  public void GoToResult()
  {
    SceneManager.LoadScene(ResultSceneName);
  }

  public void GoToHelp()
  {
    SceneManager.LoadScene(HelpSceneName);
  }

  public void StartEasyGame()
  {
    Global.Instance.selectedDifficulty = GameDifficulty.Easy;
    Global.Instance.ResetGameResult();

    SceneManager.LoadScene(GameSceneName);
  }

  public void StartNormalGame()
  {
    Global.Instance.selectedDifficulty = GameDifficulty.Normal;
    Global.Instance.ResetGameResult();

    SceneManager.LoadScene(GameSceneName);
  }

  public void StartHardGame()
  {
    Global.Instance.selectedDifficulty = GameDifficulty.Hard;
    Global.Instance.ResetGameResult();

    SceneManager.LoadScene(GameSceneName);
  }
}
