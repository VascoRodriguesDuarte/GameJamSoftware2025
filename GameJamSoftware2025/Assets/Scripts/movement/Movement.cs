using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Movement : MonoBehaviour
{

    [SerializeField] private Rigidbody2D player = default;

    [SerializeField] private List<TerrainMovement> terrains;

    [SerializeField] private Gauge boostGauge;

    private Terrain.Type currentTerrain = Terrain.Type.Land;

    public void ChangeTerrain(Terrain.Type type) {
        Debug.Log("Changing terrain to " + type.ToString());
        getCurrentMovement().Exit();
        currentTerrain = type;
        getCurrentMovement().Enter();
    }

    public Terrain.Type GetCurrentTerrain() {
        return currentTerrain;
    }

    private TerrainMovement getCurrentMovement() {
        return terrains[(int)currentTerrain];
    }

    private void FixedUpdate() {
        getCurrentMovement().ToUpdate();
        boostGauge.GaugeUpdate();
    }

    public void OnRotate(InputValue value) {
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
        TerrainMovement.player = player;
        TerrainMovement.boostGauge = boostGauge;
    }
    public void SpeedMultiplier(float value)
    {

        getCurrentMovement().extraSpeed += value;
    }
}

