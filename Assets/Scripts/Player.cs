using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce = 10f;
    private float direction = 0f;
    private Rigidbody2D rb;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;
    private Animator anim;

    private Vector3 respawnPoint;
    public GameObject fallDecetor;

    public Text scoreText;
    public HealthBar healthBar;


    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawnPoint = transform.position;
        scoreText.text = "Score : "+Scoring.totalScore;
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);
        direction = Input.GetAxis("Horizontal");
        
        if(direction >0)
        {
            rb.velocity = new Vector2(direction * speed ,rb.velocity.y);
            transform.localScale = new Vector2(0.3f,0.3f);
        }else if(direction <0)
        {
            rb.velocity = new Vector2(direction * speed ,rb.velocity.y);
            transform.localScale = new Vector2(-0.3f,0.3f);
        }else
        {
            rb.velocity = new Vector2(0,rb.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.Space) &&isTouchingGround)
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
        }
        anim.SetFloat("Speed",Mathf.Abs(rb.velocity.x));
        anim.SetBool("OnGround",isTouchingGround);

        fallDecetor.transform.position = new Vector2(transform.position.x,fallDecetor.transform.position.y);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "FallDector")
        {
            transform.position = respawnPoint;
        }
        else if(other.tag == "CheckPoint")
        {
            respawnPoint = transform.position;
        }
        else if(other.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            respawnPoint = transform.position;
        }
        else if(other.tag == "PreviosLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
            respawnPoint = transform.position;
        }
        else if(other.tag == "Crystal")
        {
            Scoring.totalScore +=1;
            scoreText.text = "Score : "+Scoring.totalScore;
            other.gameObject.SetActive(false);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Spike")
        {
            healthBar.Damage(0.002f);
        }
    }
}
