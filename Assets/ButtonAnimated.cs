using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class ButtonAnimated : EventTrigger
{
    [SerializeField] public ButtonAnimationType type;
    [SerializeField] public UnityEvent onClick;

    [System.Serializable]
    public enum ButtonAnimationType
    { 
        none, with_shadow, without_shadow
    }

    private Shadow shadow;
    private RectTransform rect;

    private float rectYPosition;
    private float shadowYPosition;

    private void Start()
	{
        rect = GetComponent<RectTransform>();

        switch (type) {        
            case ButtonAnimationType.with_shadow:
                shadow = GetComponent<Shadow>();

                rectYPosition = rect.anchoredPosition.y;
                shadowYPosition = shadow.effectDistance.y;

                break;
        }
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);

        switch (type)
        {
            case ButtonAnimationType.with_shadow:

                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rectYPosition);
                shadow.effectDistance = new Vector2(shadow.effectDistance.x, shadowYPosition);

                LeanTween.moveY(rect, rect.anchoredPosition.y + shadow.effectDistance.y, 0.1f).setIgnoreTimeScale(true).setEaseInSine();
                LeanTween.value(gameObject, shadow.effectDistance.y, 0, 0.1f).setIgnoreTimeScale(true).setEaseInSine().setOnUpdate((float value) =>
                {
                    shadow.effectDistance = new Vector2(shadow.effectDistance.x, value);
                });

                break;

            case ButtonAnimationType.without_shadow:

                rect.localScale = Vector3.one;

                LeanTween.scale(gameObject, Vector3.one * 0.8f, 0.1f).setIgnoreTimeScale(true).setEaseInSine();

                break;

            default:
                break;
        }

    }

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);

        onClick.Invoke();

        switch (type)
        {
            case ButtonAnimationType.with_shadow:

                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rectYPosition + shadowYPosition);
                shadow.effectDistance = new Vector2(shadow.effectDistance.x, 0);

                LeanTween.moveY(rect, rectYPosition, 0.1f).setIgnoreTimeScale(true).setEaseInSine();
                LeanTween.value(gameObject, 0, shadowYPosition, 0.1f).setOnUpdate((float value) =>
                {
                    shadow.effectDistance = new Vector2(shadow.effectDistance.x, value);
                });

                break;

            case ButtonAnimationType.without_shadow:

                rect.localScale = Vector3.one * 0.8f;

                LeanTween.scale(gameObject, Vector3.one, 0.1f).setIgnoreTimeScale(true).setEaseInSine();

                break;

            default:
                break;
        }

    }
}
