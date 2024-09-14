using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public VariableJoystick joystick; // 조이스틱을 사용하기 위한 변수 
    private Vector3 direction = Vector3.forward; // 현재 이동 방향
    private Vector3 targetDirection = Vector3.forward; // 목표 이동 방향

    void Update()
    {
        // 조이스틱의 입력 방향을 가져오기
        Vector2 joystickDirection = joystick.Direction;

        // 조이스틱 입력이 있는 경우에 목표 방향을 업데이트
        if(joystickDirection != Vector2.zero)
            targetDirection = new Vector3(joystickDirection.x, 0, joystickDirection.y).normalized;
        // 조이스틱 입력이 없을 때는 현재 목표 방향(바라보는 방향) 유지
        else 
            targetDirection = direction; 

        // 입력에 따라 목표 방향을 업데이트 (조이스틱 사용 x 키보드 사용)
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
        // 현재 방향을 목표 방향으로 부드럽게 전환 (Slerp는 두 벡터 간의 구면 선형 보간을 수행)
        direction = Vector3.Slerp(direction, targetDirection, SnakeManager.Instance.rotationSpeed * Time.deltaTime).normalized;

        // 현재 방향으로 이동 (SnakeManager의 속도와 Time.deltaTime을 곱해 프레임 독립적인 움직임을 보장)
        transform.position += direction * SnakeManager.Instance.snkaeSpeed * Time.deltaTime;

        // 현재 방향으로 목표 회전 방향을 설정 (LookRotation은 벡터 방향으로의 회전값을 계산)
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // 현재 회전 방향에서 목표 회전 방향으로 부드럽게 회전 (RotateTowards는 두 회전 값 사이에서 점진적으로 회전)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, SnakeManager.Instance.rotationSpeed * Time.deltaTime);

        // 현재 위치를 PositionsHistory에 저장 (위치 히스토리의 가장 앞에 현재 위치를 삽입)
        SnakeManager.Instance.PositionsHistory.Insert(0, transform.position);

        // PositionsHistory의 크기를 1000으로 제한 (1000개를 초과하면 가장 오래된 항목을 제거)
        if (SnakeManager.Instance.PositionsHistory.Count > 1000)
        {
            SnakeManager.Instance.PositionsHistory.RemoveAt(SnakeManager.Instance.PositionsHistory.Count - 1);
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Apple"))
        {
            Destroy(other.gameObject);
            SnakeManager.Instance.AddBodyPart();
        }
        else if(other.CompareTag("Spped"))
        {

        }
        else if(other.CompareTag("Magnet"))
        {

        }
        else if(other.CompareTag("Wall"))
        {
            this.gameObject.GetComponent<Collider>().isTrigger = false;
        }
    }

    public bool isAllowMoveInput = true; // 이동 입력을 허용할지 여부를 나타내는 변수

    // 새로운 이동 방향을 설정하는 메서드입니다.
    public void MoveInput(Vector3 newMoveDirection)
    {
        // 이동 입력이 허용된 경우에만 동작합니다.
        if (isAllowMoveInput)
        {
            // newMoveDirection 벡터를 정규화하여 targetDirection에 할당합니다.
            direction = newMoveDirection.normalized;
        }
    }

    public bool isMoveInputExist; // 입력이 존재하는지를 나타내는 변수

    // 입력 존재 여부를 설정하는 메서드입니다.
    public void IsMoveInputExist(bool newIsMoveInputExistState)
    {
        isMoveInputExist = newIsMoveInputExistState;
    }
}
