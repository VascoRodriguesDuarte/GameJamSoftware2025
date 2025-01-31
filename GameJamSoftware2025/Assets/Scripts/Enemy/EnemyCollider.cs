using UnityEngine;

public class EnemyCollider : MonoBehaviour
{

    [SerializeField] ResourceManager player;
    [SerializeField] private enemyType enemy;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(enemy == enemyType.Boost)
            {
                Debug.Log("Contact Boost Enemy");
                player.BoostEnemy();
            }
            else if(enemy == enemyType.Burst)
            {
                Debug.Log("Contact Burst Enemy");
                player.BurstEnemy();
                
            }
            else if (enemy == enemyType.Explode)
            {
                Debug.Log("Contact Explode Enemy");
                player.ExplodeEnemy();
            }
            
            AudioSource.PlayClipAtPoint(gameObject.GetComponent<AudioSource>().clip, gameObject.transform.position);
            LeanTween.scale(gameObject,new Vector3(0,0,0), 0.2f);
            Destroy(gameObject,0.2f);
        }
    
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        //other.GetComponent<Movement>().ChangeTerrain(Terrain.Type.Land);

    }
    
    
}
public enum enemyType{
Boost,
Burst,
Explode
};