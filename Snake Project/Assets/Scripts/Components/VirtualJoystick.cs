using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IEndDragHandler
{
    private bool isPressed; // 조이스틱이 눌려 있는지 여부를 나타내는 변수
    
    [Header("Rect References")]
    public RectTransform containerRect; // 조이스틱의 컨테이너(RectTransform)
    public RectTransform handleRect; // 조이스틱의 핸들(RectTransform)
    public Image containerRectImage; // 조이스틱 컨테이너의 이미지 컴포넌트
    public Image handleRectImage; // 조이스틱 핸들의 이미지 컴포넌트

    [Header("Settings")]
    public float joystickRange = 50f; // 조이스틱 핸들의 이동 범위
    public float magnitudeMultiplier = 1f; // 조이스틱 입력 벡터의 크기를 조절하는 배율
    public bool invertXOutputValue; // X축 방향의 출력을 반전시킬지 여부
    public bool invertYOutputValue; // Y축 방향의 출력을 반전시킬지 여부

    [Header("Output")]
    public UnityEvent<Vector2, bool> joystickOutputEvent; // 조이스틱의 입력 값을 이벤트로 전달하는 이벤트

    void Start()
    {
        SetupHandle(); // 조이스틱 핸들을 초기 위치로 설정
    }

    void Update()
    {
        if (!isPressed) // 조이스틱이 눌려 있지 않을 때
        {
            OutputPointerEventValue(Vector2.zero, false); // 입력이 없음을 이벤트로 전달

            if (handleRect)
            {
                UpdateHandleRectPosition(Vector2.zero); // 핸들을 중앙으로 이동
            }

            // 투명도 설정
            containerRectImage.color = new Color(1f, 1f, 1f, 0.075f); // 컨테이너의 색상을 변경
            handleRectImage.color = new Color(1f, 1f, 1f, 0.2f); // 핸들의 색상을 변경
        }
    }

    private void SetupHandle()
    {
        if (handleRect)
        {
            UpdateHandleRectPosition(Vector2.zero); // 핸들을 초기 위치로 설정
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true; // 조이스틱이 눌려진 상태로 설정
        OnDrag(eventData); // 드래그 이벤트를 호출하여 핸들 위치 업데이트
        containerRectImage.color = new Color(1f, 1f, 1f, 0.15f); // 컨테이너 색상 변경
        handleRectImage.color = new Color(1f, 1f, 1f, 0.7f); // 핸들 색상 변경
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 화면 좌표를 로컬 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);
    
        position = ApplySizeDelta(position); // 조이스틱 크기에 맞게 위치 조정
        
        Vector2 clampedPosition = ClampValuesToMagnitude(position); // 위치의 크기를 제한

        Vector2 outputPosition = ApplyInversionFilter(clampedPosition); // 출력 값에 반전을 적용

        OutputPointerEventValue(outputPosition * magnitudeMultiplier, true); // 조이스틱의 입력 값을 이벤트로 전달

        if (handleRect)
        {
            UpdateHandleRectPosition(clampedPosition * joystickRange); // 핸들 위치 업데이트
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false; // 조이스틱이 눌려지지 않은 상태로 설정

        OutputPointerEventValue(Vector2.zero, false); // 입력이 없음을 이벤트로 전달

        if (handleRect)
        {
            UpdateHandleRectPosition(Vector2.zero); // 핸들을 중앙으로 이동
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isPressed = false; // 드래그가 끝났음을 설정

        OutputPointerEventValue(Vector2.zero, false); // 입력이 없음을 이벤트로 전달

        if (handleRect)
        {
            UpdateHandleRectPosition(Vector2.zero); // 핸들을 중앙으로 이동
        }
    }

    private void OutputPointerEventValue(Vector2 pointerPosition, bool inputExist)
    {
        joystickOutputEvent?.Invoke(pointerPosition, inputExist); // 입력 값을 이벤트로 전달
    }

    private void UpdateHandleRectPosition(Vector2 newPosition)
    {
        handleRect.anchoredPosition = newPosition; // 핸들의 위치를 새로운 위치로 업데이트
    }

    Vector2 ApplySizeDelta(Vector2 position)
    {
        // 조이스틱의 크기에 맞게 위치 조정
        float x = (position.x / containerRect.sizeDelta.x) * 2.5f;
        float y = (position.y / containerRect.sizeDelta.y) * 2.5f;
        return new Vector2(x, y);
    }

    Vector2 ClampValuesToMagnitude(Vector2 position)
    {
        // 조이스틱 입력 벡터의 크기를 1로 제한
        return Vector2.ClampMagnitude(position, 1);
    }

    Vector2 ApplyInversionFilter(Vector2 position)
    {
        if (invertXOutputValue)
        {
            position.x = InvertValue(position.x); // X축 방향 값 반전
        }

        if (invertYOutputValue)
        {
            position.y = InvertValue(position.y); // Y축 방향 값 반전
        }

        return position;
    }

    float InvertValue(float value)
    {
        // 값을 반전시켜 -value 반환
        return -value;
    }
}
