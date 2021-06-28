using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    // Start is called before the first frame update
    public float attackDamage;
    Rigidbody rigid;
    public float speed;
    public bool left;
    public bool isHit;
    public bool inRange;
    public Vector3 direction;
    public float health = 5;
    public GameObject mesh;
    public GameObject particle;
    Vector3 pos;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHit)
        {
            if (left)
            {
                rigid.velocity = transform.forward * speed;
            }
            else
            {
                rigid.velocity = transform.forward * speed;
            }
        }


        if(isHit)
        {
            rigid.velocity = direction * 55;
        }
        mesh.transform.Rotate(0, 0, 2, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Boulder")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if(other.gameObject.tag == "Boulder" && gameObject.tag == "SpecialHazard")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if(other.gameObject.tag == "HazardCollider")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Border" && gameObject.tag == "HazardOne")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Border" && gameObject.tag == "HazardTwo")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(gameObject);
        }



        if (other.gameObject.tag == "ArmOne")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            inRange = true;
            direction = other.transform.root.transform.forward;
        }

        if (other.gameObject.tag == "ArmTwo")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            inRange = true;
            direction = other.transform.root.transform.forward;
        }

        if (other.gameObject.tag == "ArmOne" && other.transform.root.transform.GetComponent<MechController>().isMelee == true && inRange)
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            gameObject.tag = "HazardOne";
            isHit = true;
        }

        if (other.gameObject.tag == "ArmTwo" && other.transform.root.transform.GetComponent<MechController>().isMelee == true && inRange)
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f); gameObject.tag = "HazardTwo";
            
            
            isHit = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {

       

        if (other.gameObject.tag == "TeamOneBullet")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            health -= 1;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "TeamTwoBullet")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            health -= 1;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "TeamOnePellet")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "TeamTwoPellet")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(gameObject);
        }


        if (other.gameObject.tag == "HazardCollider")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "ArmOne")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            inRange = true;
            direction = other.transform.root.transform.forward;
        }

        if (other.gameObject.tag == "ArmTwo")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            inRange = true;
            direction = other.transform.root.transform.forward;
        }

        if (other.gameObject.tag == "ArmOne" && other.transform.root.transform.GetComponent<MechController>().isMelee == true && inRange)
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            gameObject.tag = "HazardOne";
            
            
            isHit = true;
        }

        if (other.gameObject.tag == "ArmTwo" && other.transform.root.transform.GetComponent<MechController>().isMelee == true && inRange)
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            gameObject.tag = "HazardTwo";

            
            
            isHit = true;
        }

        if(other.gameObject.tag == "TA1_Bullet")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "TA2_Bullet")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(gameObject);
        }

        if(other.gameObject.tag == "HazardOne")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "HazardTwo")
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if(other.gameObject.tag == "Border")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "TeamOneBullet" && health <= 0)
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "TeamTwoBullet" && health <= 0)
        {
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

    }
}
