using System;
using UnityEditor;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    [SerializeField]
    RectTransform visual;

    [SerializeField, Range(0,1)]
    float gaugeValue;

    [SerializeField, Range(0,1)] private float boostGaugeCost = 0.2f;

    [SerializeField, Range(0,1)] private float boostGaugeGain = 0.10f;

    [SerializeField, Range(0,1)] private float boostCooldownPercent = 0.10f;

    float originalWidth;
    /*
        originalWidth - 1
        ?             - gaugeValue
    */
    private enum GaugeUpdateState {
        Increasing,
        Decreasing,
        Stagnant
    }
    private GaugeUpdateState updateMode = GaugeUpdateState.Stagnant;

    public void SetIncreasing() {
        updateMode = GaugeUpdateState.Increasing;
    }
    public void SetDecreasing() {
        updateMode = GaugeUpdateState.Decreasing;
    }
    public void SetStagnant() {
        updateMode = GaugeUpdateState.Stagnant;
    }

    public bool IsEmpty() {
        return gaugeValue <= 0f;
    }

    public bool IsOverHalf() {
        return gaugeValue > 0.5f;
    }

    public bool IsOver(float amount) {
        return gaugeValue > amount;
    }

    public bool IsOverCooldown() {
        return IsOver(boostCooldownPercent);
    }

    public bool IsMax() {
        return gaugeValue >= 1;
    }

    public void IncreaseGauge(float amount) {
        if (gaugeValue < 1) {
            float newgauge = gaugeValue + amount;
            gaugeValue = Math.Clamp(newgauge, 0f, 1f);
            setVisuals();
        }
    }

    private void DecreaseGauge(float amount) {
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

    public void GaugeUpdate() {

        switch (updateMode) {
            case GaugeUpdateState.Increasing:
                IncreaseGauge(boostGaugeGain*Time.deltaTime);
                if (IsMax()) {
                    updateMode = GaugeUpdateState.Stagnant;
                }
                break;
            case GaugeUpdateState.Decreasing:
                DecreaseGauge(boostGaugeCost*Time.deltaTime);
                if (IsEmpty()) {
                    updateMode = GaugeUpdateState.Stagnant;
                }
                break;
            case GaugeUpdateState.Stagnant:
                break;
            default:
                Debug.Log("Uh Oh");
                break;
        } 
    }

}
