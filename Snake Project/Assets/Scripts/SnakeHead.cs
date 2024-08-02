using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;    // 이동 속도
    [SerializeField]
    private float rotationSpeed = 180.0f; // 회전 속도
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
        direction = Vector3.Slerp(direction, targetDirection, rotationSpeed * Time.deltaTime).normalized;

        // 현재 방향으로 이동
        transform.position += direction * moveSpeed * Time.deltaTime;

        // 목표 회전 방향으로 부드럽게 회전
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
