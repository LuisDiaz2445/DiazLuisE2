using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;


public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;
    Collider2D col;
    public float moveSpeed = 3f;
    public float jumpForce = 10f;
    bool inside;
    bool insideTrigger;
    public GameObject[] doors;
    public int idDoor;
    float KillerTimer = 0f;
    public bool KillerEnable = false;
    bool canMove;

    public delegate void FNotify();
    public static event FNotify OnPlayerDeath;

    private void OnDisable()
    {
        GameManager.OnGameOver -= GameManager_OnGameOver;
        GameManager.OnRoundOver -= GameManager_OnRoundOver;
        GameManager.OnGamePlay -= GameManager_OnGamePlay;
        GameManager.OnWait -= GameManager_OnWait;
    }
    private void OnEnable()
    {
        GameManager.OnGameOver += GameManager_OnGameOver;
        GameManager.OnRoundOver += GameManager_OnRoundOver;
        GameManager.OnGamePlay += GameManager_OnGamePlay;
        GameManager.OnWait += GameManager_OnWait;
    }
    public void GameManager_OnGameOver()
    {
        canMove = false;
    }
    public void GameManager_OnRoundOver()
    {
        canMove = false;
    }
    public void GameManager_OnGamePlay()
    {
        canMove = true;
    }
    public void GameManager_OnWait()
    {
        canMove = false;
    }

    List<Color> DcolorList = new List<Color>()
    {
        Color.blue,
        Color.red,
        Color.green
    };
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

    }
    // Start is called before the first frame update
    void Start()
    {
        inside = false;
        insideTrigger = false;
    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        Vector2 targetVelocity = Vector2.right * x * moveSpeed;
        targetVelocity.y = rb.velocity.y;

        rb.velocity  = targetVelocity; 
        sprite.flipX = x < 0;
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    void Jump()
    {
        if (Mathf.Abs(rb.velocity.y) > 0f) return;

        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        
    }
    private void Update()
    {
        if (canMove==true && inside == false)
        {
            MovePlayer();
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && inside == true && KillerEnable == false)
        {
            int numintem = 0;
            inside = false;
            col.isTrigger = false;
            rb.constraints = RigidbodyConstraints2D.None;
            sprite.enabled = !sprite.enabled;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            Color actualcolor = doors[idDoor].gameObject.GetComponent<Door>().GetComponent<SpriteRenderer>().color;
            foreach (var item in DcolorList)
            {
                if (actualcolor == item)
                {
                    if (numintem == 2) { numintem = 0; } else { numintem++; }
                    doors[idDoor].gameObject.GetComponent<Door>().GetComponent<SpriteRenderer>().color = DcolorList[numintem];
                }
                numintem++;               
            }

            doors[idDoor].transform.GetChild(1).gameObject.SetActive(true);
            int i = 0;
            foreach (var item in doors)
            {
                if (actualcolor == item.gameObject.GetComponent<Door>().GetComponent<SpriteRenderer>().color)
                {
                    doors[i].transform.GetChild(1).gameObject.SetActive(true);
                    KillerEnable = true;
                }
                i++;
            }
        }
        if (KillerEnable == true)
        {
            KillerTimer += Time.deltaTime;
            if (KillerTimer >= 0.15f)
            {
                for (int d = 0; d < doors.Length; d++)
                {
                    doors[d].transform.GetChild(1).gameObject.SetActive(false);
                }
                KillerTimer = 0;
                KillerEnable = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && insideTrigger == true && inside == false)
        {
            inside = true;
            col.isTrigger = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            sprite.enabled = !sprite.enabled;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            GameManager.Instance.gameState = GameState.GameOver;
            OnPlayerDeath.Invoke();
            Debug.Log("Game Over");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Door>())
        {
            insideTrigger = true;
            idDoor = Convert.ToInt32(collision.name);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Door>())
        {
            insideTrigger = false;
        }
    }
}
