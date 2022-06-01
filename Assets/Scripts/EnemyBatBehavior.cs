using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatBehavior : MonoBehaviour
{
    [SerializeField]
    float chaseRange;

    [SerializeField]
    float attackRange;

    [SerializeField]
    float speed = 4;

    public int health = 2;
    public GameObject playerObject;
    public PlayerBehaviour player;
    Rigidbody2D rb2d;
    private bool movingRight = true;
    private bool isHit = false;
    private bool noGround = false;
    Animator batAnimator;
    SpriteRenderer batRenderer;
    public Material newMaterial;
    public Material oldMaterial;
    public Transform groundDetection;
    float time = 0f;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        batAnimator = GetComponent<Animator>();
        batRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
        {
            time += Time.deltaTime;
            if (time >= 0.100)
            {
                batRenderer.material = oldMaterial;
                time = 0f;
                isHit = false;
            }
        }
    }
    private void FixedUpdate()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (playerObject.activeInHierarchy)
        {
            if (distToPlayer <= attackRange)
            {
                rb2d.velocity = Vector2.zero;
                batAnimator.SetTrigger("Bite");
            }
            else if (distToPlayer >= chaseRange)
            {
                batAnimator.SetTrigger("Fly");
                Patrol();
            }
            else if (distToPlayer <= chaseRange && !noGround)
            {
                batAnimator.SetTrigger("Fly");
                ChasePlayer();
            }
        }
        else
        {
            batAnimator.SetTrigger("Fly");
            Patrol();
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals("Projectile"))
        {
            batRenderer.material = newMaterial;
            isHit = true;
            AudioManager.PlaySound("Hurt");
            health -= 1;
            if (health == 0)
            {
                batAnimator.SetTrigger("Death");
            }
        }
        
        if (c.gameObject.layer == 6)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else if (movingRight == false)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

    void Patrol()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        CheckGround();
        CheckWalls();
    }

    void ChasePlayer()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        CheckGround();

        if (transform.position.x  > player.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            batRenderer.flipX = true;
            movingRight = false;
        } else
        {
           transform.eulerAngles = new Vector3(0, 0, 0);
            batRenderer.flipX = true;
            movingRight = true;
        }
    }
    void CheckGround()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 2f);
        if (groundInfo.collider == false)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
            noGround = true;
        } else
        {
            noGround = false;
        }
    }

    void CheckWalls()
    {
        RaycastHit2D wallInfo;
        if (movingRight)
        {
            wallInfo = Physics2D.Raycast(groundDetection.position, Vector2.left);
        }
        else
        {
            wallInfo = Physics2D.Raycast(groundDetection.position, Vector2.right);
        }
        if (!noGround)
        {
            if (wallInfo.distance <= 1 && wallInfo.collider != playerObject)
            {
                if (movingRight == true)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;
                }
            }
        }

    }

    void HurtPlayer()
    {
        player.currentHealth -= 1;
        player.GetComponent<PlayerBehaviour>().Damaged();
        AudioManager.PlaySound("Hit");
    }

    void Death()
    {
        gameObject.SetActive(false);
    }

    void DieSound()
    {
        AudioManager.PlaySound("EnemyDie");
    }
}
