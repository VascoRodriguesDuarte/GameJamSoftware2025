using System;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    [SerializeField]
    RectTransform visual;

    [SerializeField, Range(0,1)]
    float gaugeValue;

    float originalWidth;
    /*
        originalWidth - 1
        ?             - gaugeValue
    */

    public bool IsEmpty() {
        return gaugeValue <= 0f;
    }

    public bool IsOverHalf() {
        return gaugeValue > 0.5f;
    }

    public bool IsOver(float amount) {
        return gaugeValue > amount;
    }

    public void IncreaseGauge(float amount) {
        if (gaugeValue < 1) {
            float newgauge = gaugeValue + amount;
            gaugeValue = Math.Clamp(newgauge, 0f, 1f);
            setVisuals();
        }
    }

    public void DecreaseGauge(float amount) {
        if (gaugeValue > 0) {
            float newgauge = gaugeValue - amount;
            gaugeValue = Math.Clamp(newgauge, 0f, 1f);
            setVisuals();
        }
    }

    private void setVisuals() {
        setVisualWidth();
    }

    private void setVisualWidth() {
        visual.sizeDelta = new Vector2(
            originalWidth*gaugeValue,
            visual.rect.height
            );
    }

    void Awake() {
        originalWidth = visual.rect.width;
        setVisualWidth();
    }

}
