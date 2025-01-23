using UnityEngine;

public class AirCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Movement>().ChangeTerrain(Terrain.Type.Air);
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponent<Movement>().ChangeTerrain(Terrain.Type.Land);

    }

    
}
