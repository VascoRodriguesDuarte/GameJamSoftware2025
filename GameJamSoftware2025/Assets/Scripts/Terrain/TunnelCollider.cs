using UnityEngine;

public class TunnelCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered tunnel collider");
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exited tunnel collider");
    }
}
