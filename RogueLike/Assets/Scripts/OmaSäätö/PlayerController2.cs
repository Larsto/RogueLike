using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public static PlayerController2 instance;
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    public Rigidbody2D theRB;
    //public Transform gunArm;

    private Camera theCam;

    public Animator anim;

    public float meleeRange;
    public int meleeDamage = 50;
    public LayerMask enemyLayer;
    public LayerMask breakableLayer;

    public GameObject bulletToFire;
    public Transform firePoint;

    public float timeBetweenShots;
    public float timeBetweenHits;
    private float shotCounter;

    public SpriteRenderer bodySR;

    private float activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = .5f, dashCooldown = 1f, dashInvinsibility = .5f;
    [HideInInspector]
    public float dashCounter;
    private float dashCoolCounter;

  
    public bool canMove = true;

    private bool facingRight = true;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        theCam = Camera.main;

        activeMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !LevelManager2.instance.isPaused)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();

            theRB.velocity = moveInput * activeMoveSpeed;
            //transform.position += new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime;
            /*
            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = theCam.WorldToScreenPoint(transform.localPosition);
            
          if (mousePos.x < screenPoint.x)
          {
              transform.localScale = new Vector3(-1f, 1f, 1f);
              gunArm.localScale = new Vector3(-1f, -1f, 1f);
          }
          else
          {
              transform.localScale = Vector3.one;
              gunArm.localScale = Vector3.one;
          }

          //rotate gunArm
          Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
          float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
          gunArm.rotation = Quaternion.Euler(0f, 0f, angle);
            */
            if (shotCounter > 0)
            {
                shotCounter -= Time.deltaTime;
            }
                
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (shotCounter <= 0)
                {
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                    shotCounter = timeBetweenShots;
                    AudioManager.instance.PlaySFX(12);
                    anim.SetTrigger("Shoot");
                }
            }
        
    
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (shotCounter <= 0)
                {
                    AudioManager.instance.PlaySFX(12);
                    anim.SetTrigger("Hit1");
                    shotCounter = timeBetweenHits;
                    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(firePoint.position, meleeRange, enemyLayer);
                    foreach (Collider2D enemy in hitEnemies)
                    {
                        enemy.GetComponent<EnemyController2>().DamageEnemy(meleeDamage);
                    }
                    Collider2D[] hitBreakable = Physics2D.OverlapCircleAll(firePoint.position, meleeRange, breakableLayer);
                    foreach (Collider2D breakable in hitBreakable)
                    {
                        breakable.GetComponent<Breakables2>().Smash();
                    }
                }
            }
          
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;
                    anim.SetTrigger("Dash");
                    PlayerHealthController2.instance.MakeInvincible(dashInvinsibility);
                    AudioManager.instance.PlaySFX(8);
                }
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;
                }
            }

            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }

            if (moveInput != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
            if (facingRight == false && moveInput.x > 0)
            {
                Flip();
            }
            else if (facingRight == true && moveInput.x < 0)
            {
                Flip();
            }
        }
        else
        {
            theRB.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        if (facingRight)
        {
            firePoint.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            firePoint.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        
    }
}
