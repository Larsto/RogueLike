using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController2 : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;

    public float rangeToChacePlayer;
    private Vector3 moveDirection;

    public Animator anim;

    public int health = 150;

    public GameObject[] deathSplatters;
    public GameObject hitEffect;

    public bool shouldShoot;

    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;
    public float shootRange;

    public SpriteRenderer theBody;

    private bool facingRight = true;
    private float oldPosition;
    private float newPosition;
    private float hitTime;
    public float setHitTime = 1;
    public float minDistance = 2f;
    public float meleeRange;
    public LayerMask playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        oldPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (theBody.isVisible && PlayerController2.instance.gameObject.activeInHierarchy)
        {
            if (Vector3.Distance(transform.position, PlayerController2.instance.transform.position) < minDistance)
                
            {
                Hit();
                moveDirection = Vector3.zero;
            }
            else if (Vector3.Distance(transform.position, PlayerController2.instance.transform.position) < rangeToChacePlayer)
            {
                moveDirection = PlayerController2.instance.transform.position - transform.position;
            }
            else
            {
                moveDirection = Vector3.zero;
            }

            moveDirection.Normalize();

            theRB.velocity = moveDirection * moveSpeed;

            if (shouldShoot && Vector3.Distance(transform.position, PlayerController2.instance.transform.position) < shootRange)
            {
                fireCounter -= Time.deltaTime;

                if (fireCounter <= 0)
                {
                    fireCounter = fireRate;
                    Instantiate(bullet, firePoint.position, firePoint.rotation);
                    AudioManager.instance.PlaySFX(13);
                }
            }

        }
        else
        {
            theRB.velocity = Vector2.zero;
        }
        if (moveDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
        newPosition = oldPosition;

        if (transform.position.x > oldPosition && facingRight)
        {
            Flip();
        }
            
        if (transform.position.x < oldPosition && !facingRight)
        {
            Flip();
        }
            
        oldPosition = transform.position.x;

    }


    private void Hit()
    {
        hitTime -= Time.deltaTime;
        if (hitTime <= 0)
        {
            AudioManager.instance.PlaySFX(4);
            anim.SetTrigger("Hit");
            hitTime = setHitTime;
            if(Physics2D.OverlapCircle(firePoint.position, meleeRange, playerLayer))
            {
                PlayerHealthController2.instance.DamagePlayer();
            }
            
        } 
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;

        AudioManager.instance.PlaySFX(2);

        anim.SetTrigger("Hurt");
        hitTime = setHitTime;
        Instantiate(hitEffect, transform.position, transform.rotation);
        rangeToChacePlayer = 10;
        if (health <= 0)
        {
            Destroy(gameObject);

            AudioManager.instance.PlaySFX(1);

            int selectedSplatter = Random.Range(0, deathSplatters.Length);
            int rotation = Random.Range(0, 4);

            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation * 90));
        }
    }
    /*
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController2.instance.DamagePlayer();

        }
    }
    */
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
