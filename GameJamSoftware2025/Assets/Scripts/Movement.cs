using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private float defaultSpeed = 5f;
    [SerializeField] private float burstMultiplier = 1f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float burstCooldown = 2f;
    [SerializeField] private Rigidbody2D playerRigidbody = default;

    [SerializeField] private Gauge boostGauge;
    [SerializeField] private float boostSpeed = 10f;

    [SerializeField, Range(0,1)] private float boostGaugeCost = 0.2f;

    [SerializeField, Range(0,1)] private float boostGaugeGain = 0.10f;

    [SerializeField, Range(0,1)] private float boostCooldownPercent = 0.10f;

    [SerializeField] private float brakeSpeed = 3f;

    private float currentBoostSpeed = 0f;

    private float currentBrakeSpeed = 0f;

    private bool boosting = false;
    private bool braking = false;

    private float rotationInput;
    private float burstTimer = 0.0f;

    private void FixedUpdate()
    {

        transform.Translate(Vector2.up * (defaultSpeed + currentBoostSpeed - currentBrakeSpeed) * Time.deltaTime);

        transform.Rotate(Vector3.forward, rotationInput * rotateSpeed * Time.deltaTime);

        BoostUpdate();

    }

    public void OnRotate(InputValue value)
    {
        rotationInput = value.Get<float>();
        Debug.Log("Rotate");

    }
    public void OnBurst()
    { // WE might be able to fake force by just changing the sppeed
        if (Time.time >= burstTimer && !boosting && !braking)
        {
            Vector2 currentUp = transform.up;

            currentUp.x += currentUp.x > 0 ? 1 : -1;
            currentUp.y += currentUp.y > 0 ? 1 : -1;

            playerRigidbody.AddForce(currentUp*burstMultiplier, ForceMode2D.Impulse);
            burstTimer = burstCooldown + Time.time;
            Debug.Log("BURSTED");
        }
        else
        {
            Debug.Log("FAILED burst");
        }
    }

    public void BoostUpdate() {
        if (boosting) {
            boostGauge.DecreaseGauge(boostGaugeCost * Time.deltaTime);
            if (boostGauge.IsEmpty()) {
                currentBoostSpeed = 0f;
                boosting = false;
                Debug.Log("Stopped Boosting");            
            };
        } else {
            boostGauge.IncreaseGauge(boostGaugeGain * Time.deltaTime);
        }
    }

    public void OnBoost(InputValue value) {
        float boost = value.Get<float>();
        if (boost == 1 && boostGauge.IsOver(boostCooldownPercent) && !braking) {
            currentBoostSpeed = boostSpeed;
            boosting = true;
            Debug.Log("Boosting");
        } else {
            currentBoostSpeed = 0f;
            boosting = false;
            Debug.Log("Stopped Boosting");
        }
        
    }

    public void OnBrake(InputValue value) {
        float brake = value.Get<float>();
        if (brake == 1 && !boosting) {
            currentBrakeSpeed = brakeSpeed;
            braking = true;
            Debug.Log("Braking");
        } else {
            currentBrakeSpeed = 0f;
            braking = false;
            Debug.Log("Stopped Braking");
        }
        
    }
}
