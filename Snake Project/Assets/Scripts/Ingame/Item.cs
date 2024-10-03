using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemManager.ItemType itemtype;
    private SnakeManager snakeManager;

    // 플레이어와 충돌 시 아이템의 효과를 적용하는 메서드
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Snake Head")) // 플레이어와 충돌했는지 확인
        {
            ApplyItemEffect(other.gameObject); // 효과 적용
        }        
    }    

    // 아이템 효과 적용 로직
    private void ApplyItemEffect(GameObject player)
    {
        snakeManager = SnakeManager.Instance;
        
        if (snakeManager != null)
        {
            switch (itemtype)
            {
                case ItemManager.ItemType.APPLE:
                    snakeManager.AddBodyPart(); // 사과는 몸체 추가
                    player.GetComponentInChildren<PlayerCanvas>().LevelUp(); // 레벨업 호출
                    break;
                case ItemManager.ItemType.SPEED:
                    snakeManager.ApplySpeedBoost(5f, 5f); // 스피드 부스트
                    break;
                case ItemManager.ItemType.MAGNET:
                    snakeManager.ActivateMagnet(5f); // 자석 활성화
                    break;
            }
        }

        // 아이템을 풀로 반환
        PoolManager.Instance.ReturnToPool(gameObject);
    }
}   
