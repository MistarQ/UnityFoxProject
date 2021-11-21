using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public Joystick joystick;
    public Button jumpButton;

    [SerializeField]private Rigidbody2D rb;
    private Animator anim;
    public LayerMask ground;
    public Collider2D coll;
    public int Cherry = 0;

    public Text CherryNum;

    public Transform groundCheck;
    public float speed, jumpForce;
    public bool isGround, isJump;

    bool jumpPressed;
    int jumpCount;

    private bool isHurt;

    //public AudioSource jumpAudio;
    //public AudioSource hurtedAudio, cherryAudio;

    public Collider2D DisColl;

    private bool hurtecd = true;

    public Transform CellingCheck;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    public void ButtonJump() {
        jumpPressed = true;
    }

    void Update()
    {

        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        // if (joystick.Vertical >0.5f && jumpCount > 0)
            {
            jumpPressed = true;
        }
        // CherryNum.text = Cherry.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        if (!isHurt)
        {
            GroundMovement();
            Jump();

        }

        Crouch();
        SwitchAnim();
    }

    void GroundMovement()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        //float horizontalMove = joystick.Horizontal;
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            if (horizontalMove > 0f)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (horizontalMove <0f) {
                transform.localScale = new Vector3(-1, 1, 1);
            }
           
        }
        
    }

    //跳跃
    void Jump()
    {
        
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            //jumpAudio.Play();
            SoundManager.instance.JumpAudio();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && !isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //jumpAudio.Play();
            SoundManager.instance.JumpAudio();
            jumpCount--;
            jumpPressed = false;
        }
    }

    //动画
    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if (isHurt)
        {
            anim.SetBool("hurt", true);
            if (hurtecd)
            {
                //hurtedAudio.Play();
                SoundManager.instance.HurtAudio();
                hurtecd = false;
            }
            // anim.SetFloat("running", 0f);
            if (Mathf.Abs(rb.velocity.x) < 6f)
            {
                anim.SetBool("hurt", false);
                anim.SetBool("idle", true);
                isHurt = false;
                hurtecd = true;
            }
        }

        if (isGround)
        {
            anim.SetBool("falling", false);
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 收集物品
        if (collision.tag == "Collection") {
            //cherryAudio.Play();
            // Destroy(collision.gameObject);
            // Cherry += 1;
            SoundManager.instance.CherryAudio();
            collision.GetComponent<Animator>().Play("isGot");
            // CherryNum.text = Cherry.ToString();
        }

        if (collision.tag == "DeadLine")
        {
            // GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 2f);
        }
    }

    // 消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)  
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // Enemy_Frog frog = collision.gameObject.GetComponent<Enemy_Frog>();
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                // Destroy(collision.gameObject);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                anim.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x) {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)  
            {
                rb.velocity = new Vector2(10, rb.velocity.y);
                isHurt = true;
            }
        }
    }

    void Crouch1()
    {
        if (!Physics2D.OverlapCircle(CellingCheck.position, 0.2f, ground)) { 
            if (Input.GetButtonDown("Crouch"))
            {
                anim.SetBool("crouching", true);
                DisColl.enabled = false;
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                anim.SetBool("crouching", false);
                DisColl.enabled = true;
            }
        }
    }

    //蹲下
    void Crouch() {
        if (Input.GetButton("Crouch")) {
        //if (joystick.Vertical<-0.5f)
            
                anim.SetBool("crouching", true);
            DisColl.enabled = false;
        }
        if (!Input.GetButton("Crouch") && !Physics2D.OverlapCircle(CellingCheck.position, 0.2f, ground)) {
        //if (joystick.Vertical >= -0.5f && !Physics2D.OverlapCircle(CellingCheck.position, 0.2f, ground))
            
                anim.SetBool("crouching", false);
            DisColl.enabled = true;
        }
    }

    void Restart() {
        
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void CherryCount() {
        Cherry += 1;
    }
}
