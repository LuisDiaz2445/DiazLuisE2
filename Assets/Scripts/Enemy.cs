using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public int PtsKill = 50;

    [HideInInspector]
    public bool mustPatrol;
    private bool mustFlip;

    public Rigidbody2D rb;
    public Transform ground;
    public LayerMask wall;
    public LayerMask groundLayer;
    public LayerMask enemy;
    public Collider2D body;
    bool canMove;

    public delegate void Fnotify();
    public static event Fnotify OnEnemyDeath;

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
    private void Awake()
    {
        GameManager.EnemyAmount++;
    }

    private void Start()
    {
        mustPatrol = true;
    }
    private void Update()
    {
        if (mustPatrol && canMove == true)
        {
            Patrol();
        }
    }

    private void FixedUpdate()
    {
        if (mustPatrol)
        {
            mustFlip = !Physics2D.OverlapCircle(ground.position, 0.1f, groundLayer);
        }
    }
    void Patrol()
    {
        if (mustFlip || body.IsTouchingLayers(enemy) || body.IsTouchingLayers(wall))
        {
            Flip();
        }
        rb.velocity = new Vector2(speed * Time.fixedDeltaTime, rb.velocity.y);
    }

    void Flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        speed *= -1;
        mustPatrol = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Killer>())
        {
            OnEnemyDeath.Invoke();
            ScoreManager.Points += PtsKill;
            Destroy(gameObject);

        }
    }
}
