using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageManager : MonoBehaviour
{

    public static List<GameObject> garbage = new List<GameObject>();
    // Start is called before the first frame update

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Garbage()
    {
        for(int i = 0; i < garbage.Count; i++)
        {
            Destroy(garbage[i]);
        }
    }
}
