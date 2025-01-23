using UnityEngine;

abstract public class TerrainMovement: MonoBehaviour
{
    [SerializeField] protected float defaultSpeed = 5f;

    static public Rigidbody2D player = default;

    static public Gauge boostGauge;

    abstract public void ToUpdate();
    abstract public void Enter();
    abstract public void Exit();
    abstract public void Rotate(float value);
    abstract public void Burst();
    abstract public void Boost(float value); //add wrapper
    abstract public void Brake(float value); //add wrapper

    protected bool boosting = false;
    protected bool braking = false;

    protected float rotationInput;

    void Awake() {
        Enter();
    }
}