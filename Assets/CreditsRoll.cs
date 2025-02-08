using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections; // <- Necesario para usar IEnumerator

public class CreditsRoll : MonoBehaviour
{
    public RectTransform creditsPanel;
    public float scrollSpeed = 50f;
    public float delayBeforeStart = 2f;
    public float delayAfterEnd = 3f;

    private Vector2 startPosition;
    private Vector2 endPosition;
    private float panelHeight;

    void Start()
    {
        panelHeight = creditsPanel.rect.height;
        startPosition = creditsPanel.anchoredPosition;
        endPosition = new Vector2(startPosition.x, startPosition.y + panelHeight);
        creditsPanel.anchoredPosition = startPosition;
        StartCoroutine(ScrollCredits());
    }

    IEnumerator ScrollCredits() // <- Esto necesita System.Collections
    {
        yield return new WaitForSeconds(delayBeforeStart);

        float elapsedTime = 0f;
        while (elapsedTime < panelHeight / scrollSpeed)
        {
            creditsPanel.anchoredPosition = Vector2.Lerp(startPosition, endPosition, elapsedTime / (panelHeight / scrollSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        creditsPanel.anchoredPosition = endPosition;
        yield return new WaitForSeconds(delayAfterEnd);

        Debug.Log("Créditos terminados");
    }
}