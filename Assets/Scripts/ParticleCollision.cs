using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        if (other.gameObject.tag == "Player" && other.GetComponent<MechController>().team == MechController.Team.TeamOne)
        {
            other.gameObject.GetComponent<MechController>().StartCoroutine("Hit");
        }
    }

 


}
