using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 따라갈 대상

    public bool isCustomDistance; // 사용자 직접 설정 - true 자동 거리 계산 - false
    public Vector3 baseDistance; // 카메라 - 타겟 사이의 기본 거리
    private Vector3 currentDistance; // 동적으로 조절되는 거리

    public float smoothSpeed = 0.1f; // 타겟을 따라가는 속도

    private void Start()
    {
        StartCoroutine(FindTargetCoroutine());
    }

    private IEnumerator FindTargetCoroutine()
    {
        while (target == null)
        {
            target = FindObjectOfType<SnakeHead>()?.transform; // ?.는 널 조건부 연산자로, FindObjectOfType<SnakeHead>()가 null인 경우 안전하게 target을 null로 설정합니다.
            // target = FindObjectOfType<EnemySnake>()?.transform; // ?.는 널 조건부 연산자로, FindObjectOfType<SnakeHead>()가 null인 경우 안전하게 target을 null로 설정합니다.
            yield return null;
        }
        
        // 자동 거리 계산
        if (!isCustomDistance)
        {
            baseDistance = transform.position - target.position;
            currentDistance = baseDistance * 0.2f; // 기본 거리의 20%로 시작
        }

        currentDistance = baseDistance; // 초기 거리를 현재 거리로 설정
    }

    private void LateUpdate()
    {
        AdjustCameraOffset();
        SmoothFollow();   
    }

    private void AdjustCameraOffset()
    {
        int bodyCount = SnakeManager.Instance.BodyParts.Count;

        // 갯수를 홀수로 설정한 이유는 BodyParts.Count의 갯수에 머리 갯수도 포함되기때문에 +1씩
        if(bodyCount > 31)
        {
            currentDistance = baseDistance * 0.6f;
        }
        else if (bodyCount > 21)
        {
            currentDistance = baseDistance * 0.5f;
        }
        else if (bodyCount >= 11) 
        {
            currentDistance = baseDistance * 0.35f; // 40개 이상일 때 70% 거리 증가
        } 
        else if(bodyCount >= 6)
        {
            currentDistance = baseDistance * 0.25f; // 10개 이상일 때 50% 거리 증가
        }
        else 
        {
            currentDistance = baseDistance * 0.15f; // 기본 거리의 20%로 시작
        }
    }

    public void SmoothFollow()
    {
        if(target == null)
            return;

        // target의 현재 위치에 offset을 더해 targetPos를 계산합니다. 이 위치는 카메라가 목표로 하는 위치입니다.
        Vector3 targetPos = target.position + currentDistance;
        // smoothSpeed 값에 따라 현재 위치와 목표 위치 사이를 천천히 이동하게 됩니다. 이로 인해 카메라가 target을 부드럽게 따라가게 됩니다.
        Vector3 smoothFollow = Vector3.Lerp(transform.position,targetPos, smoothSpeed);

        transform.position = smoothFollow; // 카메라 위치 업데이트
        transform.LookAt(target); // 타겟을 바라보도록 설정
    }
}