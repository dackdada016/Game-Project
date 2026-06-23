using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("Card UI")]
    [SerializeField] private Image cardImage;
    [SerializeField] private Button cardButton;

    [Header("Animation")]
    [SerializeField] private float flipDuration = 0.16f;

    private GameManager gameManager;
    private Sprite frontSprite;
    private Sprite backSprite;
    private int cardId;

    private bool isRevealed;
    private bool isMatched;
    private bool isAnimating;

    private Vector3 defaultScale;

    private void Awake()
    {
        if (cardImage == null)
        {
            cardImage = GetComponent<Image>();
        }

        if (cardButton == null)
        {
            cardButton = GetComponent<Button>();
        }

        defaultScale = transform.localScale;

        cardButton.onClick.AddListener(OnCardClicked);
    }

    public void Setup(GameManager manager, int id, Sprite front, Sprite back)
    {
        StopAllCoroutines();

        gameManager = manager;
        cardId = id;
        frontSprite = front;
        backSprite = back;

        isRevealed = false;
        isMatched = false;
        isAnimating = false;

        transform.localScale = defaultScale;

        cardImage.sprite = backSprite;
        cardButton.interactable = true;
    }

    public void Reveal()
    {
        if (isRevealed || isMatched || isAnimating)
        {
            return;
        }

        isRevealed = true;

        StartCoroutine(FlipCard(frontSprite));
    }

    public void Hide()
    {
        if (isMatched || isAnimating)
        {
            return;
        }

        isRevealed = false;

        StartCoroutine(FlipCard(backSprite));
    }

    public void SetMatched()
    {
        isMatched = true;
        cardButton.interactable = false;
    }

    public int GetCardId()
    {
        return cardId;
    }

    public bool IsRevealed()
    {
        return isRevealed;
    }

    private IEnumerator FlipCard(Sprite targetSprite)
    {
        isAnimating = true;
        cardButton.interactable = false;

        float halfDuration = flipDuration / 2f;
        float elapsedTime = 0f;

        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / halfDuration);

            transform.localScale = new Vector3(
                Mathf.Lerp(defaultScale.x, 0f, progress),
                defaultScale.y,
                defaultScale.z
            );

            yield return null;
        }

        cardImage.sprite = targetSprite;

        elapsedTime = 0f;

        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / halfDuration);

            transform.localScale = new Vector3(
                Mathf.Lerp(0f, defaultScale.x, progress),
                defaultScale.y,
                defaultScale.z
            );

            yield return null;
        }

        transform.localScale = defaultScale;

        isAnimating = false;

        if (!isMatched)
        {
            cardButton.interactable = true;
        }
    }

    private void OnCardClicked()
    {
        if (gameManager == null)
        {
            Debug.LogError($"{gameObject.name} 尚未設定 GameManager。");
            return;
        }

        gameManager.SelectCard(this);
    }
}