using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // 아이템의 타입을 정의하는 ItemType
    public ItemManager.ItemType itemtype;

    // SnakeManager와 EnemySnakeManager 참조
    private SnakeManager snakeManager;
    private EnemySnakeManager enemySnakeManager;

    // 플레이어 또는 적 스네이크와 충돌 시 아이템의 효과를 적용하는 메서드
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어의 SnakeHead와 충돌했는지 확인
        if (other.CompareTag("Snake Head")) 
        {
            ApplyItemEffect(other.gameObject, true); // 플레이어에게 효과 적용
        }
        // 적 스네이크와 충돌했는지 확인
        else if(other.CompareTag("Enemy Snake"))
        {
            ApplyItemEffect(other.gameObject, false); // 적 스네이크에게 효과 적용
        }
    }    

    // 아이템 효과를 적용하는 메서드
    // isPlayer가 true면 플레이어에게, false면 적 스네이크에게 효과 적용
    private void ApplyItemEffect(GameObject snake, bool isPlayer)
    {
        // SnakeManager와 EnemySnakeManager의 인스턴스를 참조
        snakeManager = SnakeManager.Instance;
        enemySnakeManager = EnemySnakeManager.Instance;

        // 플레이어가 아이템과 충돌한 경우
        if (isPlayer && snakeManager != null)
        {
            // 아이템 종류에 따라 다른 효과를 적용
            switch (itemtype)
            {
                case ItemManager.ItemType.APPLE:
                    snakeManager.AddBodyPart(); // 사과는 플레이어 몸체 추가
                    snake.GetComponentInChildren<SnakeCanvas>().LevelUp(); // 레벨업 UI 업데이트
                    break;
                case ItemManager.ItemType.SPEED:
                    snakeManager.ApplySpeedBoost(5f, 5f); // 스피드 부스트 효과 적용
                    break;
                case ItemManager.ItemType.MAGNET:
                    snakeManager.ActivateMagnet(5f); // 자석 효과 활성화
                    break;
            }
        }
        // 적 스네이크가 아이템과 충돌한 경우
        else if (!isPlayer && enemySnakeManager != null)
        {
            // 적 스네이크가 사과를 먹었을 때 몸체를 추가
            EnemySnake enemySnake = snake.GetComponent<EnemySnake>();
            if (enemySnake != null && itemtype == ItemManager.ItemType.APPLE)
            {
                enemySnake.AddBodyPart(); // 적 스네이크 몸체 추가
                snake.GetComponentInChildren<SnakeCanvas>().LevelUp(); // 적 스네이크 레벨업 UI 업데이트
            }
        }

        // 아이템 사용 후 풀로 반환
        PoolManager.Instance.ReturnToPool(gameObject);
    }
}
