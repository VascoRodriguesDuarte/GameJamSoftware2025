using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private float defaultSpeed = 5f;

    [SerializeField] private float rotateSpeed = 5f;

    private float rotationInput;

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * defaultSpeed * Time.deltaTime);

        transform.Rotate(Vector3.forward, rotationInput * rotateSpeed * Time.deltaTime);
    }

    public void OnRotate(InputValue value)
    {
        rotationInput = value.Get<float>();
    }
}
