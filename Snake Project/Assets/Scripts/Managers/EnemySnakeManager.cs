using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnakeManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static EnemySnakeManager Instance;

    [Header("적 스네이크 관리")]
    public float bodySpeed = 5.0f; // 적 스네이크 몸체의 이동 속도
    public float gap = 10f; // 몸체 간의 거리
    public float detectionRange = 15f; // 플레이어를 탐지할 수 있는 범위

    // 적 스네이크 리스트
    public List<EnemySnake> enemySnakes = new List<EnemySnake>(); // 현재 활성화된 모든 적 스네이크를 저장하는 리스트

    // References
    public GameObject enemyBodyPrefab; // 적 스네이크 몸체의 프리팹

    void Awake()
    {
        // 싱글톤 패턴 구현 - 인스턴스가 없을 경우 현재 객체를 인스턴스로 지정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(this.gameObject); // 인스턴스가 이미 존재하면 중복 방지를 위해 파괴
        }
    }   

    // 적 스네이크 초기화 함수
    public void InitializeEnemySnake(GameObject enemySnakeObject)
    {
        EnemySnake enemySnake = enemySnakeObject.GetComponent<EnemySnake>();

        // 적 스네이크를 리스트에 추가하여 관리
        enemySnakes.Add(enemySnake);
    }

    // 적 스네이크 제거 함수
    public void DestroyEnemySnake(EnemySnake enemySnake)
    {
        // 적 스네이크 리스트에서 제거
        enemySnakes.Remove(enemySnake);

        // 해당 적 스네이크 객체 파괴
        Destroy(enemySnake.gameObject);
    }

    // 특정 적 스네이크의 인덱스를 반환하는 함수 (레벨 증가 시 특정 적 스네이크를 식별하기 위해 사용)
    public int GetEnemyIndex(EnemySnake enemySnake)
    {
        return enemySnakes.IndexOf(enemySnake); // 리스트에서 적 스네이크의 위치 반환
    }
}
