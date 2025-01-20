using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private float defaultSpeed = 5f;
    [SerializeField] private float burstMultiplier = 1f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float burstCooldown = 2f;
    [SerializeField] private Rigidbody2D playerRigidbody = default;

    private float rotationInput;
    private float burstTimer = 0.0f;

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * defaultSpeed * Time.deltaTime);

        transform.Rotate(Vector3.forward, rotationInput * rotateSpeed * Time.deltaTime);
    }

    public void OnRotate(InputValue value)
    {
        rotationInput = value.Get<float>();
    }
    public void OnBurst()
    { // WE might be able to fake force by just changing the sppeed
        if (Time.time >= burstTimer)
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
}
