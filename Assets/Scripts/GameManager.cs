using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditorInternal;
using UnityEngine.UI;


public enum GameState
{
    GameOver, RoundOver, GamePlay, Wait
}
public class GameManager : MonoBehaviour
{
    float time = 0f;
    float EndTime = 150f;
    public GameObject TimeText;

    public static int EnemyAmount = 6;
    public static int enemyKill = 0;
    public GameState gameState;

    static GameManager instance;
    
    public static GameManager Instance { get { return instance; } } //singleton//

    private void Awake()
    {
        instance = this;
    }

    public delegate void FNotify();
    public static event FNotify OnGameOver;
    public static event FNotify OnRoundOver;
    public static event FNotify OnGamePlay;
    public static event FNotify OnWait;

    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= PlayerController_OnPlayerDeath;
        Enemy.OnEnemyDeath -= Enemy_OnEnemyDeath;
    }
    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += PlayerController_OnPlayerDeath;
        Enemy.OnEnemyDeath += Enemy_OnEnemyDeath;
    }

    void PlayerController_OnPlayerDeath()
    {
        OnGameOver.Invoke();
    }

    void Enemy_OnEnemyDeath()
    {
        enemyKill++;
    }

    private void Updategs(GameState currentState)
    {
        gameState = currentState;

        switch (currentState)
        {
            case GameState.GameOver:
                break;
            case GameState.RoundOver:
                break;
            case GameState.GamePlay:
                break;
            case GameState.Wait:
                break;
            default:
                break;
        }
    }
    private void Start()
    {
        gameState = GameState.Wait;
        OnWait.Invoke();
        TimeText.GetComponent<Text>().text = EndTime.ToString("0");
        Debug.Log(EnemyAmount);

    }

    private void Update()
    {
        if (gameState == GameState.Wait)
        {
            time += Time.deltaTime;
        }
        if (time >= 2f)
        {
            Debug.Log("Start!");
            gameState = GameState.GamePlay;
            OnGamePlay.Invoke();
            time = 0;
        }
        if (gameState == GameState.GamePlay)
        {
            EndTime -= Time.deltaTime;
            TimeText.GetComponent<Text>().text = EndTime.ToString("0");
            if (EndTime<= 0)
            {
                gameState = GameState.GameOver;
                OnGameOver.Invoke();
                Debug.Log("Game Over");
            }
        }
        if (gameState == GameState.GamePlay)
        {
            if (enemyKill >= EnemyAmount)
            {
                gameState = GameState.RoundOver;
                OnRoundOver.Invoke();
                Debug.Log("You Win!");
            }
        }
    }
}

