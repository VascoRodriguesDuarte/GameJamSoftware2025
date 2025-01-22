using UnityEngine;
using UnityEngine.InputSystem;

public class LandMovement : TerrainMovement
{
    [SerializeField] private float burstMultiplier = 1f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float burstCooldown = 2f;
    [SerializeField] private Gauge boostGauge;
    [SerializeField] private float boostSpeedModifier = 10f;

    [SerializeField, Range(0,1)] private float boostGaugeCost = 0.2f;

    [SerializeField, Range(0,1)] private float boostGaugeGain = 0.10f;

    [SerializeField, Range(0,1)] private float boostCooldownPercent = 0.10f;

    [SerializeField] private float brakeSpeedModifier = -3f;

    private float currentAdditionalSpeed = 0f;

    private bool boosting = false;
    private bool braking = false;

    private float rotationInput;
    private float burstTimer = 0.0f;
    

    public override void ToUpdate()
    {

        transform.Translate(Vector2.up * (defaultSpeed + currentAdditionalSpeed) * Time.deltaTime);

        transform.Rotate(Vector3.forward, rotationInput * rotateSpeed * Time.deltaTime);

        BoostUpdate();

    }

    public void BoostUpdate() {
        if (boosting) {
            boostGauge.DecreaseGauge(boostGaugeCost * Time.deltaTime);
            if (boostGauge.IsEmpty()) {
                currentAdditionalSpeed = 0f;
                boosting = false;
                Debug.Log("Stopped Boosting");            
            };
        } else {
            boostGauge.IncreaseGauge(boostGaugeGain * Time.deltaTime);
        }
    }


    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Rotate(float value)
    {
        rotationInput = value;
        Debug.Log("Rotate");
    }

    public override void Burst()
    {
        if (Time.time >= burstTimer && !boosting && !braking)
        {
            Vector2 currentUp = transform.up;

            currentUp.x += currentUp.x > 0 ? 1 : -1;
            currentUp.y += currentUp.y > 0 ? 1 : -1;

            player.AddForce(currentUp*burstMultiplier, ForceMode2D.Impulse);
            burstTimer = burstCooldown + Time.time;
            Debug.Log("BURSTED");
        }
        else
        {
            Debug.Log("FAILED burst");
        }
    }

    public override void Boost(float value)
    {
        if (value == 1 && boostGauge.IsOver(boostCooldownPercent) && !braking) {
            currentAdditionalSpeed = boostSpeedModifier;
            boosting = true;
            Debug.Log("Boosting");
        } else {
            currentAdditionalSpeed = 0f;
            boosting = false;
            Debug.Log("Stopped Boosting");
        }
    }

    public override void Brake(float value)
    {
        if (value == 1 && !boosting) {
            currentAdditionalSpeed = brakeSpeedModifier;
            braking = true;
            Debug.Log("Braking");
        } else {
            currentAdditionalSpeed = 0f;
            braking = false;
            Debug.Log("Stopped Braking");
        }    
    }
}
