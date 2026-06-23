using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleFormUploader : MonoBehaviour
{
    private const string FormResponseUrl =
        "https://docs.google.com/forms/d/e/1FAIpQLSe0ohSiMlp-KfJdCpdiWUeEypHFSPdWyG5jpW6vBeZfvDpj-A/formResponse";

    private const string PlayerNameEntryId = "entry.1857103850";
    private const string DifficultyEntryId = "entry.91882363";
    private const string AttemptCountEntryId = "entry.557705570";
    private const string ElapsedTimeEntryId = "entry.193529864";

    public void UploadCurrentResult()
    {
        if (Global.Instance == null)
        {
            Debug.LogError("找不到 Global，無法送出 Google Form。");
            return;
        }

        StartCoroutine(UploadResultCoroutine());
    }

    private IEnumerator UploadResultCoroutine()
    {
        string formData =
            $"{PlayerNameEntryId}={EscapeValue(Global.Instance.playerName)}&" +
            $"{DifficultyEntryId}={EscapeValue(Global.Instance.GetDifficultyText())}&" +
            $"{AttemptCountEntryId}={EscapeValue(Global.Instance.attemptCount.ToString())}&" +
            $"{ElapsedTimeEntryId}={EscapeValue(Global.Instance.GetFormattedElapsedTime())}";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(formData);

        using UnityWebRequest request = new UnityWebRequest(FormResponseUrl, UnityWebRequest.kHttpVerbPOST);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        request.SetRequestHeader("Accept", "*/*");

        yield return request.SendWebRequest();

        Debug.Log($"Google Form Response Code：{request.responseCode}");
        Debug.Log($"Google Form Result：{request.result}");

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Google Form 資料送出成功。");
            yield break;
        }

        Debug.LogError($"Google Form 資料送出失敗：{request.error}");
        Debug.LogError($"Response Body：{request.downloadHandler.text}");
    }

    private string EscapeValue(string value)
    {
        return UnityWebRequest.EscapeURL(value ?? string.Empty);
    }
}