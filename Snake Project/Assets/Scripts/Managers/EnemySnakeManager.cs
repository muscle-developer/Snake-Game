using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnakeManager : MonoBehaviour
{
    public static EnemySnakeManager Instance;

    [Header("적 스네이크 관리")]
    public float bodySpeed = 5.0f; // 몸체가 따라오는 속도
    public float gap = 10f; // 몸체 간의 거리
    public float detectionRange = 15f; // 플레이어 탐지 범위

    public List<EnemySnake> enemySnakes = new List<EnemySnake>(); // 적 스네이크 리스트

    // References
    public GameObject enemyBodyPrefab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }   

    // 적 스네이크 초기화
    public void InitializeEnemySnake(GameObject enemySnakeObject)
    {
        EnemySnake enemySnake = enemySnakeObject.GetComponent<EnemySnake>();

        // 적 스네이크 리스트에 추가
        enemySnakes.Add(enemySnake);
    }

    // 적 스네이크 제거
    public void DestroyEnemySnake(EnemySnake enemySnake)
    {
        enemySnakes.Remove(enemySnake);
        Destroy(enemySnake.gameObject);
    }
}
