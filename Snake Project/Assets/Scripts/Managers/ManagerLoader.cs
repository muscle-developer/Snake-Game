using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerLoader : MonoBehaviour
{
    [SerializeField]
    private Transform managersPrefab;

    private void Awake()
    {
        var manager = GameObject.Find("Managers");

        // 게임 시작시에 매니저를 로드하자.
        if(manager == null)
        {
            var newManager = Instantiate(managersPrefab);
            newManager.name = "Managers";
        }
    }
}
