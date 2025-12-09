using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;        // Assign if using TMP (optional)
    public Image fadeImage;
    public TextMeshProUGUI scoreText;
    public HypeSystem hypeSystem;
    public GameObject endScreenPanel;       // Assign your end screen panel
    public float startTimeSeconds = 120f;   // 2 minutes = 120 seconds

    private float remainingTime;
    private bool running = false;

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        remainingTime = startTimeSeconds;
        endScreenPanel.SetActive(false);
        running = true;
        UpdateTimerUI();
        StartCoroutine(StartFade());
    }

    void Update()
    {
        if (!running) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            running = false;
            UpdateTimerUI();
            StartCoroutine(TriggerEndScreen());
        }
        else
        {
            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        string timeStr = minutes.ToString("0") + ":" + seconds.ToString("00");

        if (timerText != null)
            timerText.text = timeStr;
    }

    IEnumerator TriggerEndScreen()
    {
        // --- Fade Out to Black ---
        float fadeToBlackDuration = 2f; 
        Color currentColor = fadeImage.color;
        Color blackColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
        float blackoutTime = 0f;

        while (blackoutTime < fadeToBlackDuration)
        {
            float t = blackoutTime / fadeToBlackDuration;
            fadeImage.color = Color.Lerp(currentColor, blackColor, t);
            blackoutTime += Time.deltaTime;
            yield return null;
        }

        int score = Mathf.RoundToInt(hypeSystem.score);
        scoreText.text = $"Congrats! You saved the gig and scored {score} points!";
        endScreenPanel.SetActive(true);
        //Time.timeScale = 0f;
        // (Play sounds, show stats, etc... here)
        // Optionally: Stop gameplay inputs, show score, etc.
    }

    IEnumerator StartFade()
    {
        float fadeToBlackDuration = 1.5f;
        Color currentColor = fadeImage.color;
        Color blackColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
        float blackoutTime = 0f;

        while (blackoutTime < fadeToBlackDuration)
        {
            float t = blackoutTime / fadeToBlackDuration;
            fadeImage.color = Color.Lerp(blackColor, currentColor, t);
            blackoutTime += Time.deltaTime;
            yield return null;
        }
    }
}
