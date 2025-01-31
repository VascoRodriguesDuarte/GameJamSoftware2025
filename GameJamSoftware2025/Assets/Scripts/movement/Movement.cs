using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Movement : MonoBehaviour
{
    [SerializeField] private List<TerrainMovement> terrains;

    [SerializeField] private Gauge boostGauge;

    [SerializeField] private TerrainManager TM;

    [SerializeField] private Animator animator;

    [SerializeField] private PauseManager pauseManager;
    private GameTerrain.MinorType currentTerrain;

    public void ChangeTerrainMovement(GameTerrain.MinorType type, Dictionary<String, Vector2> additional) {
        if (type != currentTerrain) {
            Debug.Log("Changing terrain to " + type.ToString() + " from " + currentTerrain.ToString());

            getCurrentMovement().Exit();
            currentTerrain = type;
            getCurrentMovement().Enter(additional);
        }
    }

    public GameTerrain.MinorType GetCurrentTerrain() {
        return currentTerrain;
    }

    public float GetCurentSpeed() {
        return getCurrentMovement().CurrentSpeed(pauseManager.Paused);
    }

    public TerrainMovement getCurrentMovement() {
        return terrains[(int)currentTerrain];
    }

    private void FixedUpdate() {
        TerrainMovement movement = getCurrentMovement();
        animator.speed = Mathf.Clamp(1+movement.CurrentSpeed(pauseManager.Paused)-movement.defaultSpeed,0.1f, 4f);
        movement.ToUpdate();
        boostGauge.GaugeUpdate();
    }

    public void OnRotate(InputValue value) {
        //Debug.Log(terrains[(int)currentTerrain].type);
        getCurrentMovement().Rotate(value.Get<float>());
    }

    public void OnBurst(){
        getCurrentMovement().Burst();
    }

    public void OnBoost(InputValue value) {
        getCurrentMovement().Boost(value.Get<float>());
    }


    public void OnBrake(InputValue value) {
        getCurrentMovement().Brake(value.Get<float>());
    }

    public void Awake() {
        TerrainMovement.boostGauge = boostGauge;
        currentTerrain = TM.CurrentTerrain;
    }
    public void SpeedMultiplier(float value)
    {
        TerrainMovement.extraSpeed += value;
    }

    public void CheckSpeed()
    {
        getCurrentMovement().StunSpeed();
    }

    public void CheckBurst()
    {
        getCurrentMovement().StunBurst();
    }

    public void ApplyExplosionSlow(float value)
    {
        TerrainMovement.extraSpeed -= value;
        if(TerrainMovement.extraSpeed < 0f)
        {
            TerrainMovement.extraSpeed = 0f;
        }
    }

    public Transform getTransform() {
        return transform;
    }
}

