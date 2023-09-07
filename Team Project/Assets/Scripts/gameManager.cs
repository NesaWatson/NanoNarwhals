using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public GameObject player;

    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        
    }
}
