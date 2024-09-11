using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    void FixedUpdate()
    {
        // Move body parts
        int index = 0;
        var snakeManager = SnakeManager.Instance;
        foreach (var body in snakeManager.BodyParts) 
        {
            Vector3 point = snakeManager.PositionsHistory[Mathf.Clamp(index * snakeManager.gap, 0, snakeManager.PositionsHistory.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;

            body.transform.position += moveDirection * snakeManager.bodySpeed * Time.deltaTime;

            if (moveDirection != Vector3.zero)
            {
                // 부드러운 회전을 위해 Slerp 사용
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                body.transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRotation, snakeManager.bodySpeed * Time.deltaTime);

                // body.transform.LookAt(point);
            }

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
