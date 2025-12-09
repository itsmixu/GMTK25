using System.Collections;
using UnityEngine;

public class SmokeMachine : MonoBehaviour
{
    [SerializeField] private HypeSystem hypeSystem;
    [SerializeField] private KeyCode keyBind;
    private AudioSource audioSource;
    private ParticleSystem particles;

    private bool smoking = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator Smoke()
    {
        if (!smoking)
        {
            smoking = true;
            audioSource.Play();
            yield return new WaitForSeconds(0.3f);
            particles.Play();
            hypeSystem.ChangeHype(2);
            yield return new WaitForSeconds(1.7f);
            audioSource.Stop();
            smoking = false;
        }
    }
}
