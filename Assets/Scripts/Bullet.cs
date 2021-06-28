using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    
    // Start is called before the first frame update
    public float atkDamage;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Border")
        {
            
            Destroy(gameObject);
        }

        if(col.gameObject.tag == "Boulder")
        {
            
            Destroy(gameObject);
        }

        if (col.gameObject.tag == "Structure")
        {
            Destroy(gameObject);
        }
    }

    
}
