using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingHUDNickName : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget; // 따라갈 타겟

    [Header("닉네임")]
    [SerializeField]
    private TMP_Text nickNameText; 
    [SerializeField]
    private List<Color> nickNameColorList;

    [SerializeField]
    private bool isFollowing = true; // HUD가 타겟을 따라가는지 여부
    [SerializeField]
    private float nickNameYOffset = 5f; // NickName HUD의 Y축 오프셋, 캐릭터 위로 띄워지는 거리 조정

    private Camera mainCamera; // 메인 카메라 참조

    void LateUpdate()
    {
        if (mainCamera == null)
            mainCamera = Camera.main; // 메인 카메라 참조가 없다면 찾기

        if (followTarget != null && isFollowing) // 타겟이 존재하고 추적이 활성화된 경우
        {
            // 닉네임 텍스트 위치 설정
            Vector3 nickNamePosition = followTarget.position + new Vector3(0f, nickNameYOffset, 0f);
            nickNameText.transform.position = nickNamePosition;

            // HUD가 카메라를 바라보도록 설정
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
    }

    // HUD 초기화
    public void Init(Transform target, bool isPlayer, bool isFollowing, string nickname)
    {
        this.followTarget = target; // 따라갈 타겟 설정
        this.isFollowing = isFollowing; // 추적 여부 설정
        nickNameText.color = isPlayer ? nickNameColorList[0] : nickNameColorList[1]; // 플레이어와 적의 닉네임 색상 구분
        nickNameText.text = nickname; // 닉네임 설정
    }

    // 스네이크가 죽었을 때 레벨 텍스트 없애기
    public void DestroyHUD()
    {
        if (gameObject != null)
        {
            Destroy(gameObject); // 게임 오브젝트 삭제
        }
    }
}
