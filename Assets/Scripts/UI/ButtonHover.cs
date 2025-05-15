using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Hover Scaling")]
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1f);
    private Vector3 originalScale;
    private Vector3 targetScale;
    public float scaleSpeed = 10f;

    [Header("Text Shift on Press")]
    public float yOffsetOnPress = -6f;
    private RectTransform textRect;
    private Vector2 originalTextPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        textRect = GetComponentInChildren<TextMeshProUGUI>()?.rectTransform;
        if (textRect != null)
        {
            originalTextPos = textRect.anchoredPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Hover detected on: " + gameObject.name);
        targetScale = hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (textRect != null)
            textRect.anchoredPosition = originalTextPos + new Vector2(0, yOffsetOnPress);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetTextPosition();
    }

    private void ResetTextPosition()
    {
        if (textRect != null)
            textRect.anchoredPosition = originalTextPos;
    }
}
