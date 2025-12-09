using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderBounce : MonoBehaviour
{
    public Slider slider;
    public float bounceAmount = 1f;  // How much to overshoot or undershoot
    public float bounceDuration = 0.3f;

    private Coroutine bounceCoroutine = null;
    private float lastValue;

    void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        lastValue = slider.value;
    }

    void OnEnable()
    {
        if (slider != null)
            slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnDisable()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);

        if (bounceCoroutine != null)
            StopCoroutine(bounceCoroutine);
    }

    private void OnSliderValueChanged(float newValue)
    {
        // Ignore if value hasn't changed enough (optional)
        if (Mathf.Approximately(newValue, lastValue))
            return;

        // Determine direction of change: positive if increased, negative if decreased
        float direction = (newValue > lastValue) ? 1f : -1f;
        lastValue = newValue;

        // Restart bounce coroutine
        if (bounceCoroutine != null)
            StopCoroutine(bounceCoroutine);

        bounceCoroutine = StartCoroutine(BounceValue(newValue, direction));
    }

    private IEnumerator BounceValue(float targetVal, float direction)
    {
        float startVal = slider.value;

        // Calculate overshoot or undershoot based on direction
        float overshoot = targetVal + bounceAmount * direction;

        // Clamp overshoot/undershoot to slider's range
        overshoot = Mathf.Clamp(overshoot, slider.minValue, slider.maxValue);

        float halfDuration = bounceDuration / 2f;
        float elapsed = 0f;

        // Animate from start to overshoot/undershoot
        while (elapsed < halfDuration)
        {
            slider.SetValueWithoutNotify(Mathf.Lerp(startVal, overshoot, elapsed / halfDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        slider.SetValueWithoutNotify(overshoot);

        // Animate back to target value
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            slider.SetValueWithoutNotify(Mathf.Lerp(overshoot, targetVal, elapsed / halfDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        slider.SetValueWithoutNotify(targetVal);

        bounceCoroutine = null;
    }
}
