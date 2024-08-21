using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instace;
    public SnakeHead player;

    public void Awake()
    {
        GameManager.Instace = this;
    }
}
