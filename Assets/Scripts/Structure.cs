using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{

    public enum Dimension {Stone, Lava};
    public Dimension dimension;
    public GameObject part;
    public bool tutorial;

    Vector3 pos;

    public bool border;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {

        if(col.gameObject.tag == "Boulder" && border)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
        }

        if(col.gameObject.tag == "TeamOneBullet")
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);

        }

        if (col.gameObject.tag == "TeamTwoBullet")
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "TA1_Bullet")
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "TA2_Bullet")
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "HazardOne" && border)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "HazardTwo" && border)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "SmallAIBullet")
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "TeamOnePellet")
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "TeamTwoPellet")
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "TeamTwoPellet")
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "TeamOneStoneShard")
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "TeamTwoStoneShard")
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
        }

        if(col.gameObject.tag == "AIBulletRed")
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
        }

        if(col.gameObject.tag == "AIBulletBlue")
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(part, pos, part.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
        }
    }



}
