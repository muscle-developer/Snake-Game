using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    private Vector3 direction = Vector3.forward; // 현재 이동 방향
    private Vector3 targetDirection = Vector3.forward; // 목표 이동 방향

    void Update()
    {
        // 입력에 따라 목표 방향을 업데이트
        Vector3 inputDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) inputDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) inputDirection += Vector3.back;
        if (Input.GetKey(KeyCode.A)) inputDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D)) inputDirection += Vector3.right;

        // 입력이 있는 경우에, 방향의 크기 1로 만든다.
        if (inputDirection != Vector3.zero)
        {
            targetDirection = inputDirection.normalized;
        }
    }

    void FixedUpdate()
    {
        // 현재 방향을 목표 방향으로 부드럽게 전환
        direction = Vector3.Slerp(direction, targetDirection, SnakeManager.Instance.rotationSpeed * Time.deltaTime).normalized;

        // 현재 방향으로 이동
        transform.position += direction * SnakeManager.Instance.snkaeSpeed * Time.deltaTime;

        // 목표 회전 방향으로 부드럽게 회전
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, SnakeManager.Instance.rotationSpeed * Time.deltaTime);

        // Store position history
        SnakeManager.Instance.PositionsHistory.Insert(0, transform.position); 

        // PositionsHistory 크기 제한
        if (SnakeManager.Instance.PositionsHistory.Count > 1000)
        {
            SnakeManager.Instance.PositionsHistory.RemoveAt(SnakeManager.Instance.PositionsHistory.Count - 1);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("apple"))
        {
            Destroy(other.gameObject);
            SnakeManager.Instance.AddBodyPart();
        }
    }
}
