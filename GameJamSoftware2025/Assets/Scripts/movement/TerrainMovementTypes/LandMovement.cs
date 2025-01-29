using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class LandMovement : TerrainMovement
{
    [SerializeField] protected float burstMultiplier = 15f;
    [SerializeField] protected float burstDamageDuration = 0.5f;
    [SerializeField] protected float rotateSpeed = 50f;
    [SerializeField] protected float burstCooldown = 2f;
    [SerializeField] protected float stunDuration = 1.5f;
    [SerializeField] protected float boostSpeedModifier = 10f;
    [SerializeField, Range(0,1)] protected float brakeSpeedModifier = 0.6f;
    [SerializeField] protected float minEnemySpeed = 1f;
    
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
            transform.Translate(Vector2.up * GetCurrentSpeed() * Time.deltaTime);

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
        if (scheduleBoost) {
            StartBoosting(boostSpeedModifier);
        }
        if (scheduleBrake) {
            StartBraking(brakeSpeedModifier);
        }
    }


    public override void Exit()
    {
        if (boosting) {
            StopBoosting(boostSpeedModifier);
            scheduleBoost = true;
        }
        if (braking) {
            StopBraking(brakeSpeedModifier);
            scheduleBrake = true;   
        }
    }

    public override void Rotate(float value)
    {
        rotationInput = value;
    }

    public override void Burst()
    {
        if (Time.time >= burstTimer && !boosting && !braking)
        {
            //Vector2 currentUp = transform.up;

            //currentUp.x += currentUp.x > 0 ? 1 : -1;
            //currentUp.y += currentUp.y > 0 ? 1 : -1;

            //player.AddForce(currentUp*burstMultiplier, ForceMode2D.Impulse);
            force += burstMultiplier;
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


    public override void Boost(float value)
    {
        if (value == 1 && boostGauge.IsOverCooldown() && !braking) {
            StartBoosting(boostSpeedModifier);
        } else {
            StopBoosting(boostSpeedModifier);
        }
    }

    public void BoostUpdate() {
        if (boosting) {
            if (boostGauge.IsEmpty()) {
                StopBoosting(boostSpeedModifier);         
            };
        }
    }

    public override void Brake(float value)
    {
        if (value == 1 && !boosting) {
            StartBraking(brakeSpeedModifier);
        } else {
            StopBraking(brakeSpeedModifier);
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
