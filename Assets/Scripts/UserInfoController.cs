using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInfoController : MonoBehaviour
{
  [SerializeField] private InputField nameInputField;

  private const string LevelSceneName = "LevelPage";

  public void SubmitUserInfo()
  {
      string playerName = nameInputField.text.Trim();

      if (string.IsNullOrEmpty(playerName))
      {
          Debug.Log("請輸入姓名");
          return;
      }
      
      Global.Instance.playerName = playerName;

      SceneManager.LoadScene(LevelSceneName);
  }
}