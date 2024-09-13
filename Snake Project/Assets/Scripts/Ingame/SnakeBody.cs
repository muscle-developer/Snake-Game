using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    void FixedUpdate()
    {
        // 몸체를 움직이기 위한 인덱스 초기화   
        int index = 0;
        var snakeManager = SnakeManager.Instance;
        // 스네이크의 모든 몸체를 순회
        foreach (var body in snakeManager.BodyParts) 
        {
            // PositionsHistory에서 각 몸체의 위치를 참조
            Vector3 point = snakeManager.PositionsHistory[Mathf.Clamp(index * snakeManager.gap, 0, snakeManager.PositionsHistory.Count - 1)];
            
            // 목표 지점으로의 이동 방향 계산
            Vector3 moveDirection = point - body.transform.position;

            // 몸체를 계산된 이동 방향으로 이동시킴
            body.transform.position += moveDirection * snakeManager.bodySpeed * Time.deltaTime;

            // 이동 방향이 0이 아닐 때, 즉 움직임이 있는 경우
            if (moveDirection != Vector3.zero)
            {
                // 부드러운 회전을 위해 Quaternion.Slerp를 사용하여 목표 회전 방향으로 회전
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                body.transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRotation, snakeManager.bodySpeed * Time.deltaTime);

                // 대안으로 body.transform.LookAt(point);를 사용할 수 있음 (코멘트 처리된 코드)
            }

            // 인덱스를 증가시켜 다음 몸체로 이동
            index++;
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if(other.CompareTag("EnemyHead"))
    //     {
    //         Destroy(other.gameObject);
    //     }
    // }
}
