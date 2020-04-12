using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{

    public static GameAssets instance;

    public static GameAssets getInstance() 
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }


    public Transform pfBlockHead;
    public Transform pfBlockBody;
}
