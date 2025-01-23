using UnityEngine;
using UnityEngine.InputSystem;

public class LandMovement : TerrainMovement
{
    [SerializeField] private float burstMultiplier = 1f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float burstCooldown = 2f;
    [SerializeField] private float boostSpeedModifier = 10f;
    [SerializeField] private float brakeSpeedModifier = -3f;

    private float currentAdditionalSpeed = 0f;

    private float burstTimer = 0.0f;
    

    public override void ToUpdate()
    {

        transform.Translate(Vector2.up * (defaultSpeed + currentAdditionalSpeed) * Time.deltaTime);

        transform.Rotate(Vector3.forward, rotationInput * rotateSpeed * Time.deltaTime);

        BoostUpdate();

    }

    public override void Enter()
    {
        if (previousBoosting) {
            StartBoosting();
        } else if (previousBraking) {
            StartBraking();
        }
    }

    private bool previousBoosting = false;
    private bool previousBraking = false;

    public override void Exit()
    {
        previousBoosting = boosting;
        previousBraking = braking;
        StopBoosting();
        StopBraking();
        rotationInput = 0;
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

    private void StopBoosting() {
        if (boosting) {
            boostGauge.SetIncreasing();
            currentAdditionalSpeed -= boostSpeedModifier;
            boosting = false;
            Debug.Log("Stopped Boosting");
        }
    }
    private void StartBoosting() {
        if (!boosting) {
            boostGauge.SetDecreasing();
            currentAdditionalSpeed += boostSpeedModifier;
            boosting = true;
            Debug.Log("Boosting");
        }
    }
    public override void Boost(float value)
    {
        if (value == 1 && boostGauge.IsOver(boostGauge.boostCooldownPercent) && !braking) {
            StartBoosting();
        } else {
            StopBoosting();
        }
    }

    public void BoostUpdate() {
        if (boosting) {
            if (boostGauge.IsEmpty()) {
                StopBoosting();         
            };
        }
    }

    private void StopBraking() {
        if (braking) {
            currentAdditionalSpeed -= brakeSpeedModifier;
            braking = false;
            Debug.Log("Stopped Braking");
        }
    }
    private void StartBraking() {
        if (!braking) {
            currentAdditionalSpeed += brakeSpeedModifier;
            braking = true;
            Debug.Log("Braking");
        }
    }
    public override void Brake(float value)
    {
        if (value == 1 && !boosting) {
            StartBraking();
        } else {
            StopBraking();
        }    
    }
}
