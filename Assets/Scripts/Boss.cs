using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Boss : MonoBehaviour
{
    NavMeshAgent agent;
    public List<GameObject> destinationPoints = new List<GameObject>();
    private Quaternion _lookRotation;
    private Vector3 _direction;
    Transform target;
    public float rotationSpeed;
    public List<GameObject> players = new List<GameObject>();
    public GameObject chosenPlayer;
    Rigidbody rigid;
    Quaternion rotation;
    public bool atSpot;
    Vector3 playerPos;
    public int numofShots;
    public GameObject boulder;
    public List<GameObject> boulderSpawns = new List<GameObject>();
    public List<GameObject> boulderSpawns_2 = new List<GameObject>();
    public List<ParticleSystem> particles = new List<ParticleSystem>();
    public List<ParticleSystem> particles_2 = new List<ParticleSystem>();
    public int previousNum;
    public DimensionManager dimensionManager;
    public float armor;
    public float health;
    public float attackDamage;
    public Material mat;
    public GameObject rotator;
    public List<GameObject> flameThrowers = new List<GameObject>();
    public float eruptionCount;
    public GameObject body;
    public GameObject particle;

    public Image healthBar;
    public Image red_Healthbar;
    Vector3 pos;

    public AudioClip hit;
    public AudioClip shoot;
    public AudioSource source;
    public Animator anim;

    public Text hitNumber;
    public Text hitNumber_2;


    public BoxCollider collider;
    // Start is called before the first frame update
    void Start()
    {

        source = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        rigid = GetComponent<Rigidbody>();
        GetPlayerPosition();
        StartCoroutine("Invincible");
    }

    // Update is called once per frame
    void Update()
    {
        red_Healthbar.fillAmount = health * 0.01f;
        healthBar.fillAmount = health * 0.01f;

        if (health <= 50)
        {
            healthBar.gameObject.SetActive(false);
            red_Healthbar.gameObject.SetActive(true);
        }
        else
        {
            
                healthBar.gameObject.SetActive(true);
                red_Healthbar.gameObject.SetActive(false);
            
        }


        if (gameObject != null)
        {
            if (Time.timeScale == 0)
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                Destroy(gameObject);
            }
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 6f, transform.position.z);


            if (!atSpot)
            {
                if (gameObject != null)
                {
                    //find the vector pointing from our position to the target
                    _direction = (playerPos - transform.position).normalized;

                    //create the rotation we need to be in to look at the target
                    _lookRotation = Quaternion.LookRotation(_direction);

                    //rotate us over time according to speed until we are in the required rotation
                    transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
                    if (Time.timeScale != 0)
                    {
                        if (!agent.pathPending)
                        {
                            if (agent.remainingDistance <= agent.stoppingDistance)
                            {
                                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                                {
                                    SetNextSpot();
                                }
                            }
                        }
                    }
                }
            }

            if (atSpot)
            {
                gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotation, 1f * Time.deltaTime);
            }
            rotator.transform.Rotate(2, 0, 0, Space.Self);

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        
          
            
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "BossSpot")
        {
            rotator.SetActive(false);
            atSpot = true;
            rotation = other.gameObject.transform.rotation;
            if(dimensionManager.dimension == DimensionManager.Dimension.Stone)
            {
                float timer = 0.8f;
                if (gameObject != null)
                {
                    InvokeRepeating("Attack", 0.5f, timer);
                }
                
            }

            if (dimensionManager.dimension == DimensionManager.Dimension.Lava)
            {
                float timer = 10f;
                if (gameObject != null)
                {
                    InvokeRepeating("Attack", 0.5f, timer);
                }
                source.Stop();

            }
            anim.SetBool("Walk", false);
            CancelInvoke("Attack_360");
        }

        if (other.gameObject.tag == "TeamOneBullet")
        {
            float damage = other.gameObject.GetComponent<Bullet>().atkDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            health -= (int)(damage * (100 / (100 + armor)));
            DimensionManager.total_BlueTeamDamage += damage;
            TeamManager.blue_TotalDamage += (int)hitNumber;
            damage = 0;
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            StartCoroutine("Hit");
            source.PlayOneShot(hit);
        }

        if (other.gameObject.tag == "TeamTwoBullet")
        {
            float damage = other.gameObject.GetComponent<Bullet>().atkDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            health -= (int)(damage * (100 / (100 + armor)));
            DimensionManager.total_RedTeamDamage += damage;
            TeamManager.red_TotalDamage += (int)hitNumber;
            damage = 0;
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            StartCoroutine("Hit");
            source.PlayOneShot(hit);
        }

        if (other.gameObject.tag == "TeamOneStoneShard")
        {
            float damage = other.gameObject.GetComponent<StoneShard>().atkDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            health -= (int)(damage * (100 / (100 + armor)));
            DimensionManager.total_BlueTeamDamage += damage;
            TeamManager.blue_TotalDamage += (int)hitNumber;
            damage = 0;
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            StartCoroutine("Hit");
            source.PlayOneShot(hit);
        }

        if (other.gameObject.tag == "TeamTwoStoneShard")
        {
            float damage = other.gameObject.GetComponent<StoneShard>().atkDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            health -= (int)(damage * (100 / (100 + armor)));
            DimensionManager.total_RedTeamDamage += damage;
            TeamManager.red_TotalDamage += (int)hitNumber;
            damage = 0;
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            StartCoroutine("Hit");
            source.PlayOneShot(hit);
        }

        if (other.gameObject.tag == "TeamOnePellet")
        {
            float damage = other.gameObject.GetComponent<Pellet>().attackDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            health -= (int)(damage * (100 / (100 + armor)));
            DimensionManager.total_BlueTeamDamage += damage;
            TeamManager.blue_TotalDamage += (int)hitNumber;
            damage = 0;
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            StartCoroutine("Hit");
            source.PlayOneShot(hit);
        }

        if (other.gameObject.tag == "TeamTwoPellet")
        {
            float damage = other.gameObject.GetComponent<Pellet>().attackDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            health -= (int)(damage * (100 / (100 + armor)));
            DimensionManager.total_RedTeamDamage += damage;
            TeamManager.red_TotalDamage += (int)hitNumber;
            damage = 0;
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            StartCoroutine("Hit");
            source.PlayOneShot(hit);
        }

        if (other.gameObject.tag == "HazardOne")
        {
            float damage = other.gameObject.GetComponent<Boulder>().attackDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            health -= (int)(damage * (100 / (100 + armor)));
            DimensionManager.total_BlueTeamDamage += damage;
            TeamManager.blue_TotalDamage += (int)hitNumber;
            damage = 0;
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            StartCoroutine("Hit");
            source.PlayOneShot(hit);
        }

        if (other.gameObject.tag == "HazardTwo")
        {
            float damage = other.gameObject.GetComponent<Boulder>().attackDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            health -= (int)(damage * (100 / (100 + armor)));
            DimensionManager.total_RedTeamDamage += damage;
            TeamManager.red_TotalDamage += (int)hitNumber;
            damage = 0;
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            StartCoroutine("Hit");
            source.PlayOneShot(hit);
        }


        if (other.gameObject.tag == "SpecialHazardOne")
        {
            float damage = 25;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            health -= (int)(damage * (100 / (100 + armor)));
            DimensionManager.total_BlueTeamDamage += damage;
            TeamManager.blue_TotalDamage += (int)hitNumber;
            damage = 0;
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            StartCoroutine("Hit");
            source.PlayOneShot(hit);
        }

        if (other.gameObject.tag == "SpecialHazardTwo")
        {
            float damage = 25;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            health -= (int)(damage * (100 / (100 + armor)));
            DimensionManager.total_RedTeamDamage += damage;
            TeamManager.red_TotalDamage += (int)hitNumber;
            damage = 0;
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            StartCoroutine("Hit");
            source.PlayOneShot(hit);
        }


        if (other.gameObject.tag == "ArmOne" && other.gameObject.transform.root.GetComponent<MechController>().isMelee)
        {
            float damage = other.gameObject.transform.root.GetComponent<MechController>().current_attackDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            DimensionManager.total_BlueTeamDamage += damage;
            TeamManager.blue_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            StartCoroutine("Hit");
            source.PlayOneShot(hit);
        }

        if (other.gameObject.tag == "ArmTwo" && other.gameObject.transform.root.GetComponent<MechController>().isMelee)
        {
            float damage = other.gameObject.transform.root.GetComponent<MechController>().current_attackDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            DimensionManager.total_RedTeamDamage += damage;
            TeamManager.red_TotalDamage += (int)hitNumber;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            StartCoroutine("Hit");
            source.PlayOneShot(hit);
        }

        if (other.gameObject.tag == "FireWave_Blue")
        {
            float damage = other.gameObject.GetComponent<FireWaveCollision>().atkDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            health -= (int)(damage * (100 / (100 + armor)));
            DimensionManager.total_BlueTeamDamage += damage;
            TeamManager.blue_TotalDamage += (int)hitNumber;
            damage = 0;
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            StartCoroutine("Hit");
            source.PlayOneShot(hit);
        }

        if (other.gameObject.tag == "FireWave_Red")
        {
            float damage = other.gameObject.GetComponent<FireWaveCollision>().atkDamage;
            float hitNumber = (damage * (100 / (100 + armor)));
            StartCoroutine("HitNumber", hitNumber);
            health -= (int)(damage * (100 / (100 + armor)));
            DimensionManager.total_RedTeamDamage += damage;
            TeamManager.red_TotalDamage += (int)hitNumber;
            damage = 0;
            pos = other.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(other.gameObject);
            StartCoroutine("Hit");
            source.PlayOneShot(hit);
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "BossSpot")
        {
            atSpot = false;
            if (dimensionManager.dimension == DimensionManager.Dimension.Stone)
            {
                InvokeRepeating("Attack_360", 0.5f, 0.8f);
                CancelInvoke("Attack");
               
            }
            if (dimensionManager.dimension == DimensionManager.Dimension.Lava)
            {
                InvokeRepeating("Attack_360", 0.5f, 0.8f);
                CancelInvoke("Attack");
                source.Stop();
            }
            anim.SetBool("Walk", true);
        }


    }

    void Attack()
    {
        if (dimensionManager.dimension == DimensionManager.Dimension.Stone)
        {
            if (numofShots < 20)
            {
                numofShots++;
                int randNum = Random.Range(0, 5);
                int specialNum = Random.Range(0, 100);
                if (randNum == previousNum)
                {
                    Attack();
                }
                else
                {
                    GameObject boulderInstance = (GameObject)Instantiate(boulder, boulderSpawns[randNum].transform.position, boulderSpawns[randNum].transform.rotation);
                    particles[randNum].Emit(3);
                    previousNum = randNum;
                    if (randNum > 2)
                    {
                        boulderInstance.GetComponent<Boulder>().left = false;
                        if (specialNum >= 0 && specialNum <= 5)
                        {
                            boulderInstance.gameObject.tag = "SpecialHazard";
                            boulderInstance.gameObject.transform.GetChild(0).gameObject.SetActive(true);

                        }
                    }
                    else if (randNum <= 2)
                    {
                        boulderInstance.GetComponent<Boulder>().left = true;
                        if (specialNum >= 0 && specialNum <= 5)
                        {
                            boulderInstance.gameObject.tag = "SpecialHazard";
                            boulderInstance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        }
                    }
                    DimensionManager.garbage.Add(boulderInstance);
                }
            }
            else
            {

                numofShots = 0;
                int randNum = Random.Range(0, 101);
                if (randNum >= 0 && randNum <= 50)
                {
                    previousNum = 13;
                    SetNextSpot();
                }
                if (randNum >= 51 && randNum <= 100)
                {
                    previousNum = 13;
                    GetPlayerPosition();
                }
            }
        }



        if (dimensionManager.dimension == DimensionManager.Dimension.Lava)
        {
            if(gameObject.activeSelf)
            {
                source.Play();
            }
           
            if (eruptionCount < 3)
            {
                for (int i = 0; i < 8; i++)
                {
                    flameThrowers[i].SetActive(true);
                }

                eruptionCount++;
                for (int i = 0; i < 4; i++)
                {
                    int randNum = Random.Range(0, 101);
                    if (randNum >= 0 && randNum <= 50)
                    {
                        flameThrowers[i].GetComponent<Rotator>().speed *= 1;
                    }
                    if (randNum >= 51 && randNum <= 100)
                    {
                        flameThrowers[i].GetComponent<Rotator>().speed *= -1;
                    }

                }
            }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                flameThrowers[i].SetActive(false);
            }
            eruptionCount = 0;
            int randNum = Random.Range(0, 101);
            if (randNum >= 0 && randNum <= 50)
            {
                previousNum = 13;
                SetNextSpot();
            }
            if (randNum >= 51 && randNum <= 100)
            {
                previousNum = 13;
                GetPlayerPosition();
            }
        }
        }
    }

    void GetPlayerPosition()
    {
        if (gameObject.activeSelf)
        {
            int ranNum = Random.Range(0, 6);
            if (dimensionManager.superMech_Blue.activeSelf && dimensionManager.superMech_Red.activeSelf)
            {
                int random = Random.Range(0, 2);
                if (random == 0)
                {
                    playerPos = dimensionManager.superMech_Blue.transform.position;
                    agent.SetDestination(playerPos);
                    agent.speed = 15f;
                }
                if (random == 1)
                {
                    playerPos = dimensionManager.superMech_Red.transform.position;
                    agent.SetDestination(playerPos);
                    agent.speed = 15f;
                }

            }
            else if (dimensionManager.superMech_Blue.activeSelf && !dimensionManager.superMech_Red.activeSelf)
            {
                int random = Random.Range(3, 7);
                if (random == 6)
                {
                    playerPos = dimensionManager.superMech_Blue.transform.position;
                    agent.SetDestination(playerPos);
                    agent.speed = 15f;
                }
                else
                {
                    playerPos = players[random].transform.position;
                    agent.SetDestination(playerPos);
                    agent.speed = 15f;
                }
            }
            else if (!dimensionManager.superMech_Blue.activeSelf && dimensionManager.superMech_Red.activeSelf)
            {
                int random = Random.Range(0, 4);
                if (random == 3)
                {
                    playerPos = dimensionManager.superMech_Red.transform.position;
                    agent.SetDestination(playerPos);
                    agent.speed = 15f;
                }
                else
                {
                    playerPos = players[random].transform.position;
                    agent.SetDestination(playerPos);
                    agent.speed = 15f;
                }
            }
            else
            {
                playerPos = players[ranNum].transform.position;
                agent.SetDestination(playerPos);
                agent.speed = 15f;
            }
            anim.SetBool("Walk", true);
        }
    }

    void SetNextSpot()
    {
        int randNum = Random.Range(0, 4);
        if(gameObject.activeSelf)
        agent.SetDestination(destinationPoints[randNum].transform.position);
        //anim.SetBool("Walk", true);
    }

    void Attack_360()
    {
        if (dimensionManager.dimension == DimensionManager.Dimension.Stone)
        {

            rotator.SetActive(false);
            int randNum = Random.Range(0, 10);
            int specialNum = Random.Range(0, 100);
            if (randNum == previousNum)
            {
                Attack();
            }
            else
            {
                GameObject boulderInstance = (GameObject)Instantiate(boulder, boulderSpawns_2[randNum].transform.position, boulderSpawns_2[randNum].transform.rotation);
                particles_2[randNum].Emit(3);
                previousNum = randNum;
                source.PlayOneShot(shoot);
                if (randNum > 2)
                {
                    boulderInstance.GetComponent<Boulder>().left = false;
                    if (specialNum >= 0 && specialNum <= 5)
                    {
                        boulderInstance.gameObject.tag = "SpecialHazard";
                        boulderInstance.gameObject.transform.GetChild(0).gameObject.SetActive(true);

                    }
                }
                else if (randNum <= 2)
                {
                    boulderInstance.GetComponent<Boulder>().left = true;
                    if (specialNum >= 0 && specialNum <= 5)
                    {
                        boulderInstance.gameObject.tag = "SpecialHazard";
                        boulderInstance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
                DimensionManager.garbage.Add(boulderInstance);
            }
        }
        if(dimensionManager.dimension == DimensionManager.Dimension.Lava)
        {
            rotator.SetActive(true);
            source.Play();
        }
    }

    /*IEnumerator Music()
    {
        source.Stop();
        yield return new WaitForSeconds(0.5f);
        source.Play();
    }*/

    IEnumerator Invincible()
    {
        collider.enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        collider.enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator Hit()
    {
        body.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        body.GetComponent<SkinnedMeshRenderer>().material = mat;
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
}
