using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public CreateMap createMap;
    public List<GameObject> blocks = new List<GameObject>();

    void Start()
    {
        
        createMap = (CreateMap)FindObjectOfType(typeof(CreateMap));
    }

    void Update()
    {
        
    }
}
