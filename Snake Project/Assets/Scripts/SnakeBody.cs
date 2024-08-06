using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    public class Body
    {
        public Vector3 postion;
        public Quaternion rotation;

        public Body(Vector3 pos, Quaternion rot)
        {
            postion = pos;
            rotation = rot;
        }
    }

    public List<Body> bodyList = new List<Body>();
    public float bodySpeed = 5;
    public int gap = 10;
    
    void FixedUpdate()
    {
        UpdateBodyList();
        // Move body parts
        // int index = 0;
        // var headPos = SnakeManager.Instance.HeadPositions;

        // if(headPos.Count < 2)
        //     return;

        // for (int index = 0; index < SnakeManager.Instance.BodyParts.Count; index++)
        // {
        //     var body = SnakeManager.Instance.BodyParts[index];
        //     int targetIndex = Mathf.Clamp(index * gap, 0, headPos.Count - 1);
        //     Vector3 point = headPos[targetIndex];

        //     Vector3 moveDirection = point - body.transform.position;
        //     if (moveDirection.sqrMagnitude > 0.001f) // 너무 가까울 때는 이동하지 않음
        //     {
        //         body.transform.position += moveDirection.normalized * bodySpeed * Time.deltaTime;
        //         body.transform.rotation = Quaternion.LookRotation(moveDirection);
        //     }
        // }
    }

    public void UpdateBodyList()
    {
        bodyList.Add(new Body(transform.position, transform.rotation));
    }

    public void ClearBodyList()
    {
        bodyList.Clear();
        bodyList.Add(new Body(transform.position, transform.rotation));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EnemyHead"))
        {
            Destroy(other.gameObject);
        }
    }
}
