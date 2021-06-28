using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SmallAI : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject aiBullet;
    public int randNum;
    public int min;
    public int max;
    public List<GameObject> bullets = new List<GameObject>();
    public float health;
    public float attackDamage;
    public float armor;
    
    public GameObject health_buff;
    public GameObject attackDamage_buff;
    public GameObject armor_buff;
    public static bool isQuitting;
    public Transform target;
    public Quaternion targetRotation;
    public float rotationSpeed;
    private Quaternion _lookRotation;
    private Vector3 _direction;
    public int teamNumber = 1;
    public GameObject chosenPlayer;
    public SkinnedMeshRenderer renderer;
    public bool melee;
    public GameObject render;
    public GameObject explosion;
    public static bool supermech_Blue;
    public static bool supermech_Red;
    public GameObject superMech_Blue;
    public GameObject superMech_Red;
    public Material material;
    public GameObject mesh;
    public GameObject particle;
    Rigidbody rigid;
    Vector3 pos;

    public AudioClip shoot;
    public AudioClip AI_explosion;
    public AudioClip hit;
    AudioSource source;
    public Animator anim;

    public Text hitNumber;
    public Text hitNumber_2;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        InvokeRepeating("AIBehaviour", 0.1f, 5f);
        int randNum = Random.Range(5, 10);
        InvokeRepeating("Shoot", 2f, randNum);
        source = GetComponent<AudioSource>();
        StartCoroutine("Invincible");
    }

    // Update is called once per frame
    void Update()
    {
        if(TeamManager.numofTeamOnePlayers == 0 || TeamManager.numofTeamTwoPlayers == 0)
        {
            //gameObject.GetComponent<NavMeshAgent>().enabled = false;
            //Destroy(gameObject);
            CancelInvoke("Shoot");

        }

        if (health > 0 && (!supermech_Red || !supermech_Blue))
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 2.6f, transform.position.z);

            if (teamNumber == 1 && chosenPlayer == null && TeamManager.numofTeamOnePlayers != 0)
            {

                RandomPlayer_TeamOne();
            }

            if (teamNumber == 2 && chosenPlayer == null && TeamManager.numofTeamTwoPlayers != 0)
            {
                RandomPlayer_TeamTwo();
            }
            if (chosenPlayer != null)
            {
                //find the vector pointing from our position to the target
                _direction = (chosenPlayer.transform.position - transform.position).normalized;

                //create the rotation we need to be in to look at the target
                _lookRotation = Quaternion.LookRotation(_direction);

                //rotate us over time according to speed until we are in the required rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);

                if (TeamManager.playerOnePos.GetComponent<MechController>().health <= 0 && chosenPlayer == TeamManager.playerOnePos && TeamManager.playerOnePos != null)
                {
                    chosenPlayer = null;
                }

                if (TeamManager.playerTwoPos.GetComponent<MechController>().health <= 0 && chosenPlayer == TeamManager.playerTwoPos && TeamManager.playerTwoPos != null)
                {

                    chosenPlayer = null;
                }

                if (TeamManager.playerThreePos.GetComponent<MechController>().health <= 0 && chosenPlayer == TeamManager.playerThreePos && TeamManager.playerThreePos != null)
                {
                    chosenPlayer = null;
                }

                if (TeamManager.playerFourPos.GetComponent<MechController>().health <= 0 && chosenPlayer == TeamManager.playerFourPos && TeamManager.playerFourPos != null)
                {
                    chosenPlayer = null;
                }

                if (TeamManager.playerFivePos.GetComponent<MechController>().health <= 0 && chosenPlayer == TeamManager.playerFivePos && TeamManager.playerFivePos != null)
                {
                    chosenPlayer = null;
                }

                if (TeamManager.playerSixPos.GetComponent<MechController>().health <= 0 && chosenPlayer == TeamManager.playerSixPos && TeamManager.playerSixPos != null)
                {
                    chosenPlayer = null;
                }

                if (randNum == 1 && chosenPlayer != null)
                {
                    agent.SetDestination(chosenPlayer.transform.position);
                    anim.SetBool("Walk", true);
                }
                else
                {
                    anim.SetBool("Walk", false);
                }
             

            }

            
        
         
        }

       

        if (health > 0 && supermech_Blue && teamNumber == 1)
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 2.6f, transform.position.z);

            if(randNum == 1)
            {
                anim.SetBool("Walk", true);
                agent.SetDestination(superMech_Blue.transform.position);
            }
            else
            {
                anim.SetBool("Walk", false);
            }

            //find the vector pointing from our position to the target
            _direction = (superMech_Blue.transform.position - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            _lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);


        }

        if (health > 0 && supermech_Red && teamNumber == 2)
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 2.6f, transform.position.z);

            if (randNum == 1)
            {
                anim.SetBool("Walk", true);
                agent.SetDestination(superMech_Red.transform.position);
            }
            else
            {
                anim.SetBool("Walk", false);
            }

            //find the vector pointing from our position to the target
            _direction = (superMech_Red.transform.position - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            _lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);

            
        }



    }

    void AIBehaviour()
    {
        randNum = Random.Range(min, max);
    }

    void Shoot()
    {
        source.PlayOneShot(shoot);
        //gameObject.transform.GetChild(0).eulerAngles = new Vector3(0, 320, 0);
        for (int i = 1; i < 6;i++)
        {
            GameObject bulletInstance = (GameObject)Instantiate(aiBullet, gameObject.transform.GetChild(i).position, gameObject.transform.GetChild(i).rotation);
            
            bulletInstance.GetComponent<Rigidbody>().velocity = gameObject.transform.GetChild(i).transform.forward * 5;
            if(teamNumber == 1)
            {
                bulletInstance.GetComponent<MeshRenderer>().material.color = Color.red;
                bulletInstance.GetComponent<SmallAIBullet>().attackDamage = attackDamage;
                bulletInstance.transform.GetChild(1).gameObject.SetActive(true);
                bulletInstance.gameObject.tag = "AIBulletRed";
                DimensionManager.garbage.Add(bulletInstance);
            }
            if (teamNumber == 2)
            {
                bulletInstance.GetComponent<MeshRenderer>().material.color = Color.blue;
                bulletInstance.GetComponent<SmallAIBullet>().attackDamage = attackDamage;
                bulletInstance.transform.GetChild(0).gameObject.SetActive(true);
                bulletInstance.gameObject.tag = "AIBulletBlue";
                DimensionManager.garbage.Add(bulletInstance);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag ==  "Arena")
        {
           
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        /*if(col.gameObject.tag == "AISpawn" && teamNumber == 1)
        {
           
            if (DimensionManager.Red_AI > 3)
            {
                DimensionManager.Red_AI = 3;
            }
            
        }

        if (col.gameObject.tag == "AISpawn" && teamNumber == 2)
        {

            if (DimensionManager.Blue_AI > 3)
            {
                DimensionManager.Blue_AI = 3;
            }
        }*/

        if (col.gameObject.tag == "TeamOneBullet" && teamNumber == 1)
        {
            float damage = col.gameObject.GetComponent<Bullet>().atkDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            TeamManager.blue_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            StartCoroutine("Hit");
            
        }

        if (col.gameObject.tag == "TeamTwoBullet" && teamNumber == 2)
        {
            float damage = col.gameObject.GetComponent<Bullet>().atkDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            TeamManager.red_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));

            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            StartCoroutine("Hit");
        }

        if (col.gameObject.tag == "TeamOneStoneShard" && teamNumber == 1)
        {
            float damage = col.gameObject.GetComponent<StoneShard>().atkDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            TeamManager.blue_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            StartCoroutine("Hit");
        }

        if (col.gameObject.tag == "TeamTwoStoneShard" && teamNumber == 2)
        {
            float damage = col.gameObject.GetComponent<StoneShard>().atkDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            TeamManager.red_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            StartCoroutine("Hit");
        }

        if (col.gameObject.tag == "TeamOnePellet" && teamNumber == 1)
        {
            float damage = col.gameObject.GetComponent<Pellet>().attackDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            TeamManager.blue_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            StartCoroutine("Hit");
        }

        if (col.gameObject.tag == "TeamTwoPellet" && teamNumber == 2)
        {
            float damage = col.gameObject.GetComponent<Pellet>().attackDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            TeamManager.red_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            StartCoroutine("Hit");
        }

        if (col.gameObject.tag == "HazardOne" && teamNumber == 1)
        {
            float damage = col.gameObject.GetComponent<Boulder>().attackDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            TeamManager.blue_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            StartCoroutine("Hit");
        }

        if (col.gameObject.tag == "HazardTwo" && teamNumber == 2)
        {
            float damage = col.gameObject.GetComponent<Boulder>().attackDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            TeamManager.red_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            StartCoroutine("Hit");
        }


        if (col.gameObject.tag == "SpecialHazardOne" && teamNumber == 1)
        {
            float damage = 20;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            TeamManager.blue_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            StartCoroutine("Hit");
        }

        if (col.gameObject.tag == "SpecialHazardTwo" && teamNumber == 2)
        {
            float damage = 20;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            TeamManager.red_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            StartCoroutine("Hit");
        }

        if (col.gameObject.tag == "Boulder")
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
        }

        

        if (col.gameObject.tag == "ArmOne" && col.gameObject.transform.root.GetComponent<MechController>().isMelee && teamNumber == 1)
        {
            float damage = 10;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            TeamManager.blue_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            source.PlayOneShot(hit);
            StartCoroutine("Hit");
        }

        if (col.gameObject.tag == "ArmTwo" && col.gameObject.transform.root.GetComponent<MechController>().isMelee && teamNumber == 2)
        {
            float damage = 10;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            TeamManager.red_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            source.PlayOneShot(hit);
            StartCoroutine("Hit");
        }

        if (col.gameObject.tag == "FireWave_Blue" && teamNumber == 1)
        {
            float damage = 15;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            TeamManager.blue_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            StartCoroutine("Hit");
        }

        if (col.gameObject.tag == "FireWave_Red" && teamNumber == 2)
        {
            float damage = 15;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            TeamManager.red_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            StartCoroutine("Hit");
        }

        if (col.gameObject.tag == "TeamOneBullet" && health <= 0 && teamNumber == 1)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamOneMeter += 5;
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");

        }

        if (col.gameObject.tag == "TeamTwoBullet" && health <= 0 && teamNumber == 2)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamTwoMeter += 5;
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "TeamOneStoneShard" && health <= 0 && teamNumber == 1)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamOneMeter += 5;
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "TeamTwoStoneShard" && health <= 0 && teamNumber == 2)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamTwoMeter += 5;
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "TeamOnePellet" && health <= 0 && teamNumber == 1)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamOneMeter += 5;
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "TeamTwoPellet" && health <= 0 && teamNumber == 2)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamTwoMeter += 5;
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "ArmOne" && col.gameObject.transform.root.GetComponent<MechController>().isMelee && health <= 0 && teamNumber == 1)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamOneMeter += 5;
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "ArmTwo" && col.gameObject.transform.root.GetComponent<MechController>().isMelee && health <= 0 && teamNumber == 2)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamTwoMeter += 5;
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "HazardOne" && health <= 0 && teamNumber == 1)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            TeamManager.TeamOneMeter += 5;
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");

        }

        if (col.gameObject.tag == "HazardTwo" && health <= 0 && teamNumber == 2)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamTwoMeter += 5;
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "SpecialHazardOne" && health <= 0 && teamNumber == 1)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamOneMeter += 5;
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "SpecialHazardTwo" && health <= 0 && teamNumber == 2)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamTwoMeter += 5;
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "TA1_Bullet" && teamNumber == 1)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "TA2_Bullet" && teamNumber == 2)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "FireWave_Blue" && teamNumber == 1 && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamOneMeter += 5;
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "FireWave_Red" && teamNumber == 2 && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamTwoMeter += 5;
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(AI_explosion);
            StartCoroutine("Death");
        }


    }

    void RandomPlayer_TeamOne()
    {
        if(TeamManager.numofTeamOnePlayers == 3)
        {
            int randNum = Random.Range(0, 3);
            if(randNum == 0)
            {
                chosenPlayer = TeamManager.playerOnePos;
            }
            if (randNum == 1)
            {
                chosenPlayer = TeamManager.playerTwoPos;
            }
            if (randNum == 2)
            {
                chosenPlayer = TeamManager.playerThreePos;
            }
        }
        if (TeamManager.numofTeamOnePlayers == 2)
        {
            int randNum = Random.Range(0, 3);
            if (randNum == 0 && TeamManager.playerOnePos != null)
            {
                chosenPlayer = TeamManager.playerOnePos;
            }
            else if (randNum == 1 && TeamManager.playerTwoPos != null)
            {
                chosenPlayer = TeamManager.playerTwoPos;

            }
            else if (randNum == 2 && TeamManager.playerThreePos != null)
            {
                chosenPlayer = TeamManager.playerThreePos;

            }
            else
            {
                RandomPlayer_TeamOne();
            }
        }
        if (TeamManager.numofTeamOnePlayers == 1)
        {
            int randNum = Random.Range(0, 3);
            if (randNum == 0 && TeamManager.playerOnePos != null)
            {
                chosenPlayer = TeamManager.playerOnePos;
            }
            else if (randNum == 1 && TeamManager.playerTwoPos != null)
            {
                chosenPlayer = TeamManager.playerTwoPos;

            }
            else if (randNum == 2 && TeamManager.playerThreePos != null)
            {
                chosenPlayer = TeamManager.playerThreePos;

            }
            else
            {
                RandomPlayer_TeamOne();
            }
        }
    }

    void RandomPlayer_TeamTwo()
    {
        if (TeamManager.numofTeamTwoPlayers == 3)
        {
            int randNum = Random.Range(0, 3);
            if (randNum == 0)
            {
                chosenPlayer = TeamManager.playerFourPos;
            }
            if (randNum == 1)
            {
                chosenPlayer = TeamManager.playerFivePos;
            }
            if (randNum == 2)
            {
                chosenPlayer = TeamManager.playerSixPos;
            }
        }
        if (TeamManager.numofTeamTwoPlayers == 2)
        {
            int randNum = Random.Range(0, 3);
            if (randNum == 0 && TeamManager.playerFourPos != null)
            {
                chosenPlayer = TeamManager.playerFourPos;
            }
            else if (randNum == 1 && TeamManager.playerFivePos != null)
            {
                chosenPlayer = TeamManager.playerFivePos;

            }
            else if (randNum == 2 && TeamManager.playerSixPos != null)
            {
                chosenPlayer = TeamManager.playerSixPos;

            }
            else
            {
                RandomPlayer_TeamTwo();
            }
        }
        if (TeamManager.numofTeamTwoPlayers == 1)
        {
            int randNum = Random.Range(0, 3);
            if (randNum == 0 && TeamManager.playerFourPos != null)
            {
                chosenPlayer = TeamManager.playerFourPos;
            }
            else if (randNum == 1 && TeamManager.playerFivePos != null)
            {
                chosenPlayer = TeamManager.playerFivePos;

            }
            else if (randNum == 2 && TeamManager.playerSixPos != null)
            {
                chosenPlayer = TeamManager.playerSixPos;

            }
            else
            {
                RandomPlayer_TeamTwo();
            }
        }
        
     
    }

    /*void Melee()
    {
        Debug.Log("Melee");
    }*/
   

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

   

    void OnDestroy()
    {

        if (teamNumber == 1)
        {
            DimensionManager.Red_AI--;
        }
        if (teamNumber == 2)
        {
            DimensionManager.Blue_AI--;    
        }
        
        if (!isQuitting)
        {
            float randNum = Random.Range(0, 91);
            if (randNum >= 0 && randNum <= 30)
            {
                if(teamNumber == 1)
                {
                    GameObject health_Clone = Instantiate(health_buff, new Vector3(transform.position.x, transform.position.y + 13f, transform.position.z), transform.rotation);
                    health_Clone.gameObject.tag = "Health_Increase";
                    GarbageManager.garbage.Add(health_Clone);
                    DimensionManager.garbage.Add(health_Clone);
                }

                if (teamNumber == 2)
                {
                    GameObject health_Clone = Instantiate(health_buff, new Vector3(transform.position.x, transform.position.y + 13f, transform.position.z), transform.rotation);
                    health_Clone.gameObject.tag = "Health_Increase_2";
                    GarbageManager.garbage.Add(health_Clone);
                    DimensionManager.garbage.Add(health_Clone);
                }


            }
            if (randNum >= 31 && randNum <= 60)
            {
                if(teamNumber == 1)
                {
                    GameObject attackDamage_Clone = Instantiate(attackDamage_buff, new Vector3(transform.position.x, transform.position.y + 13f, transform.position.z), transform.rotation);
                    attackDamage_Clone.gameObject.tag = "AttackDamage_Buff";
                    GarbageManager.garbage.Add(attackDamage_Clone);
                    DimensionManager.garbage.Add(attackDamage_Clone);
                }

                if (teamNumber == 2)
                {
                    GameObject attackDamage_Clone = Instantiate(attackDamage_buff, new Vector3(transform.position.x, transform.position.y + 13f, transform.position.z), transform.rotation);
                    attackDamage_Clone.gameObject.tag = "AttackDamage_Buff_2";
                    GarbageManager.garbage.Add(attackDamage_Clone);
                    DimensionManager.garbage.Add(attackDamage_Clone);
                }
            }
            if (randNum >= 61 && randNum <= 90)
            {
                if (teamNumber == 1)
                {
                    GameObject armor_Clone = Instantiate(armor_buff, new Vector3(transform.position.x, transform.position.y + 13f, transform.position.z), transform.rotation);
                    armor_Clone.gameObject.tag = "Armor_Buff";
                    GarbageManager.garbage.Add(armor_Clone);
                    DimensionManager.garbage.Add(armor_Clone);
                }

                if (teamNumber == 2)
                {
                    GameObject armor_Clone = Instantiate(armor_buff, new Vector3(transform.position.x, transform.position.y + 13f, transform.position.z), transform.rotation);
                    armor_Clone.gameObject.tag = "Armor_Buff_2";
                    GarbageManager.garbage.Add(armor_Clone);
                    DimensionManager.garbage.Add(armor_Clone);
                }
            }
        }
    }

    IEnumerator HitNumber(int damage)
    {
        if (damage >= 10)
        {
            hitNumber.text = "-" + damage.ToString("f0");
            yield return new WaitForSeconds(0.5f);
            hitNumber.text = null;
        }
        if (damage < 10)
        {
            hitNumber_2.text = "-" + damage.ToString("f0");
            yield return new WaitForSeconds(0.5f);
            hitNumber_2.text = null;
        }
    }

    IEnumerator Hit()
    {
        mesh.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        mesh.GetComponent<Renderer>().material = material;
    }

    IEnumerator Invincible()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(5f);
        gameObject.GetComponent<BoxCollider>().enabled = true;
       
    }

    IEnumerator Death()
    {
        render.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        explosion.SetActive(true);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
