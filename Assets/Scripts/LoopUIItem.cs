using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoopUIItem : MonoBehaviour
{
    public TextMeshProUGUI keybindLabel;
    public TextMeshProUGUI keybindFXLabel;
    public TextMeshProUGUI loopName;

    public AudioSource loopAudioSource;   // reference to loop's AudioSource
    public KeyCode activationKey;         // input key assigned to this loop

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        keybindLabel.text = activationKey.ToString().ToUpper();
        keybindFXLabel.text = activationKey.ToString().ToUpper();
        loopName.text = loopAudioSource.name;
        animator.SetBool("Playing", !loopAudioSource.mute);
    }


    void Update()
    {
        if (loopAudioSource == null) return;

        // Update icon based on playing state
        //playPauseIcon.sprite = loopAudioSource.isPlaying ? pauseSprite : playSprite;

        // Reactive feedback on key press
        if (Input.GetKeyDown(activationKey))
        {
            animator.SetBool("Playing", !loopAudioSource.mute);
        }
    }
}
