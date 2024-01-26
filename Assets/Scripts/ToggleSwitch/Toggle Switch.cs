using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
{
    [Header("Slider setup")]
    [SerializeField, Range(0, 1f)] protected float sliderValue;
    public bool CurrentValue { get; private set; }

    private Slider slider;

    [Header("Animation")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve slideEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine animateSliderCourouitine;

    [Header("Events")]
    [SerializeField] private UnityEvent onToggleOn;
    [SerializeField] private UnityEvent onToggleOff;

    private ToggleSwitchGroupManager toogleSwitchGroupManager;

    protected Action transitionEffect;

    protected virtual void OnValidate()
    {
        SetupToggleComponents();
        slider.value = sliderValue;
    }

    private void SetupToggleComponents()
    {
        if (slider != null) return;
        SetupSliderComponent();
    }

    private void SetupSliderComponent()
    {
        slider = GetComponent<Slider>();
        if (slider == null)
        {
            Debug.LogError("No slider component found on this gameobject");
            return;
        }

        slider.interactable = false;
        var sliderColors = slider.colors;
        sliderColors.disabledColor = Color.white;
        slider.colors = sliderColors;
        slider.transition = Selectable.Transition.None;
    }

    public void SetupForManager(ToggleSwitchGroupManager manager)
    {
        toogleSwitchGroupManager = manager;
    }

    protected virtual void Awake()
    {
        if (sliderValue == 1) { CurrentValue = true; }
        SetupToggleComponents();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toogle();
    }

    private void Toogle()
    {
        if (toogleSwitchGroupManager != null)
        {
            toogleSwitchGroupManager.ToggleGroup(this);
        }
        else
        {
            SetStateAndStartAnimation(!CurrentValue);
        }
    }

    public void ToogleByGroupManager(bool valueToSetTo)
    {
        SetStateAndStartAnimation(valueToSetTo);
    }

    private void SetStateAndStartAnimation(bool state)
    {
        CurrentValue = state;

        if (CurrentValue)
        {
            onToggleOn?.Invoke();
        }
        else
        {
            onToggleOff?.Invoke();
        }

        if (animateSliderCourouitine != null) { StopCoroutine(animateSliderCourouitine); }

        animateSliderCourouitine = StartCoroutine(AnimateSlider());
    }

    private IEnumerator AnimateSlider()
    {
        float time = 0;
        float startValue = slider.value;
        float endValue = CurrentValue ? 1 : 0;

        if (animationDuration > 0)
        {
            while (time < animationDuration)
            {
                time += Time.deltaTime;
                float lerpFactor = slideEase.Evaluate(time / animationDuration);
                slider.value = sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);
                transitionEffect?.Invoke();
                yield return null;
            }
        }
    
        slider.value = endValue;
    }

}