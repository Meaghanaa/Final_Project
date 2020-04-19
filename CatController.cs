using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CatController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float speed;

    public Text countText;
    public Text winText;
    private int count;
    public Text livesText;
    private int lives;
    public Text checkText;
    public Text timeText;

    public AudioSource musicSource;
    public AudioSource musicSource2;
    public AudioSource birdCaw;
    public AudioSource coin;
    AudioSource audioSource;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioClip birdClip;
    public AudioClip coinClip;


    Animator anim;
    private bool facingRight = true;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public float thrust;

    private bool restart;

    public float timeLeft = 90.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        count = 0;
        lives = 3;
        SetCountText();
        SetlivesText();
        winText.text = "";
        checkText.text = "";
        musicSource.clip = musicClipOne;
        musicSource.Play();
        anim = GetComponent<Animator>();
        restart = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rb2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        Debug.Log(hozMovement + "//" + vertMovement + "//" + speed);
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("State", 2);
        }
        if (isOnGround = false)
        {
            anim.SetInteger("State", 2);
        }
        float hozMovement = Input.GetAxis("Horizontal");

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
            anim.SetInteger("State", 1);
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
            anim.SetInteger("State", 1);
        }

        if (restart)
            restart = true;
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }

        timeLeft -= Time.deltaTime;
        timeText.text = "Time left: " + (timeLeft).ToString("0");
        if (timeLeft < 0)
        {
            Destroy(this);
            winText.text = "You Lose!";
        }

    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            Destroy(collision.collider.gameObject);
            count = count + 1;
            audioSource.clip = coinClip;
            coin.PlayOneShot(coinClip);
            audioSource.loop = false;
            SetCountText();

        }
        else if (collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);
            lives = lives - 1;
            SetlivesText();
        }
        if (collision.collider.tag == "Ground")
        {
            anim.SetInteger("State", 0);
        }
        if (collision.collider.tag == "Bird")
        {
            rb2d.AddForce(transform.up * thrust, ForceMode2D.Impulse);
            anim.SetInteger("State", 2);
            audioSource.clip = birdClip;
            birdCaw.PlayOneShot(birdClip);
            audioSource.loop = false;
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "Level_flag")
        {
            transform.position = new Vector2(205.0f, 0.0f);
            lives = 3;
            SetlivesText();
        }
        if (collision.collider.tag == "Complete_flag")
        {
            Destroy(this);
            winText.text = "You win! Game created by Meaghan Archer!";
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            if (count <= 3)
            {
                checkText.text = count.ToString() + "/8! Nice Try...";
            }
            if (count <= 5 && count > 3)
            {
                checkText.text = count.ToString() + "/8! Not bad for your average cat!";
            }
            if (count <= 8 && count >= 6)
            {
                checkText.text = count.ToString() + "/8! Way to go!";
            }
        }
    }
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString() + "/8";
    }
    void SetlivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives == 0)
        {
            Destroy(this);
            winText.text = "You Lose!";
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKeyDown (KeyCode.W))
            {
                rb2d.velocity = new Vector2(0, 30);
            }
        }
    }
}
