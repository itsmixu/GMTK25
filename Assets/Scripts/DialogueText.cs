using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueText : MonoBehaviour
{
    private TextMeshProUGUI uiText;
    private string fullText;     // The text to display
    public float delay = 0.05f; // Delay between letters (in seconds)

    private Coroutine typingCoroutine;

    void OnEnable()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        fullText = uiText.text;

        typingCoroutine = StartCoroutine(ShowText());
    }

    void OnDisable()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
    }

    IEnumerator ShowText()
    {
        uiText.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            uiText.text += fullText[i];
            yield return new WaitForSeconds(delay);
        }
    }
}
