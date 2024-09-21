using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemManager.ItemType itemtype;

    // 플레이어와 충돌 시 아이템의 효과를 적용하는 메서드
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어와 충돌했는지 확인
        {
            // ApplyItemEffect(other.gameObject); // 효과 적용
            gameObject.SetActive(false);
            // Destroy(gameObject); // 아이템을 사용 후 파괴
        }
    }    
}   
