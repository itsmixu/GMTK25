using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class HypeSystem : MonoBehaviour
{
    [Header("UI")]
    public Slider hypeMeter;
    public TextMeshProUGUI hypeText;

    [Header("References")]
    public AudioSource[] loops; // assign in Inspector all your loop audio sources
    private CrowdAI[] crowdMembers; // Assign all your NPC scripts in Inspector



    [Header("Values")]
    public int hype = 0;
    public int hypeMax = 10;
    public float score = 0;
    public float multiplier = 1f;

    public float idleThreshold = 6f;  // Seconds before "boredom" sets in
    public float spamWindow = 2f;     // Time window to detect spam
    public int spamLimit = 5;         // Too many actions in this window = spam

    private float lastChangeTime;
    private Queue<float> recentActionTimestamps = new Queue<float>();

    void Start()
    {
        lastChangeTime = Time.time;

        // Find all NPCs tagged "NPC" and get their CrowdAI components
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("NPC");
        Debug.Log($"Found {npcObjects.Length} NPCs.");

        List<CrowdAI> crowdList = new List<CrowdAI>();
        foreach (var obj in npcObjects)
        {
            CrowdAI crowdAI = obj.GetComponent<CrowdAI>();
            if (crowdAI != null)
            {
                crowdList.Add(crowdAI);
            }
            else
            {
                Debug.LogWarning($"NPC '{obj.name}' does not have a CrowdAI component!");
            }
        }
        crowdMembers = crowdList.ToArray();
        Debug.Log($"Crowd members count: {crowdMembers.Length}");
    }


        void Update()
        {
        // Check if any loop is playing
        bool anyPlaying = false;
        foreach (var loop in loops)
        {
            if (loop != null && loop.isPlaying && !loop.mute)
            {
                anyPlaying = true;
                break;
            }
        }

        if (!anyPlaying)
        {
            // No music playing → hype is zero
            if (hype != 0)
                ChangeHype(-hype); // reset to zero
        }
        else
        {
            // Music is playing → ensure hype is at least 1
            if (hype < 1)
                ChangeHype(1 - hype); // raise hype to 1 if below

            // Score multiplier based on hype
            //multiplier = hype;
            score += Time.deltaTime * hype;
            if (hypeText != null) hypeText.text = Mathf.Round(score).ToString();

            // Boredom detection only if music playing
            if (Time.time - lastChangeTime > idleThreshold)
            {
                ChangeHype(-1); // Lose hype for not varying
                lastChangeTime = Time.time;
            }
        }
    }



    // Call this method every time a track is toggled
    public void OnLoopChanged()
    {
        float now = Time.time;
        // Pace detection
        float sinceLast = now - lastChangeTime;

        // Reasonable interval = 1-4 seconds as an example
        if (sinceLast > 0.8f && sinceLast < 4f)
        {
            ChangeHype(+1); // Reward good timing!
        }

        lastChangeTime = now;

        // Spam logic
        recentActionTimestamps.Enqueue(now);
        while (recentActionTimestamps.Count > 0 && (now - recentActionTimestamps.Peek()) > spamWindow)
        {
            recentActionTimestamps.Dequeue();
        }
        if (recentActionTimestamps.Count > spamLimit)
        {
            ChangeHype(-1); // Penalize spam
        }
    }

    public void ChangeHype(int delta)
    {
        bool musicPlaying = false;
        foreach (var loop in loops)
        {
            if (loop != null && loop.isPlaying && !loop.mute)
            {
                musicPlaying = true;
                break;
            }
        }

        hype = Mathf.Clamp(hype + delta, musicPlaying ? 1 : 0, hypeMax);
        Debug.Log("Hype: " + hype);

        if (hypeMeter != null) hypeMeter.value = hype;
        UpdateCrowdDancing();
    }



    void UpdateCrowdDancing()
    {
        if (crowdMembers == null || crowdMembers.Length == 0) return;

        int numToDance = Mathf.RoundToInt(hype / (float)hypeMax * crowdMembers.Length);
        Debug.Log("Numtodance: " + numToDance);

        for (int i = 0; i < crowdMembers.Length; i++)
        {
            if (crowdMembers[i] != null)
                crowdMembers[i].SetDancing(i < numToDance);
        }
    }
}
