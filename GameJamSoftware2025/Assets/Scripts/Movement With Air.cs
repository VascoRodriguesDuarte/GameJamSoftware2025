using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementWithAir : MonoBehaviour
{
    [SerializeField] private float defaultSpeed = 5f;
    [SerializeField] private float burstMultiplier = 1f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float rotateAirValue = 5f;
    [SerializeField] private float burstCooldown = 2f;
    [SerializeField] private float airSpeed = 2f;
    [SerializeField] private Rigidbody2D playerRigidbody = default;

    private float rotationInput;
    private float burstTimer = 0.0f;
    private float airSpeedPrivate = 1f;
    private bool airTime;

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * (defaultSpeed * airSpeedPrivate) * Time.deltaTime);

        float dot = Vector2.Dot(transform.up, Vector2.down);

        if(dot > 0.99f && airTime) 
        {
            rotationInput = 0f;
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }

        transform.Rotate(0f, 0f, rotationInput * rotateSpeed * Time.deltaTime);
    }

    private void InAir()
    {
        airTime = true;
        airSpeedPrivate = airSpeed;

        float dot = Vector2.Dot(transform.up, Vector2.right);

        Debug.Log(dot);

        if(dot > 0f) {rotationInput = -rotateAirValue; Debug.Log("Right looking, go down");}
        else{rotationInput = rotateAirValue; Debug.Log("Left looking, go down");}
    }

    private void OutAir()
    {
        airTime = false;
        airSpeedPrivate = 1f;
        rotationInput = 0f;
    }

    public void OnRotate(InputValue value)
    {
        if(!airTime)
        {
            rotationInput = value.Get<float>();
        }
    }

    public void OnBurst()
    { // WE might be able to fake force by just changing the sppeed
        if (Time.time >= burstTimer && !airTime)
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

    public void InAirPublic()
    {
        InAir();
    }

    public void OutAirPublic()
    {
        OutAir();
    }
}