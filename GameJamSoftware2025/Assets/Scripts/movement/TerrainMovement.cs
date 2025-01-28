using System;
using System.Collections.Generic;
using UnityEngine;

abstract public class TerrainMovement: MonoBehaviour
{
    [SerializeField] protected float defaultSpeed = 5f;
    public static float extraSpeed = 1f;

    protected static float currentAdditionalSpeed = 0f;

    static public Rigidbody2D player = default;

    static public Gauge boostGauge;

    public abstract GameTerrain.MinorType type {get;}

    protected static bool scheduleBoost = false;
    protected static bool scheduleBrake = false;


    protected void StopBoosting(float modifier) {
        if (boosting) {
            boostGauge.SetIncreasing();
            currentAdditionalSpeed -= modifier;
            boosting = false;
            scheduleBoost = false;
            Debug.Log("Stopped Boosting");
        }
    }
    protected void StartBoosting(float modifier) {
        if (!boosting) {
            boostGauge.SetDecreasing();
            currentAdditionalSpeed += modifier;
            boosting = true;
            scheduleBoost = false;
            Debug.Log("Boosting");
        }
    }

    protected void StopBraking(float modifier) {
        if (braking) {
            currentAdditionalSpeed -= modifier;
            braking = false;
            scheduleBrake = false;
            Debug.Log("Stopped Braking");
        }
    }
    protected void StartBraking(float modifier) {
        if (!braking) {
            currentAdditionalSpeed += modifier;
            braking = true;
            scheduleBrake = false;
            Debug.Log("Braking");
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
    }
}