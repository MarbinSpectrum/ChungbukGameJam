using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public PlayerDataFromJson playerData;

    private void Awake()
    {
        if (instance)
            Destroy(instance);
        instance = this;
    }

    private void Start() => playerData = GetComponent<PlayerDataFromJson>();
    

}

