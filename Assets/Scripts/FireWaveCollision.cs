using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWaveCollision : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public MechController mechController;
    public float attackDamage;

    public float atkDamage;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        Destroy(gameObject, 3.5f);
        atkDamage = atkDamage + attackDamage;
    }

    void Update()
    {
       
    }

    

  

 
}
