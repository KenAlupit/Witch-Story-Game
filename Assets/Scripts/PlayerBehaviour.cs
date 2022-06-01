using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    public float speed = 5;
    public int health = 4;
    public int currentHealth;
    public int numOfHearts = 4;
    public float currentSpeed;
    public float jumpHeight = 5;
    public float currentJumpHeight;
    public float speedMultiplier = 1.5f;
    public float jumpMultiplier = 2f;

    public float jumpPUTimer = 0f;
    public float speedPUTimer = 0f;
    public float speedjumpPUTimer = 0f;
    public float healthPUTimer = 0f;
    public float cancelPUTimer = 0f;

    public float hitTime = 0f;
    public bool isGrounded = false;

    private float direction = 1;
    private bool isHit = false;
    private string Ground;
    private float horizontalInput;
    private float time = 0f;
    private float threshold = 5f;

    public Transform bulletSpawnLocation;
    public GameObject bulletPrefab;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public TextMeshProUGUI buffTimer;
    public TextMeshProUGUI buffName;
    public GameObject deathScreen;
    public Material newMaterial;
    public Material oldMaterial;
    private AudioSource walkSound;

    SpriteRenderer playerRenderer;
    Animator playerAnimator;
    Rigidbody2D playerRigidbody;
    Vector2 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = transform.position;
        playerRigidbody = GetComponent<Rigidbody2D>();

        currentJumpHeight = jumpHeight;
        currentSpeed = speed;
        currentHealth = health;

        playerRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();

        walkSound = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        float dirx = currentSpeed * Time.deltaTime * horizontalInput;
        transform.Translate(dirx, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y < -10)
        {
            currentHealth = 0;
        }

        if (isHit)
        {
            hitTime += Time.deltaTime;
            if (hitTime >= 0.100)
            {
                playerRenderer.material = oldMaterial;
                hitTime = 0f;
                isHit = false;
            }
        }

        horizontalInput = Input.GetAxis("Horizontal");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        if (hit)
        {
            Ground = hit.collider.tag;
        }


        ShootBullets();
        if (isGrounded && Input.GetKey(KeyCode.A) || isGrounded && Input.GetKey(KeyCode.D))
        {
            playerAnimator.SetTrigger("Walk");

        } else if (isGrounded == false)
        {
            playerAnimator.SetTrigger("Jump");
        }
        else if (isGrounded)
        {
            playerAnimator.SetTrigger("Idle");
            time += Time.deltaTime;
            if (time >= threshold)
            {
                playerAnimator.SetTrigger("ThumbsUp");
                time = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (!UIManager.GameIsPaused)
            {
                AudioManager.PlaySound("Jump");
                playerRigidbody.AddForce(Vector2.up * currentJumpHeight, ForceMode2D.Impulse);
            }
        }

        if (jumpPUTimer > 0)
        {
            ShowTimer(jumpPUTimer);
            buffName.text = "Jump";
            jumpPUTimer -= Time.deltaTime;
            if (jumpPUTimer <= 0)
            {
                SetBuffToNull();
                jumpPUTimer = 0;
                currentJumpHeight = jumpHeight;
            }
        }

        if (speedPUTimer > 0)
        {
            ShowTimer(speedPUTimer);
            buffName.text = "Speed";
            speedPUTimer -= Time.deltaTime;
            if (speedPUTimer <= 0)
            {
                SetBuffToNull();
                speedPUTimer = 0;
                currentSpeed = speed;
            }
        }

        if (speedjumpPUTimer > 0)
        {
            ShowTimer(speedjumpPUTimer);
            buffName.text = "Speed & Jump";
            speedjumpPUTimer -= Time.deltaTime;
            if (speedjumpPUTimer <= 0)
            {
                SetBuffToNull();
                speedjumpPUTimer = 0;
                currentJumpHeight = jumpHeight;
                currentSpeed = speed;
            }
        }
        if (healthPUTimer > 0)
        {
            buffTimer.text = null;
            buffName.text = "Health Potion";
            healthPUTimer -= Time.deltaTime;
            if (healthPUTimer <= 0)
            {
                SetBuffToNull();
                healthPUTimer = 0;
            }
        }
        if (cancelPUTimer > 0)
        {
            buffTimer.text = null;
            buffName.text = "Cancel Potion";
            cancelPUTimer -= Time.deltaTime;
            if (cancelPUTimer <= 0)
            {
                SetBuffToNull();
                cancelPUTimer = 0;
            }
        }
        if (currentHealth == 0)
        {
            playerAnimator.SetTrigger("Death");
        }

        if(currentHealth > numOfHearts)
        {
            currentHealth = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < currentHealth)
            {
                
                hearts[i].sprite = fullHeart;
            } else
            {
                
                hearts[i].sprite = emptyHeart;
            }

            if(i < numOfHearts)
            {
                
                hearts[i].enabled = true;
            } else
            {
                
                hearts[i].enabled = false;
            }
        }
    }

    void ShootBullets()
    {
        if (horizontalInput > 0)
        {
            playerRenderer.flipX = false;
            bulletSpawnLocation.position = transform.position + new Vector3(0.75f, 0, 0);
            direction = 1;
        } else if (horizontalInput < 0)
        {
            playerRenderer.flipX = true;
            bulletSpawnLocation.position = transform.position - new Vector3(0.75f, 0, 0);
            direction = -1;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!UIManager.GameIsPaused)
            {
                playerAnimator.SetTrigger("Shoot");
                time = 0f;
                AudioManager.PlaySound("Shoot");
                GameObject g = Instantiate(bulletPrefab, bulletSpawnLocation.position, Quaternion.identity);
                g.GetComponent<Bullet>().speed *= direction;
                Destroy(g, 3);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals("Ground") || c.gameObject.tag.Equals("Destructable"))
        {
            if (c.gameObject.tag.Equals(Ground))
            {
                AudioManager.PlaySound("Landing");
                isGrounded = true;
            }
        }
        
        if (c.gameObject.tag.Equals("HealthPotion"))
        {
            Debug.Log(c.gameObject.tag);
            AudioManager.PlaySound("PowerUp");
            currentHealth = health;
            healthPUTimer = 1;
            c.gameObject.SetActive(false);
        }

        if (c.gameObject.tag.Equals("JumpPowerUp"))
        {
            Debug.Log(c.gameObject.tag);
            AudioManager.PlaySound("PowerUp");
            currentJumpHeight = jumpHeight * jumpMultiplier;
            jumpPUTimer = 10;
            PowerUpRespawn jpu = c.gameObject.GetComponent<PowerUpRespawn>();
            jpu.HidePowerup();
        }

        if (c.gameObject.tag.Equals("CancelJump"))
        {
            SetBuffToNull();
            Debug.Log(c.gameObject.tag);
            AudioManager.PlaySound("PowerUp");
            cancelPUTimer = 1;
            jumpPUTimer = 0;
            speedjumpPUTimer = 0;
            currentJumpHeight = jumpHeight;
            currentSpeed = speed;
            PowerUpRespawn cj = c.gameObject.GetComponent<PowerUpRespawn>();
            cj.HidePowerup();
        }

        if (c.gameObject.tag.Equals("SpeedPowerUp"))
        {
            Debug.Log(c.gameObject.tag);
            AudioManager.PlaySound("PowerUp");
            speedPUTimer = 10;
            currentSpeed = speed * speedMultiplier;
            PowerUpRespawn SPU = c.gameObject.GetComponent<PowerUpRespawn>();
            SPU.HidePowerup();
        }

        if (c.gameObject.tag.Equals("Jump&SpeedPowerUp"))
        {
            Debug.Log(c.gameObject.tag);
            AudioManager.PlaySound("PowerUp");
            currentSpeed = speed * speedMultiplier;
            currentJumpHeight = jumpHeight * jumpMultiplier;
            jumpPUTimer = 10;
            speedjumpPUTimer = 10;
            PowerUpRespawn JSPU = c.gameObject.GetComponent<PowerUpRespawn>();
            JSPU.HidePowerup();
        }
    }

    private void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals("Ground") || c.gameObject.tag.Equals("Destructable"))
        {
            isGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals("Ground") || c.gameObject.tag.Equals("Destructable"))
        {
            isGrounded = true;
        }

    }
    private void Walk()
    {
        walkSound.Play();
    }

    private void Death()
    {
        SetBuffToNull();
        gameObject.SetActive(false);
        deathScreen.SetActive(true);
    }

    private void ShowTimer(float time)
    {
        time = (int)time;
        buffTimer.text = time.ToString();
    }

    private void SetBuffToNull()
    {
        buffName.text = null;
        buffTimer.text = null;
    }

    public void Damaged()
    {
        playerRenderer.material = newMaterial;
        isHit = true;
    }
}
