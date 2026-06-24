using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Attempt UI")]
    [SerializeField] private Text attemptCountText;

    [Header("Cards")]
    [SerializeField] private Card[] cards;
    [SerializeField] private Sprite[] frontSprites;
    [SerializeField] private Sprite cardBackSprite;

    [Header("Settings")]
    [SerializeField] private float mismatchDelay = 0.8f;

    private const string ResultSceneName = "ResultPage";

    private Card firstSelectedCard;
    private Card secondSelectedCard;

    private int attemptCount;
    private int matchedPairCount;
    private int requiredPairCount;

    private float elapsedSeconds;

    private bool isGameRunning;
    private bool isCheckingCards;

    private void Start()
    {
        if (Global.Instance == null)
        {
            Debug.LogError("找不到 Global。請從第一個有 Global 的 Scene 開始執行。");
            return;
        }

        SetupGame();
    }

    private void Update()
    {
        if (!isGameRunning)
        {
            return;
        }

        elapsedSeconds += Time.deltaTime;
    }

    private void SetupGame()
    {
        GameDifficulty difficulty = Global.Instance.selectedDifficulty;

        requiredPairCount = GetPairCount(difficulty);

        attemptCount = 0;
        matchedPairCount = 0;
        elapsedSeconds = 0f;

        firstSelectedCard = null;
        secondSelectedCard = null;

        isGameRunning = true;
        isCheckingCards = false;

        UpdateAttemptCountText();
        SetupCards();
    }

    private int GetPairCount(GameDifficulty difficulty)
    {
        switch (difficulty)
        {
            case GameDifficulty.Easy:
                return 4;

            case GameDifficulty.Normal:
                return 6;

            case GameDifficulty.Hard:
                return 8;

            default:
                return 4;
        }
    }

    private void SetupCards()
    {
        int activeCardCount = requiredPairCount * 2;

        if (cards == null || cards.Length < activeCardCount)
        {
            Debug.LogError($"Cards 數量不足。目前難度需要 {activeCardCount} 張卡片。");
            return;
        }

        if (frontSprites == null || frontSprites.Length < requiredPairCount)
        {
            Debug.LogError($"Front Sprites 數量不足。目前難度需要至少 {requiredPairCount} 張卡面圖片。");
            return;
        }

        if (cardBackSprite == null)
        {
            Debug.LogError("尚未設定 Card Back Sprite。");
            return;
        }

        List<int> cardIds = new List<int>();

        for (int i = 0; i < requiredPairCount; i++)
        {
            cardIds.Add(i);
            cardIds.Add(i);
        }

        Shuffle(cardIds);

        for (int i = 0; i < cards.Length; i++)
        {
            bool shouldBeActive = i < activeCardCount;

            cards[i].gameObject.SetActive(shouldBeActive);

            if (!shouldBeActive)
            {
                continue;
            }

            int cardId = cardIds[i];

            cards[i].Setup(
                this,
                cardId,
                frontSprites[cardId],
                cardBackSprite
            );
        }
    }

    public void SelectCard(Card selectedCard)
    {
        if (!isGameRunning || isCheckingCards)
        {
            return;
        }

        if (selectedCard.IsRevealed())
        {
            return;
        }

        selectedCard.Reveal();

        if (firstSelectedCard == null)
        {
            firstSelectedCard = selectedCard;
            return;
        }

        secondSelectedCard = selectedCard;

        attemptCount++;
        UpdateAttemptCountText();

        StartCoroutine(CheckSelectedCards());
    }

    private IEnumerator CheckSelectedCards()
    {
        isCheckingCards = true;

        yield return new WaitForSeconds(mismatchDelay);

        bool isMatched = firstSelectedCard.GetCardId() == secondSelectedCard.GetCardId();

        if (isMatched)
        {
            firstSelectedCard.SetMatched();
            secondSelectedCard.SetMatched();

            matchedPairCount++;

            if (matchedPairCount >= requiredPairCount)
            {
                FinishGame();
                yield break;
            }
        }
        else
        {
            firstSelectedCard.Hide();
            secondSelectedCard.Hide();
        }

        firstSelectedCard = null;
        secondSelectedCard = null;

        isCheckingCards = false;
    }

    private void FinishGame()
    {
        isGameRunning = false;

        Global.Instance.SetGameResult(attemptCount, elapsedSeconds);

        SceneManager.LoadScene(ResultSceneName);
    }

    private void UpdateAttemptCountText()
    {
        if (attemptCountText == null)
        {
            Debug.LogError("尚未綁定 Attempt Count Text。");
            return;
        }

        attemptCountText.text = attemptCount.ToString();
    }

    private void Shuffle(List<int> values)
    {
        for (int i = values.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            int temp = values[i];
            values[i] = values[randomIndex];
            values[randomIndex] = temp;
        }
    }
}