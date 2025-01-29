using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SolidMovement : TerrainMovement
{
    [SerializeField] private float reflectMultiplier = 5;
    public override void Boost(float value)
    {
        throw new System.NotImplementedException();
    }

    public override void Brake(float value)
    {
        throw new System.NotImplementedException();
    }

    public override void Burst()
    {
        throw new System.NotImplementedException();
    }

    public override void Enter(Dictionary<String, Vector2> additional)
    {
        Debug.Log("Entered Solid");
        Vector2 up = -transform.right;
        Debug.Log(up.ToString());
        Vector2 normal = additional["normal"];
        Debug.Log(normal.ToString());
        Vector2 force = Vector2.Reflect(up, normal);
        //Vector2 force = 2 * (up * normal) * normal - up;
        Debug.Log(force.ToString());
        //player.AddForce(force*reflectMultiplier, ForceMode2D.Impulse);
        var angle = Vector2.Angle(-transform.right, normal);
        Debug.Log(angle.ToString());
        transform.Rotate(Vector3.forward, -angle*2);
    }

    public override void Exit()
    {

    }

    public override void ManageExtraSpeed()
    {
        throw new System.NotImplementedException();
    }

    public override void Rotate(float value)
    {
        throw new System.NotImplementedException();
    }

    public override void StunBurst()
    {
        throw new System.NotImplementedException();
    }

    public override void StunSpeed()
    {
        throw new System.NotImplementedException();
    }

    public override void ToUpdate()
    {

    }
}
