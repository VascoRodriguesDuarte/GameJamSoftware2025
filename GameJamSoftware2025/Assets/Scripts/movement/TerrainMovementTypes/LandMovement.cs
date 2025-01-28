using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class LandMovement : TerrainMovement
{
    [SerializeField] protected float burstMultiplier = 1f;
    [SerializeField] protected float burstDamageDuration = 0.5f;
    [SerializeField] protected float rotateSpeed = 50f;
    
    [SerializeField] protected float burstCooldown = 2f;
    [SerializeField] protected float stunDuration = 1.5f;
    [SerializeField] protected float boostSpeedModifier = 10f;
    [SerializeField] protected float brakeSpeedModifier = -3f;
    [SerializeField] protected float speedBoostLost = 0.003f;
    [SerializeField] protected float minEnemySpeed = 1f;
    
    private float currentAdditionalSpeed = 0f;

    private float burstTimer = 0.0f;

    private float damageTimer = 0.0f;

    private float stunTimer = 0.0f;

    private bool stun = false;

    private bool burstDamage = false;

    private Vector3 pos;

    private float velocity;

private void Awake()
{
    pos = transform.position;
}
    

    public override void ToUpdate()
    {

        velocity = (transform.position - pos).magnitude / Time.deltaTime;
        pos = transform.position;

        if(!stun)
        {
            transform.Translate(Vector2.up * (defaultSpeed + currentAdditionalSpeed)* extraSpeed * Time.deltaTime);

            transform.Rotate(Vector3.forward, rotationInput * rotateSpeed * Time.deltaTime);
        }
        else
        {
            if(Time.time >= stunTimer)
            {
                stun = false;
            }
        }

        if(Time.time >= damageTimer && burstDamage)
            {
                burstDamage = false;
            }

        BoostUpdate();
        ManageExtraSpeed();
    }

    public override void Enter(Dictionary<String, Vector2> additional)
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
            damageTimer = burstDamageDuration + Time.time;
            burstDamage = true;
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
        if (value == 1 && boostGauge.IsOverCooldown() && !braking) {
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
    public override void ManageExtraSpeed()
    {
        if (extraSpeed > 1f) {
            extraSpeed -= speedBoostLost;
            extraSpeed = Mathf.Max(extraSpeed, 1f);
        }
        else if(extraSpeed < 1f)
        {
            extraSpeed += speedBoostLost;
            extraSpeed = Mathf.Min(extraSpeed, 1f);
        }

        //Debug.Log(extraSpeed);
    }

    public override void StunSpeed()
    {
        if(velocity >= minEnemySpeed)
        {
            return;
        }
        stun = true;
        stunTimer = stunDuration + Time.time;
    }

    public override void StunBurst()
    {
        if(burstDamage)
        {
            return;
        }
        stun = true;
        stunTimer = stunDuration + Time.time;
    }
}
