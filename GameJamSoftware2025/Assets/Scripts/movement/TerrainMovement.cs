using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

abstract public class TerrainMovement: MonoBehaviour
{
    [SerializeField] public float defaultSpeed = 5f;

    [SerializeField] protected float maxSpeed = 30f;

    [SerializeField] protected float LingeringSpeedLoss = 0.04f;

    [SerializeField] protected float speedBoostLost = 0.003f;

    [SerializeField] protected float ForceSpeedLoss = 0.5f;

    public static float extraSpeed = 1f;

    protected static float currentAdditionalSpeed = 0f;

    static public Gauge boostGauge;

    public abstract GameTerrain.MinorType type {get;}

    protected static bool scheduleBoost = false;
    protected static bool scheduleBrake = false;

    protected static float lingeringSpeed;

    protected static float boostSpeed = 0f;

    protected static float brakeModifier = 0f;

    protected static float force;

    protected static float previousSpeed = 0f;

    protected bool stun = false;


    protected void StopBoosting(float modifier) {
        if (boosting) {
            boostGauge.SetIncreasing();
            boostSpeed -= modifier;
            boosting = false;
            scheduleBoost = false;
            Debug.Log("Stopped Boosting");
        }
    }
    protected void StartBoosting(float modifier) {
        if (!boosting) {
            boostGauge.SetDecreasing();
            boostSpeed += modifier;
            boosting = true;
            scheduleBoost = false;
            Debug.Log("Boosting");
        }
    }

    protected void StopBraking(float modifier) {
        if (braking) {
            brakeModifier += modifier;
            braking = false;
            scheduleBrake = false;
            Debug.Log("Stopped Braking");
        }
    }
    protected void StartBraking(float modifier) {
        if (!braking) {
            brakeModifier -= modifier;
            braking = true;
            scheduleBrake = false;
            Debug.Log("Braking");
        }
    }

    protected float ReduceLingeringSpeed(float value, float loss) {
        var linger = value;
        if (linger > 0f) {
            linger -= loss;
            if (linger < 0f) {
                linger=0f;
            }
        } else if (linger < 0f) {
            linger += loss;
            if (linger > 0f) {
                linger=0f;
            }     
        }
        return linger;
    }

    protected float GetCurrentSpeed() {
        lingeringSpeed = ReduceLingeringSpeed(lingeringSpeed, LingeringSpeedLoss);
        force = ReduceLingeringSpeed(force, ForceSpeedLoss);
        if (!boosting) {
            boostSpeed = ReduceLingeringSpeed(boostSpeed, ForceSpeedLoss);
        }
        if (!braking) {
            brakeModifier = ReduceLingeringSpeed(brakeModifier, ForceSpeedLoss);
        }
        previousSpeed = CurrentSpeed();
        return previousSpeed;
    }

    virtual public float CurrentSpeed(bool paused = false) {

        if (!stun && !paused) {
            return Mathf.Clamp(Mathf.Max(defaultSpeed + currentAdditionalSpeed + force + boostSpeed, lingeringSpeed + force + boostSpeed)*(extraSpeed+brakeModifier), 0, maxSpeed);
        } else {
            return 0f;
        }
    }
    abstract public void ToUpdate();
    abstract public void Enter(Dictionary<String, Vector2> additional);
    abstract public void Exit();
    abstract public void Rotate(float value);
    abstract public void Burst();
    abstract public void Boost(float value);
    abstract public void Brake(float value);
    abstract public void ManageExtraSpeed();
    abstract public void StunSpeed();
    abstract public void StunBurst();

    protected static bool boosting = false;
    protected static bool braking = false;

    protected static float rotationInput;
    

    void Awake() {
        Enter(null);
        lingeringSpeed = 0f;
        force = 0f;
        boostSpeed = 0f;
        brakeModifier = 0f;
    }
}