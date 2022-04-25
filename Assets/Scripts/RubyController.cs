using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip interact;
    public AudioClip jambiSound;

    public GameObject backgroundMusic;
    public GameObject winSound;
    public GameObject loseSound;
    public GameObject halfText;
    public GameObject healthDecrease;
    public GameObject healthIncrease;
    public Text scoreText;
    public Text winText;
    public Text loseText;
    public Text cogCount;


    public float speed = 3.0f;

    public int maxHealth = 5;
    public int ammo { get { return currentAmmo; } }
    public int currentAmmo;
    private int scoreAmount;
    private bool gameOver;
    public static int level;
    private static int scoreValue = 0;

    public GameObject projectilePrefab;

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        scoreText.text = "Robots Fixed: " + scoreValue.ToString();
        currentAmmo = 4;
        gameOver = false;
        winSound.SetActive(false);
        loseSound.SetActive(false);
        halfText.gameObject.SetActive(false);
        cogCount.text = "Cog Ammo: " + currentAmmo;

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Fixed Robots: " + scoreValue.ToString();
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.C) && currentAmmo > 0)
        {
            Launch();
            currentAmmo -= 1;
            cogCount.text = "Cog Ammo: " + currentAmmo;
        }

        if (Input.GetKey(KeyCode.R))

        {

            if (gameOver == true)

            {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();

        }


        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                    PlaySound(jambiSound);
                    if (scoreValue == 4)
                    {

                        SceneManager.LoadScene("Level Two");
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                Wizard character = hit.collider.GetComponent<Wizard>();
                if (character != null)
                {
                    character.DisplayDialog();
                    PlaySound(interact);

                }
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeScore(int ScoreAmount)
    {

        scoreValue += 1;
        scoreText.text = "Robots Fixed: " + scoreText.ToString();
        SetHalfText();
        SetWinText();

    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            GameObject projectilePrefab = Instantiate(healthDecrease, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            PlaySound(hitSound);
        }
        if (amount > 0)
        {
            GameObject projectilePrefab = Instantiate(healthIncrease, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        if (health <= 0)
        {
            loseText.text = "You Lost! Press R to restart";
            gameOver = true;
            speed = 0;
            backgroundMusic.SetActive(false);
            loseSound.SetActive(true);
            

        }
    }
    private void SetWinText()
    {
        if (scoreValue == 8)
        {
            winText.text = "You Win! Game Created by Nicolas Duque";
            backgroundMusic.SetActive(false);
            winSound.SetActive(true);
            gameOver = true;
        }
    }

    private void SetHalfText()
    {
        if (scoreValue == 4)
        {
            halfText.gameObject.SetActive(true);
        }

    }

    public void ChangeAmmo(int count)
    {
        currentAmmo += count;
        cogCount.text = "Cog Ammo: " + currentAmmo;
    }





    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(throwSound);

    }

}