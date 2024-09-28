using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCanvas : MonoBehaviour
{   
    [SerializeField]
    private TMP_Text levelText;
    private int level; // 플레이어 레벨
    private Camera mainCamera;

    void Start()
    {
        level = 0; // 초기 레벨 설정
        mainCamera = Camera.main; // 메인 카메라를 찾음
        UpdateLevelText();
    }

    void Update()
    {
        LookAtCamera(); // 매 프레임마다 텍스트가 카메라를 바라보도록 함
    }

    public void LevelUp()
    {
        level++; // 레벨 증가
        UpdateLevelText();
    }

    private void UpdateLevelText()
    {
        levelText.text = "Lv. " + level; // 텍스트 업데이트
    }

    private void LookAtCamera()
    {
        // 텍스트가 항상 카메라를 바라보도록 회전
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                         mainCamera.transform.rotation * Vector3.up);
    }
}
