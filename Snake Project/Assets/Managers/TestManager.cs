using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEditor;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    // 전역에서 사용할 수 있도록 싱글톤화
    public static TestManager Instance;
    public float snkaeSpeed = 280.0f;
    public float turnSpeed = 180.0f;
    public float distanceBetween = 0.2f;
    public List<GameObject> bodyParts = new List<GameObject>();
    public List<GameObject> snakeBody = new List<GameObject>();
    private float countUp = 0f;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    void Start()
    {   
        AddBodyPart();
    }

    void FixedUpdate()
    {
        HandleBodyAddition();
    }

    // private void AddBodyPart()
    // {
    //     if(snakeBody.Count == 0)
    //     {
    //         GameObject temp1 = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
    //         if(!temp1.GetComponent<SnakeBody>())
    //             temp1.GetComponent<SnakeBody>();
    //         if(!temp1.GetComponent<Rigidbody>())
    //         {
    //             temp1.AddComponent<Rigidbody>();
    //             temp1.GetComponent<Rigidbody>().useGravity = false;
    //         }
    //     }

    //     SnakeBody body = snakeBody[snakeBody.Count - 1].GetComponent<SnakeBody>();
    //     if(countUp == 0)
    //     {
    //         body.ClearBodyList();
    //     }
    //     countUp += Time.deltaTime;
    //     if(countUp >= distanceBetween)
    //     {
    //         GameObject temp = Instantiate(bodyParts[0], body.bodyList[0].postion, body.bodyList[0].rotation, transform);
    //         if(!temp.GetComponent<SnakeBody>())
    //             temp.GetComponent<SnakeBody>();
    //         if(!temp.GetComponent<Rigidbody>())
    //         {
    //             temp.AddComponent<Rigidbody>();
    //             temp.GetComponent<Rigidbody>().useGravity = false;
    //         }

    //         snakeBody.Add(temp);
    //         bodyParts.RemoveAt(0);
    //         temp.GetComponent<SnakeBody>().ClearBodyList();
    //         countUp = 0;
    //     }
    // }

    private void AddBodyPart()
    {
        GameObject newPart = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
        SetupBodyPart(newPart);
        snakeBody.Add(newPart);
        bodyParts.RemoveAt(0);
    }

    private void HandleBodyAddition()
    {
        if (snakeBody.Count > 0)
        {
            SnakeBody lastBodyPart = snakeBody[snakeBody.Count - 1].GetComponent<SnakeBody>();
            if (countUp == 0)
            {
                lastBodyPart.ClearBodyList();
            }
            countUp += Time.deltaTime;
            if (countUp >= distanceBetween)
            {
                Vector3 newPos = lastBodyPart.bodyList[0].postion;
                Quaternion newRot = lastBodyPart.bodyList[0].rotation;
                GameObject newPart = Instantiate(bodyParts[0], newPos, newRot, transform);
                SetupBodyPart(newPart);

                snakeBody.Add(newPart);
                bodyParts.RemoveAt(0);
                newPart.GetComponent<SnakeBody>().ClearBodyList();
                countUp = 0;
            }
        }
    }

    private void SetupBodyPart(GameObject bodyPart)
    {
        if (!bodyPart.GetComponent<SnakeBody>())
        {
            bodyPart.AddComponent<SnakeBody>();
        }
        if (!bodyPart.GetComponent<Rigidbody>())
        {
            Rigidbody rb = bodyPart.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }
    }
}
