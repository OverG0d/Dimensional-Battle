using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHazard : MonoBehaviour
{

    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public bool boss;
    public bool overTime_Damage;
    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();

        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    



    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        if(boss)
        {
            if (other.gameObject.tag == "Player")
            {
                float damage = 2f;
                other.gameObject.GetComponent<MechController>().health -=  (int)(damage * (100 / (100 + other.gameObject.GetComponent<MechController>().currentArmor)));
                other.gameObject.transform.GetChild(1).GetComponent<Animator>().SetTrigger("Hit");
            }

            if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<MechController>().health <= 0)
            {
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                if (other.gameObject.GetComponent<MechController>().team == MechController.Team.TeamOne)
                {
                    TeamManager.numofTeamOnePlayers--;
                }

                if (other.gameObject.GetComponent<MechController>().team == MechController.Team.TeamTwo)
                {
                    TeamManager.numofTeamTwoPlayers--;
                    Debug.Log(TeamManager.numofTeamOnePlayers);
                }
                other.gameObject.GetComponent<MechController>().teamManager.GetComponent<TeamManager>().teamOne.Remove(other.gameObject);
                other.gameObject.GetComponent<MechController>().StartCoroutine("Death");
            }
        }
        

    }

}
