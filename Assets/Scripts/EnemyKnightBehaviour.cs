using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnightBehaviour : MonoBehaviour
{
    [SerializeField]
    float chaseRange;

    [SerializeField]
    float attackRange;

    [SerializeField]
    float dashRange;

    [SerializeField]
    float speed = 4;

    [SerializeField]
    float dashSpeed = 6;

    public int health = 10;
    public float dashTimer;
    public float dashTime;
    public float time;
    public bool isDead = false;
    public bool touchingPlayer = true;

    private bool startChase;
    private bool canDash = true;
    private bool movingRight = true;
    private bool isHit = false;

    public Material newMaterial;
    public Material oldMaterial;
    public Transform playerDetection;
    public GameObject playerObject;
    public PlayerBehaviour player;
    private AudioSource runSound;

    Animator knightAnimator;
    Rigidbody2D rb2d;
    SpriteRenderer knightRenderer;

    private enum States {CHASE, ATTACK,DASH,PATROL,DEAD};
    private States state = States.CHASE;

    // Start is called before the first frame update
    void Start()
    {
        knightAnimator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        knightRenderer = GetComponent<SpriteRenderer>();
        runSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canDash)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashTime)
            {
                canDash = true;
                dashTimer = 0f;
            }
        }

        if (isHit)
        {
            time += Time.deltaTime;
            if (time >= 0.100)
            {
                knightRenderer.material = oldMaterial;
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
            if (!isDead) 
            {
                if (distToPlayer <= attackRange)
                {
                    state = States.ATTACK;
                }
                else if (distToPlayer <= chaseRange)
                {
                    knightAnimator.SetTrigger("Wake");
                    state = States.CHASE;
                }
                else if (distToPlayer <= dashRange && startChase)
                {
                    if (canDash)
                    {
                        state = States.DASH;
                    }
                    else
                    {
                        state = States.CHASE;
                    }
                }
                else if (distToPlayer >= chaseRange && startChase)
                {
                    state = States.PATROL;
                }
            }
            else
            {
                state = States.DEAD;
            }
           
        }
        else
        {
            state = States.PATROL;
        }

        switch (state)
        {
            case States.CHASE:
                if (startChase)
                {
                    Chase();
                }
                break;
            case States.ATTACK:
                rb2d.velocity = Vector2.zero;
                knightAnimator.SetTrigger("Attack");
                break;
            case States.DASH:
                Dash();
                break;
            case States.PATROL:
                Patrol();
                break;
            case States.DEAD:
               rb2d.velocity = Vector2.zero;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals("Projectile"))
        {
            if (startChase && !isDead)
            {
                knightRenderer.material = newMaterial;
                isHit = true;
                AudioManager.PlaySound("Hurt");
                health -= 1;
                if (health == 0)
                {
                    isDead = true;
                    AudioManager.PlaySound("KnightDie");
                    knightAnimator.SetTrigger("Death");
                }
            }
        }

        if (c.gameObject.layer == 6 || c.gameObject.layer == 9)
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
    private void Chase()
    {
        knightAnimator.SetTrigger("Run");

        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (transform.position.x > player.transform.position.x)
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

    private void StartChase()
    {
        startChase = true;
    }
    void Patrol()
    {
        knightAnimator.SetTrigger("Run");

        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (movingRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }

    }

    private void Dash()
    {
        RaycastHit2D wallInfo;
        if (movingRight)
        {
           wallInfo = Physics2D.Raycast(playerDetection.position, Vector2.right);
           movingRight = true;
        }
        else
        {
            wallInfo = Physics2D.Raycast(playerDetection.position, Vector2.left);
            movingRight = false;
        }
        if (wallInfo.collider.tag.Equals("Player"))
        {
            transform.Translate(Vector2.right * dashSpeed * Time.deltaTime);
            knightAnimator.SetTrigger("Dash");
            AudioManager.PlaySound("Dash");
            canDash = false;
            state = States.CHASE;
            if (wallInfo.distance <= 3.5)
            {
                HurtPlayer();
            }
        }
    }

    public void ChangePos()
    {
        if (movingRight)
        {
            transform.position = transform.position + new Vector3(2.47f, 0, 0);
        }
        else
        {
            transform.position = transform.position - new Vector3(2.47f, 0, 0);
        }
        
    }

    void HurtPlayer()
    {
        player.currentHealth -= 1;
        player.GetComponent<PlayerBehaviour>().Damaged();
        AudioManager.PlaySound("Hit");
    }

    private void Walk()
    {
        runSound.Play();
    }
}
