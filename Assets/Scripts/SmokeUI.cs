using UnityEngine;
using TMPro;

public class SmokeUI : MonoBehaviour
{
    public TextMeshProUGUI keybindLabel;
    public TextMeshProUGUI keybindFXLabel;
    public TextMeshProUGUI loopName;

    public float smokeMachineCooldown = 15f;
    public SmokeMachine[] smokeMachines;
    public KeyCode activationKey;

    private Animator animator;
    private float cooldown = 0f;
    private SmokeStates smokeState;
    private enum SmokeStates
    {
        Ready,
        Cooldown
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        loopName.text = "Smoke";
    }


    void Update()
    {
        switch (smokeState)
        {
            case SmokeStates.Ready:
                keybindLabel.text = activationKey.ToString().ToUpper();
                animator.SetBool("Playing", true);
                break;
            case SmokeStates.Cooldown:
                keybindLabel.text = Mathf.RoundToInt(cooldown).ToString();
                animator.SetBool("Playing", false);
                break;
        }

        cooldown -= Time.deltaTime;
        cooldown = Mathf.Clamp(cooldown, 0, smokeMachineCooldown);

        //Debug.Log("Cooldown: " + cooldown);


        if (cooldown == 0) smokeState = SmokeStates.Ready;
        else smokeState = SmokeStates.Cooldown;

        if (smokeState == SmokeStates.Ready && Input.GetKeyDown(activationKey))
        {
            for (int i = 0; i < smokeMachines.Length; i++)
                StartCoroutine(smokeMachines[i].Smoke());
            cooldown = smokeMachineCooldown;
        }
    }
}
