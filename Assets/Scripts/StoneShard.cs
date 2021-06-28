using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneShard : MonoBehaviour
{
    // Start is called before the first frame update

    public float atkDamage;
    public float attackDamage;
    Rigidbody rigid;
    public GameObject pellet;
    public int teamNumber;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        atkDamage = atkDamage + attackDamage;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 25;

        
    }


    void OnTriggerEnter(Collider col)
    {

        if(col.gameObject.tag == "Border")
        {
            Destroy(gameObject);
        }

        if (col.gameObject.tag == "Boulder")
        {
            col.gameObject.GetComponent<Boulder>().health -= 5;
            StartCoroutine("Pellets");
        }

        if (col.gameObject.tag == "Structure")
        {
            StartCoroutine("Pellets");
        }
    }


    private IEnumerator Pellets()
    {
        if (teamNumber == 1)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject pelletInstance = (GameObject)Instantiate(pellet, gameObject.transform.position, gameObject.transform.rotation);
                pelletInstance.tag = "TeamOnePellet";
                pelletInstance.GetComponent<Pellet>().attackDamage = pelletInstance.GetComponent<Pellet>().attackDamage + attackDamage;
                pelletInstance.GetComponent<Rigidbody>().velocity = gameObject.transform.GetChild(i).transform.forward * 15;
                pelletInstance.GetComponent<Renderer>().material.color = Color.blue;
                DimensionManager.garbage.Add(pelletInstance);
            }
        }

        if (teamNumber == 2)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject pelletInstance = (GameObject)Instantiate(pellet, gameObject.transform.position, gameObject.transform.GetChild(i).rotation);
                pelletInstance.tag = "TeamTwoPellet";
                pelletInstance.GetComponent<Pellet>().attackDamage = pelletInstance.GetComponent<Pellet>().attackDamage + attackDamage;
                pelletInstance.GetComponent<Rigidbody>().velocity = gameObject.transform.GetChild(i).transform.forward * 15;
                pelletInstance.GetComponent<Renderer>().material.color = Color.red;
                DimensionManager.garbage.Add(pelletInstance);
            }
        }

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshFilter>().mesh = null;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

}
