using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instace;

    public void Awake()
    {
        GameManager.Instace = this;
    }
}
