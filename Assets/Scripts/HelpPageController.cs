using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpPageController : MonoBehaviour
{
    [Header("Help Content")]
    [SerializeField] private Image helpImage;
    [SerializeField] private Sprite[] helpSprites;

    [Header("Navigation")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backToMainButton;

    private int currentPageIndex;

    private void Start()
    {
        ShowPage(0);
    }

    public void ShowNextPage()
    {
        if (currentPageIndex >= helpSprites.Length - 1)
        {
            return;
        }

        ShowPage(currentPageIndex + 1);
    }

    public void ShowPreviousPage()
    {
        if (currentPageIndex <= 0)
        {
            return;
        }

        ShowPage(currentPageIndex - 1);
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MainPage");
    }

    private void ShowPage(int pageIndex)
    {
        currentPageIndex = pageIndex;
        helpImage.sprite = helpSprites[currentPageIndex];

        bool isFirstPage = currentPageIndex == 0;
        bool isLastPage = currentPageIndex == helpSprites.Length - 1;

        previousButton.gameObject.SetActive(!isFirstPage);
        nextButton.gameObject.SetActive(!isLastPage);
        backToMainButton.gameObject.SetActive(isLastPage);
    }
}