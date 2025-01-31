using UnityEngine;

public class ResourceCollider : MonoBehaviour
{
    [SerializeField] ResourceManager player;
    [SerializeField] private boostType boost;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(boost == boostType.Chain)
            {
                Debug.Log("Resources!");
                player.ChainUp();
            }
            else if(boost == boostType.Big)
            {
                Debug.Log("BigResource!");
                player.BigBoost();
                
            }
            else if (boost == boostType.Gauge)
            {
                player.GaugeResource();
                player.ChainUp();
                Debug.Log("BackResource!");
            }
            LeanTween.scale(gameObject,new Vector3(0,0,0), 0.2f);
            Destroy(gameObject,0.2f);
        }
    
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        //other.GetComponent<Movement>().ChangeTerrain(Terrain.Type.Land);

    }
    
    
}
public enum boostType{
Chain,
Big,
Gauge
};
