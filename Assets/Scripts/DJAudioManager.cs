using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleDJAudioManager : MonoBehaviour
{
    [Header("Hype!!")]
    public HypeSystem HypeSystem;

    [Header("Drum Loops")]
    public AudioSource kick;
    public AudioSource hiHat;
    public AudioSource claps;
    public AudioSource BGLoop1;
    public AudioSource BGLoop2;

    [Header("Bass Loops")]
    public AudioSource bass1;
    public AudioSource bass2;

    [Header("Synth Loops")]
    public AudioSource synth1;
    public AudioSource synth2;

    [Header("Vocal Loop")]
    public AudioSource vocal;

    public InputAction toggleKick;
    public InputAction toggleHiHat;
    public InputAction toggleClaps;
    public InputAction toggleBG1;
    public InputAction toggleBG2;
    public InputAction toggleBass1;
    public InputAction toggleBass2;
    public InputAction toggleSynth1;
    public InputAction toggleSynth2;
    public InputAction toggleVocal;


    private void Awake()
    {
        toggleKick.performed += ctx => ToggleMute(kick);
        toggleHiHat.performed += ctx => ToggleMute(hiHat);
        toggleClaps.performed += ctx => ToggleMute(claps);
        toggleBG1.performed += ctx => ToggleMute(BGLoop1);
        toggleBG2.performed += ctx => ToggleMute(BGLoop2);

        toggleBass1.performed += ctx => ToggleMute(bass1);
        toggleBass2.performed += ctx => ToggleMute(bass2);

        toggleSynth1.performed += ctx => ToggleMute(synth1);
        toggleSynth2.performed += ctx => ToggleMute(synth2);

        toggleVocal.performed += ctx => ToggleMute(vocal);
    }

    private void OnEnable()
    {
        toggleKick.Enable();
        toggleHiHat.Enable();
        toggleClaps.Enable();

        toggleBG1.Enable();
        toggleBG2.Enable();
        toggleBass1.Enable();
        toggleBass2.Enable();

        toggleSynth1.Enable();
        toggleSynth2.Enable();

        toggleVocal.Enable();
    }

    private void OnDisable()
    {
        toggleKick.Disable();
        toggleHiHat.Disable();
        toggleClaps.Disable();

        toggleBG1.Disable();
        toggleBG2.Disable();
        toggleBass1.Disable();
        toggleBass2.Disable();

        toggleSynth1.Disable();
        toggleSynth2.Disable();

        toggleVocal.Disable();
    }

    private void Start()
    {
        
        kick.Play();
        hiHat.Play();
        claps.Play();
        BGLoop1.Play();
        BGLoop2.Play();

        bass1.Play();
        bass2.Play();

        synth1.Play();
        synth2.Play();

        vocal.Play();
        
    }

    private void ToggleMute(AudioSource source)
    {
        if (source == null) return;
        HypeSystem.OnLoopChanged();

        source.mute = !source.mute;
        Debug.Log($"{source.name} mute toggled to {source.mute}");
    }
}
