using UnityEngine;

public class explosiveEnemyScript : MonoBehaviour, IDamage
{
    public int damage;

    //explosion damage radius
    public float hitRadius;
    
    //the distance needed for the enemy to come flying towards the player
    public float attackRadius;

    //Dashing system
    private bool isDashing;
    private Vector3 dashDirection;
    public float dashForce;

    public float health;
    public float Health { get; set; }

    private void Start()
    {
        Health = health;
        isDashing = false;
    }

    private void Update()
    {
        if (Health <= 0)
        {
            Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, hitRadius);
            explode(coll);
        }

        if(isDashing)
        {
            transform.position += dashDirection * Time.deltaTime * dashForce;
        }

        if (Vector3.Distance(transform.parent.position, GameObject.Find("player").transform.position) <= attackRadius && !isDashing)
        {
            isDashing = true;
            dashDirection = GameObject.Find("player").transform.position - transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, hitRadius);
        explode(coll);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, hitRadius);
        explode(coll);
    }

    private void explode(Collider2D[] coll)
    {
        foreach (Collider2D col in coll)
        {
            IDamage _Interface = null;
            col.TryGetComponent(out _Interface);
            if (_Interface != null)
                _Interface.Health -= damage;
        }


        GameObject graphics = Instantiate(GameObjHodler._i.EnemyExplosionEffectGraphics, transform.position, Quaternion.identity);
        GameObject audio = Instantiate(GameObjHodler._i.EnemyExplosionEffectAudio, transform.position, Quaternion.identity);

        Destroy(graphics, 5);
        Destroy(audio, 5);

        Destroy(transform.parent.gameObject);
    }
}
