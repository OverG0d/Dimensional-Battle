using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllForOneMachine : MonoBehaviour
{

    public enum Team {TeamOne, TeamTwo}
    public Team team;
    public float health;
    public float armor;

    public Image health_Bar;
    public Image health_BarRed;
    public GameObject mech_UpperBody;
    public Material material;

    public float fireDamage;

    public bool onFire;
    public bool invincible;
    public GameObject particle;
    public GameObject superMech_Particle;
    Vector3 pos;
    public AudioClip clip;
    public AudioSource source;

    Rigidbody rigid;

    public Text hitNumber;
    public Text hitNumber_2;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine("Invincible");
    }

    // Update is called once per frame
    void Update()
    {


        if(health <= 100)
        {
            health_Bar.gameObject.SetActive(false);
            health_BarRed.gameObject.SetActive(true);
        }
        else
        {
            health_Bar.gameObject.SetActive(true);
            health_BarRed.gameObject.SetActive(false);
        }

        health_Bar.fillAmount = health * 0.01f;
        health_BarRed.fillAmount = health * 0.01f;
        if(onFire)
        {
            health -= Time.deltaTime * (int)(fireDamage * (100 / (100 + armor)));
        }


    }



    void OnTriggerEnter(Collider col)
    {

        if (!invincible)
        {

            if (col.gameObject.tag == "Boss")
            {
                if (col.gameObject.tag == "Boss")
                {
                    float damage = 50;
                    float hitNumber = (damage * (100 / (100 + armor)));
                    health -= (int)(damage * (100 / (100 + armor)));
                    StartCoroutine("HitNumber", hitNumber);
                    damage = 0;
                    pos = col.gameObject.transform.position;
                    GameObject particle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                    rigid.AddForce(col.gameObject.transform.forward * 200, ForceMode.Impulse);
                    Destroy(particle_Clone, 1f);
                    StartCoroutine("Hit");
                }
            }

            if (col.gameObject.tag == "TeamOneBullet" && team == Team.TeamTwo)
            {
                float damage = col.gameObject.GetComponent<Bullet>().atkDamage;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                TeamManager.blue_TotalDamage += (int)hitNumber;
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }

            if (col.gameObject.tag == "TeamTwoBullet" && team == Team.TeamOne)
            {
                float damage = col.gameObject.GetComponent<Bullet>().atkDamage;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                TeamManager.red_TotalDamage += (int)hitNumber;
                StartCoroutine("HitNumber", hitNumber);
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }

            if (col.gameObject.tag == "TeamOneStoneShard" && team == Team.TeamTwo)
            {
                float damage = col.gameObject.GetComponent<StoneShard>().atkDamage;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                TeamManager.blue_TotalDamage += (int)hitNumber;
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }

            if (col.gameObject.tag == "TeamTwoStoneShard" && team == Team.TeamOne)
            {
                float damage = col.gameObject.GetComponent<StoneShard>().atkDamage;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                TeamManager.red_TotalDamage += (int)hitNumber;
                StartCoroutine("HitNumber", hitNumber);
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }

            if (col.gameObject.tag == "TeamOnePellet" && team == Team.TeamTwo)
            {
                float damage = col.gameObject.GetComponent<Pellet>().attackDamage;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                TeamManager.blue_TotalDamage += (int)hitNumber;
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }

            if (col.gameObject.tag == "TeamTwoPellet" && team == Team.TeamOne)
            {
                float damage = col.gameObject.GetComponent<Pellet>().attackDamage;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                TeamManager.red_TotalDamage += (int)hitNumber;
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);


                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }

            if (col.gameObject.tag == "Boulder")
            {
                float damage = col.gameObject.GetComponent<Boulder>().attackDamage;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);

                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }



            if (col.gameObject.tag == "AIBulletRed" && team == Team.TeamOne)
            {
                float damage = 10;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }


            if (col.gameObject.tag == "AIBulletBlue" && team == Team.TeamTwo)
            {
                float damage = 10;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                TeamManager.blue_TotalDamage += (int)hitNumber;
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);

            }

            if (col.gameObject.tag == "TA2_Bullet" && team == Team.TeamOne)
            {
                float damage = col.gameObject.GetComponent<Bullet>().atkDamage;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                TeamManager.red_TotalDamage += (int)hitNumber;
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }

            if (col.gameObject.tag == "TA1_Bullet" && team == Team.TeamTwo)
            {
                float damage = col.gameObject.GetComponent<Bullet>().atkDamage;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                TeamManager.blue_TotalDamage += (int)hitNumber;
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }


            if (col.gameObject.tag == "ArmOne" && col.gameObject.transform.root.GetComponent<MechController>().isMelee && team == Team.TeamTwo)
            {
                float damage = col.gameObject.transform.root.GetComponent<MechController>().current_attackDamage + 5;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                TeamManager.blue_TotalDamage += (int)hitNumber;
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                rigid.AddForce(col.gameObject.transform.forward * 100, ForceMode.Impulse);
                Destroy(particleicle_Clone, 1f);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }



            if (col.gameObject.tag == "ArmTwo" && col.gameObject.transform.root.GetComponent<MechController>().isMelee && team == Team.TeamOne)
            {
                float damage = col.gameObject.transform.root.GetComponent<MechController>().current_attackDamage + 5;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                TeamManager.red_TotalDamage += (int)hitNumber;
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                rigid.AddForce(col.gameObject.transform.forward * 100, ForceMode.Impulse);
                Destroy(particleicle_Clone, 1f);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }



            if (col.gameObject.tag == "SpecialHazardTwo" && team == Team.TeamOne)
            {
                float damage = 35;
                float hitNumber = (damage * (100 / (100 + armor)));
                TeamManager.TeamTwoMeter += 15;
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                TeamManager.red_TotalDamage += (int)hitNumber;
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }

            if (col.gameObject.tag == "SpecialHazardOne" && team == Team.TeamTwo)
            {
                float damage = 35;
                float hitNumber = (damage * (100 / (100 + armor)));
                TeamManager.TeamOneMeter += 15;
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                TeamManager.blue_TotalDamage += (int)hitNumber;
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }


            if (col.gameObject.tag == "SpecialHazard")
            {

                float damage = 25;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }

            if (col.gameObject.tag == "FireWave_Red" && team == Team.TeamOne)
            {
                float damage = 20;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                TeamManager.red_TotalDamage += (int)hitNumber;
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }

            if (col.gameObject.tag == "FireWave_Blue" && team == Team.TeamTwo)
            {
                float damage = 20;
                float hitNumber = (damage * (100 / (100 + armor)));
                health -= (int)(damage * (100 / (100 + armor)));
                StartCoroutine("HitNumber", hitNumber);
                TeamManager.blue_TotalDamage += (int)hitNumber;
                damage = 0;
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                StartCoroutine("Hit");
                source.PlayOneShot(clip);

            }

            if (col.gameObject.tag == "Untagged")
            {
                pos = gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                onFire = true;
                fireDamage = 10;
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }

            if (col.gameObject.tag == "SpecialFlame")
            {
                pos = gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                onFire = true;
                fireDamage = 15;
                StartCoroutine("Hit");
                source.PlayOneShot(clip);
            }

            if (col.gameObject.tag == "SpecialHazardOne" && team == Team.TeamOne)
            {
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                source.PlayOneShot(clip);
            }
            if (col.gameObject.tag == "SpecialHazardTwo" && team == Team.TeamTwo)
            {
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                source.PlayOneShot(clip);
            }

            if (col.gameObject.tag == "HazardOne" && team == Team.TeamOne)
            {
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                source.PlayOneShot(clip);
            }

            if (col.gameObject.tag == "HazardTwo" && team == Team.TeamTwo)
            {
                pos = col.gameObject.transform.position;
                GameObject particleicle_Clone = (GameObject)Instantiate(particle, pos, particle.transform.rotation);
                Destroy(particleicle_Clone, 1f);
                Destroy(col.gameObject);
                source.PlayOneShot(clip);

            }
        }

        


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Untagged")
        {
            onFire = false;
        }

        if (other.gameObject.tag == "SpecialFlame")
        {
            onFire = false;
        }
    }
    IEnumerator Hit()
    {
        mech_UpperBody.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        mech_UpperBody.GetComponent<MeshRenderer>().material = material;
    }

    IEnumerator Invincible()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(3f);
        gameObject.GetComponent<BoxCollider>().enabled = true;
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
