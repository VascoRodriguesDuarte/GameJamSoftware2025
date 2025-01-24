using UnityEngine;

public class ExplodetEnemyExplosion : MonoBehaviour
{
    [SerializeField] private Movement player;
    [SerializeField] private float explosionDelay = 1f;
    [SerializeField] private float explosionCooldown = 3f;
    [SerializeField] private float slowValue = 1f;
    [SerializeField] private float explosionSize = 1f;

    private float explosionTimer = 0f;

    private float explosionCooldownTimer = 0f;

    private bool explosionSet = false;

    private bool explosionCooldownSet = false;

    private void FixedUpdate()
    {
        if(Time.time >= explosionTimer && explosionSet)
            {
                Debug.Log("BOOM");
                if(Vector3.Distance(player.transform.position, transform.position) <= explosionSize)
                {
                    player.ApplyExplosionSlow(slowValue);
                    explosionSet = false;
                }

            }

        if(Time.time >= explosionCooldownTimer && explosionCooldownSet)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
            explosionCooldownSet = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            explosionTimer = explosionDelay + Time.time;
            explosionCooldownTimer = explosionCooldown + Time.time;
            explosionSet = true;
            explosionCooldownSet = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    
    }
}
