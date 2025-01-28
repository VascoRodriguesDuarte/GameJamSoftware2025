using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AirMovement : TerrainMovement
{
    [SerializeField] private float burstMultiplier = 1f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float rotateAirValue = 5f;
    [SerializeField] private float burstCooldown = 2f;
    [SerializeField] private float airSpeed = 2f;
    [SerializeField] private float speedBoostLost = 0.003f;
    
    private float rotationValue;
    private float burstTimer = 0.0f;
    private float airSpeedPrivate = 1f;
    private bool airTime;

    public override GameTerrain.MinorType type => GameTerrain.MinorType.Air;

    private void InAir()
    {
        airTime = true;
        airSpeedPrivate = airSpeed;

        float dot = Vector2.Dot(transform.up, Vector2.right);

        //Debug.Log(dot);

        if(dot > 0f) {rotationValue = -rotateAirValue; Debug.Log("Right looking, go down");}
        else{rotationValue = rotateAirValue; Debug.Log("Left looking, go down");}
        ManageExtraSpeed();
    }

    private void OutAir()
    {
        airTime = false;
        airSpeedPrivate = 1f;
        rotationValue = 0f;
    }

    public override void Enter(Dictionary<String, Vector2> additional)
    {
        InAir();
    }

    public override void Exit()
    {
        OutAir();
    }

    public override void ToUpdate()
    {
        transform.Translate(Vector2.up * (defaultSpeed * airSpeedPrivate) * Time.deltaTime);

        float dot = Vector2.Dot(transform.up, Vector2.down);

        if(dot > 0.99f && airTime) 
        {
            rotationValue = 0f;
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }

        transform.Rotate(0f, 0f, rotationValue * rotateSpeed * Time.deltaTime);    }

    public override void Rotate(float value)
    {
        if(!airTime)
        {
            rotationValue = value;
        }
    }

    public override void Burst()
    {
        if (Time.time >= burstTimer && !airTime)
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
        }    }

    public override void Boost(float value)
    {
        throw new NotImplementedException();
    }

    public override void Brake(float value)
    {
        throw new NotImplementedException();
    }
    public override void ManageExtraSpeed()
    {
        if (extraSpeed > 1f) {
            extraSpeed -= speedBoostLost;
            extraSpeed = Mathf.Max(extraSpeed, 1f);
        }

        //Debug.Log(extraSpeed);
    }

    public override void StunSpeed()
    {
        throw new System.NotImplementedException();
    }

    public override void StunBurst()
    {
        throw new System.NotImplementedException();
    }
}