using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public PlayerMech player;
    public static PlayerMech Player;

    void Awake()
    {
        if (player)
            Player = player;

        Cursor.visible = false;
    }
}