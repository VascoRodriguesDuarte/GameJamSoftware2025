using UnityEngine;

public class AirCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<MovementWithAir>().InAirPublic();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponent<MovementWithAir>().OutAirPublic();
    }

    
}
