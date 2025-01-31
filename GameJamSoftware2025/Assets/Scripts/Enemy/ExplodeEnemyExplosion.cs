using UnityEngine;

public class ExplodetEnemyExplosion : MonoBehaviour
{
    [SerializeField] private Movement player;
    [SerializeField] private float explosionDelay = 1f;
    [SerializeField] private float explosionCooldown = 3f;
    [SerializeField] private float slowValue = 1f;
    [SerializeField] private float explosionSize = 1f;

    [SerializeField] private CircleCollider2D collider;

    [SerializeField] private Animator animator;

    [SerializeField] private Transform explosion;

    private float explosionTimer = 0f;

    private float explosionCooldownTimer = 0f;

    private bool explosionSet = false;

    private bool explosionCooldownSet = false;

    private void FixedUpdate()
    {
        if(Time.time >= explosionTimer && explosionSet)
            {
                Debug.Log("BOOM");
                applyGraphic();
                if(Vector3.Distance(player.transform.position, transform.position) <= explosionSize)
                {
                    player.ApplyExplosionSlow(slowValue);
                }
                AudioSource.PlayClipAtPoint(gameObject.GetComponent<AudioSource>().clip, gameObject.transform.position);
                explosionSet = false;

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
            animator.ResetTrigger("grow");
            animator.SetTrigger("grow");
        }
    
    }

    private void applyGraphic(){
        animator.ResetTrigger("boom");
        animator.SetTrigger("boom");
    }

    private void Start() {
        collider.radius = explosionSize;
        explosion.localScale = new Vector2(explosionSize,explosionSize);
    }
}
