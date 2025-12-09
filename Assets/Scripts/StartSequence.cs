using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSequence : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject DJ;
    private NavMeshAgent playerAgent;
    private NavMeshAgent DJagent;
    [Header("NavPoints")]
    [SerializeField] private Transform DJpoint1;
    [SerializeField] private Transform DJpoint2;
    [SerializeField] private Transform playerPoint;

    [Header("UI")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private TextMeshProUGUI pressSpaceText;
    [SerializeField] private GameObject[] dialogueTexts;
    private int dialogueIndex = 0;

    [Header("Audio")]
    [SerializeField] private AudioMixer mixer;
    public string volumeParam = "Volume";        // Exposed volume parameter
    public string lowpassParam = "Cutoff";  // Exposed lowpass cutoff 
    [SerializeField] private float fadeDuration = 4f;
    [SerializeField] private AudioSource[] audioSources;

    private Animator DJanimator;
    private Animator playerAnimator;

    void Start()
    {
        playerAnimator = playerObject.GetComponent<Animator>();
        playerAgent = playerObject.GetComponent<NavMeshAgent>();
        DJanimator = DJ.GetComponent<Animator>();
        DJagent = DJ.GetComponent<NavMeshAgent>();

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Play();
        }

        StartCoroutine(StartCutscene());
    }

    private IEnumerator StartCutscene()
    {
        float time = 0f;
        float startVol = -80f;       // dB volume for silence
        float endVol = -10f;           // full volume dB
        float startCutoff = 10f;    // muffled lowpass freq
        float endCutoff = 22000f;    // fully opened lowpass freq

        // Initialize image alpha to fully opaque (1) or however you want it
        Color startColor = fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);  // target alpha = 0 (fade out)
        fadeImage.color = startColor;

        // Set initial audio mixer parameters
        mixer.SetFloat(volumeParam, startVol);
        mixer.SetFloat(lowpassParam, startCutoff);

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null; // Wait for next frame
        }

        pressSpaceText.enabled = false;


        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            float vol = Mathf.Lerp(startVol, endVol, Mathf.SmoothStep(0, 1, t));
            float cutoff = Mathf.Lerp(startCutoff, endCutoff, Mathf.SmoothStep(0, 1, t));
            fadeImage.color = Color.Lerp(startColor, targetColor, t);

            mixer.SetFloat(volumeParam, vol);
            mixer.SetFloat(lowpassParam, cutoff);

            time += Time.deltaTime;
            yield return null;
        }

        // Ensure finalized values
        mixer.SetFloat(volumeParam, endVol);
        mixer.SetFloat(lowpassParam, endCutoff);
        fadeImage.color = targetColor;


        yield return new WaitForSeconds(3f);

        DJanimator.SetBool("isWalking", true);
        DJagent.SetDestination(DJpoint1.position);

        yield return new WaitUntil(() => !DJagent.pathPending && DJagent.remainingDistance < 0.5f);
        DJanimator.SetBool("isWalking", false);
        DJagent.isStopped = true;
        Debug.Log("now stopped");

        yield return new WaitForSeconds(1f);

        DJ.transform.LookAt(playerObject.transform.position);
        playerAnimator.SetBool("isIdle", true);

        foreach (var textObj in dialogueTexts) // Dialogue
            textObj.SetActive(false);

        dialogueIndex = 0; // start at 0

        while (dialogueIndex < dialogueTexts.Length)
        {
            // Show the current dialogue
            dialogueTexts[dialogueIndex].SetActive(true);

            //Debug.Log("Now showing dialogue: " + dialogueIndex);
            yield return new WaitForSeconds(1f);

            // Wait until space is pressed
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            // Hide the current dialogue
            dialogueTexts[dialogueIndex].SetActive(false);

            dialogueIndex++;
        }

        DJagent.isStopped = false;
        DJanimator.SetBool("isWalking", true);
        DJagent.SetDestination(DJpoint2.position);

        yield return new WaitForSeconds(1f);

        playerAgent.SetDestination(playerPoint.position);

        playerAgent.SetDestination(playerPoint.position);

        // --- Fade Out to Black ---
        float fadeToBlackDuration = 2f; // Or whatever duration suits your pacing
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

        fadeImage.color = blackColor;
        SceneManager.LoadScene(1);
    }

}
