using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEditor;
using UnityEngine;

// 스네이크 관리를 위한 매니저 스크립트
// 전역 시스템 관리: 게임의 전역적인 상태나 시스템을 관리합니다.
// 싱글톤 패턴: 종종 싱글톤 패턴을 사용하여 게임 내에서 하나의 인스턴스만 존재하도록 합니다.
// 다양한 기능: 게임 상태 관리, 리소스 로딩, 오디오 관리, 데이터 저장 및 로드, 레벨 전환, UI 관리 등 다양한 기능을 담당할 수 있습니다.
// 역할: 게임의 전역적인 상태와 시스템을 관리. 주로 스네이크의 몸체를 추가하고, 몸체의 리스트를 관리하는 역할을 합니다.
public class TestManager : MonoBehaviour
{
    // 전역에서 사용할 수 있도록 싱글톤화
    public static TestManager Instance;
    public float snkaeSpeed = 280.0f;
    public float rotationSpeed = 180.0f;
    public float distanceBetween = 0.2f;
    public List<GameObject> bodyParts = new List<GameObject>();
    public List<GameObject> snakeBody = new List<GameObject>();

    // void Awake()
    // {
    //     if(Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //         Destroy(this.gameObject);
    // }

    // public void AddBodyPart()
    // {
    //     if(bodyParts.Count > 0)
    //     {
    //         GameObject newPart = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
    //         SetupBodyPart(newPart);
    //         snakeBody.Add(newPart);
    //         bodyParts.RemoveAt(0);

    //         int index = snakeBody.IndexOf(newPart);
    //         if (index > 0)
    //         {
    //             newPart.GetComponent<SnakeBody>().Target = snakeBody[index - 1].transform;
    //         }
    //     }
    // }

    // public void HandleBodyAddition()
    // {
    //     if (snakeBody.Count > 0)
    //     {
    //         SnakeBody lastBodyPart = snakeBody[snakeBody.Count - 1].GetComponent<SnakeBody>();
    //         if (countUp == 0)
    //         {
    //             lastBodyPart.ClearBodyList();
    //         }
    //         countUp += Time.deltaTime;
    //         if (countUp >= distanceBetween && bodyParts.Count > 0)
    //         {
    //             Vector3 newPos = lastBodyPart.bodyList[0].position;
    //             Quaternion newRot = lastBodyPart.bodyList[0].rotation;
    //             GameObject newPart = Instantiate(bodyParts[0], newPos, newRot, transform);
    //             SetupBodyPart(newPart);

    //             snakeBody.Add(newPart);
    //             bodyParts.RemoveAt(0);
    //             newPart.GetComponent<SnakeBody>().ClearBodyList();

    //             int index = snakeBody.IndexOf(newPart);
    //             if (index > 0)
    //             {
    //                 newPart.GetComponent<SnakeBody>().Target = snakeBody[index - 1].transform;
    //             }

    //             countUp = 0;
    //         }
    //     }
    // }

    // private void SetupBodyPart(GameObject bodyPart)
    // {
    //     if (!bodyPart.GetComponent<SnakeBody>())
    //     {
    //         bodyPart.AddComponent<SnakeBody>();
    //     }
    //     if (!bodyPart.GetComponent<Rigidbody>())
    //     {
    //         Rigidbody rb = bodyPart.AddComponent<Rigidbody>();
    //         rb.useGravity = false;
    //     }
    // }
}
