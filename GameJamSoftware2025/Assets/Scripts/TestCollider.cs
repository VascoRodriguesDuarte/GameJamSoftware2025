using UnityEngine;

public class TestCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered collider");
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exited collider");
    }

}
