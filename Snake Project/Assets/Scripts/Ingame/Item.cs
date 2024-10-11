using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemManager.ItemType itemtype;
    private SnakeManager snakeManager;
    private EnemySnakeManager enemySnakeManager;

    // 플레이어와 충돌 시 아이템의 효과를 적용하는 메서드
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Snake Head")) // 플레이어와 충돌했는지 확인
        {
            ApplyItemEffect(other.gameObject, true); // 효과 적용
        }        
        else if(other.CompareTag("Enemy Snake"))
        {
            ApplyItemEffect(other.gameObject, false); // 적 스네이크에 대한 효과 적용
        }
    }    

    // 아이템 효과 적용 로직
    private void ApplyItemEffect(GameObject snake, bool isPlayer)
    {
        snakeManager = SnakeManager.Instance;
        enemySnakeManager = EnemySnakeManager.Instance;

        if (isPlayer && snakeManager != null)
        {
            switch (itemtype)
            {
                case ItemManager.ItemType.APPLE:
                    snakeManager.AddBodyPart(); // 사과는 몸체 추가
                    snake.GetComponentInChildren<PlayerCanvas>().LevelUp(); // 레벨업 호출
                    break;
                case ItemManager.ItemType.SPEED:
                    snakeManager.ApplySpeedBoost(5f, 5f); // 스피드 부스트
                    break;
                case ItemManager.ItemType.MAGNET:
                    snakeManager.ActivateMagnet(5f); // 자석 활성화
                    break;
            }
        }
        else if (!isPlayer && enemySnakeManager != null)
        {
            // 적 스네이크가 사과를 먹었을 때 몸체를 추가
            EnemySnake enemySnake = snake.GetComponent<EnemySnake>();
            if (enemySnake != null && itemtype == ItemManager.ItemType.APPLE)
            {
                enemySnake.AddBodyPart(); // 적 스네이크 몸체 추가
            }
        }

        // 아이템을 풀로 반환
        PoolManager.Instance.ReturnToPool(gameObject);
    }
}   
