using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MechController : MonoBehaviour
{
    public float heading;

    public int playerId;

    public float moveSpeed;

    public float bulletSpeed;

    public bool active_One;                 //Is this timer active?
    public float timer_One;                 //Time left on timer, can be used at 

    public bool active_Two;                 //Is this timer active?
    public float timer_Two;                 //Time left on timer, can be used at 

    
    public bool has_Powerup;


    public int playerNumber;
    public GameObject teamManager;

    public GameObject DimensionManager;

    public enum BuffType {Health, AttackDamage, Armor, None };
    public BuffType bufftype;
    public enum Team { TeamOne, TeamTwo };
    public int teamNumber;
    public Team team;
    public GameObject bulletSpawn;
    public GameObject bulletPrefab;
    public float health;
    public float armor;
    public float currentArmor;
    public float attackDamage;
    public float current_attackDamage;
    private Player player;
    private Vector3 moveVector;
    private Vector3 rotateVector;
    public float timeStamp;
    public float seconds;
    public bool isMelee;
    //private bool fire;
    Rigidbody rigid;
    DimensionManager dimensionManager;
    public bool inRange;

    //All For One Machine for three players
    public bool isDriver;
    public bool isGunnerOne;
    public bool isGunnerTwo;
    public bool isCannon;
    public float Ysensitivity;
    private float rotationY = 0f;
    public bool voted;
    public int start;
    public GameObject arrayShot;

    //UI Functionality
    //public GameObject canvas;
    public Image abilityOne_Stone;
    public Image abilityTwo_Stone;
    public Image abilityOne_Lava;
    public Image abilityTwo_Lava;
    public Image healthBar;
    public Image TeamAbility;
    public Image backGround;
    public Image red_Healthbar;
    Quaternion rotation;

    public bool invinsibleMode;

    public GameObject arm;
    public Animator animator;
    public bool ready;
    public bool tutorial;
    public SkinnedMeshRenderer mech_UpperBody;
    public Material material;
    public GameObject explosion;
    public bool left_Rotation;
    public GameObject fireWave;
    float fire;
    public float coolDownTimerOne;
    public float coolDownTimerTwo;

    public bool onFire;
    public float fireDamage;

    public Image armor_Powerup;
    public Image health_Powerup;
    public Image attackDamage_Powerup;
    public GameObject particle_Hit;
    Vector3 pos;
    public bool lava_PushRed;
    public bool lava_PushBlue;

    public float totalDamage;
    public Text attack_Damage_Text;
    public Text armor_Text;


    //Audio Clips
    public AudioClip supermech_Blast;
    public AudioClip player_Explosion;
    public AudioClip powerupGrab;
    public AudioClip playerAbility;
    public AudioClip hit;
    public AudioClip running;
    public AudioClip shooting;
    public AudioSource source;

    public TutorialManager tutorialManager;

    public bool paused;
    public bool startingGame;

    public GameObject blue_SuperMech;
    public GameObject red_SuperMech;
    public Text hitNumber;
    public Text hitNumber_2;

    public bool GameOver;

    public GameObject pauseMenu;
    void Awake()
    {

        
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime

        player = ReInput.players.GetPlayer(playerId);

        // Get the rigidbody of this controller
        rigid = GetComponent<Rigidbody>();

        source = GetComponent<AudioSource>();

        //Get Dimension Manager script.
        dimensionManager = DimensionManager.GetComponent<DimensionManager>();

        timer_One = 0;
        timer_Two = 0;

        rotation = transform.rotation;

        Time.timeScale = 1;

    }

    // Start is called before the first frame update
    void Start()
    {
        if (teamManager.GetComponent<TeamManager>().teamOne.Contains(gameObject))
        {
            teamNumber = 1;
            team = Team.TeamOne;
        }
        if (teamManager.GetComponent<TeamManager>().teamTwo.Contains(gameObject))
        {
            teamNumber = 2;
            team = Team.TeamTwo;
        }

        totalDamage = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if(TeamManager.numofTeamOnePlayers == 0 || TeamManager.numofTeamTwoPlayers == 0)
        {
            GameOver = true;
        }
        Cursor.visible = false;

        if (!tutorial)
        {
            attack_Damage_Text.text = "Attack Damage:" + current_attackDamage;

            armor_Text.text = "Armor:" + currentArmor;
        }

        if (dimensionManager.dimension == global::DimensionManager.Dimension.Stone && !tutorial)
        {
            abilityOne_Stone.gameObject.SetActive(true);
            abilityTwo_Stone.gameObject.SetActive(true);
            abilityOne_Lava.gameObject.SetActive(false);
            abilityTwo_Lava.gameObject.SetActive(false);
        }

        if (dimensionManager.dimension == global::DimensionManager.Dimension.Lava && !tutorial)
        {
            abilityOne_Stone.gameObject.SetActive(false);
            abilityTwo_Stone.gameObject.SetActive(false);
            abilityOne_Lava.gameObject.SetActive(true);
            abilityTwo_Lava.gameObject.SetActive(true);
        }

        if (health <= 50 && !tutorial)
        {
            healthBar.gameObject.SetActive(false);
            red_Healthbar.gameObject.SetActive(true);
        }
        else
        {
            if (!tutorial)
            {
                healthBar.gameObject.SetActive(true);
                red_Healthbar.gameObject.SetActive(false);
            }
        }


        if (!global::DimensionManager.switchingDimension)
        {
            if (player != null && !isDriver && !isGunnerOne && !isGunnerTwo && !isCannon && (team == Team.TeamOne || team == Team.TeamTwo) && health > 0 && !GameOver)
            {
                GetInput();
                ProcessInput();

            }

            if (player != null && isDriver && team == Team.TeamOne && dimensionManager.GetComponent<DimensionManager>().transition_Timer >= 10 && !GameOver)
            {
                GetInput();
                ProcessDriverInput();
            }

            if (player != null && isGunnerOne && team == Team.TeamOne && dimensionManager.GetComponent<DimensionManager>().transition_Timer >= 10 && !GameOver)
            {
                GetInput();
                ProcessGunnerOneInput();
            }

            if (player != null && isGunnerTwo && team == Team.TeamOne && dimensionManager.GetComponent<DimensionManager>().transition_Timer >= 10 && !GameOver) 
            {
                GetInput();
                ProcessGunnerTwoInput();
            }

            if (player != null && isCannon && team == Team.TeamOne && dimensionManager.GetComponent<DimensionManager>().transition_Timer >= 10 && !GameOver)
            {
                GetInput();
                ProcessCannonInput();
            }

            if (player != null && isDriver && team == Team.TeamTwo && dimensionManager.GetComponent<DimensionManager>().transition_Timer >= 10 && !GameOver)
            {
                GetInput();
                ProcessDriverInput_2();
            }

            if (player != null && isGunnerOne && team == Team.TeamTwo && dimensionManager.GetComponent<DimensionManager>().transition_Timer >= 10 && !GameOver)
            {
                GetInput();
                ProcessGunnerOneInput_2();
            }

            if (player != null && isGunnerTwo && team == Team.TeamTwo && dimensionManager.GetComponent<DimensionManager>().transition_Timer >= 10 && !GameOver)
            {
                GetInput();
                ProcessGunnerTwoInput_2();
            }

            if (player != null && isCannon && team == Team.TeamTwo && dimensionManager.GetComponent<DimensionManager>().transition_Timer >= 10 && !GameOver)
            {
                GetInput();
                ProcessCannonInput_2();
            }
        }



        if (health >= 200)
        {
            health = 200;
        }

        // If health reaches 0, player dies
        /*if (health <= 0)
        {
            if (team == Team.TeamOne)
            {
                TeamManager.numofTeamOnePlayers--;
                teamManager.GetComponent<TeamManager>().teamOne.Remove(gameObject);

            }
            if (team == Team.TeamTwo)
            {
                TeamManager.numofTeamTwoPlayers--;
                teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            }
            
            
        }*/


        if (!tutorial)
        {

            if (timer_One > 0)
            {
                timer_One -= Time.deltaTime;    //Subtract the time since last frame from the timer.
                if (dimensionManager.dimension == global::DimensionManager.Dimension.Stone)
                {
                    abilityOne_Stone.fillAmount += 1.0f / coolDownTimerOne * Time.deltaTime;
                }

                if (dimensionManager.dimension == global::DimensionManager.Dimension.Lava)
                {
                    abilityOne_Lava.fillAmount += 1.0f / coolDownTimerOne * Time.deltaTime;
                }
            }

            if (timer_One <= 0)
            {
                timer_One = 0;
                if (dimensionManager.dimension == global::DimensionManager.Dimension.Stone)
                {
                    abilityOne_Stone.fillAmount = 1;
                }

                if (dimensionManager.dimension == global::DimensionManager.Dimension.Lava)
                {
                    abilityOne_Lava.fillAmount = 1;
                }


            }

            if (timer_Two > 0)
            {
                timer_Two -= Time.deltaTime;    //Subtract the time since last frame from the timer.


               

                if (dimensionManager.dimension == global::DimensionManager.Dimension.Stone)
                {
                    abilityTwo_Stone.fillAmount += 1.0f / coolDownTimerTwo * Time.deltaTime;
                    if (timer_Two <= 10)
                    {
                        if (armor > 0)
                        {
                            currentArmor -= armor;
                            armor = 0;
                        }
                    }
                }

                if (dimensionManager.dimension == global::DimensionManager.Dimension.Lava)
                {
                    abilityTwo_Lava.fillAmount += 1.0f / coolDownTimerTwo * Time.deltaTime;
                    if (timer_Two <= 10)
                    {
                        if (attackDamage > 0)
                        {
                            current_attackDamage -= attackDamage;
                            attackDamage = 0;
                        }
                    }
                   
                }

            }

            if (timer_Two <= 0)
            {
                timer_Two = 0;
                if (dimensionManager.dimension == global::DimensionManager.Dimension.Stone)
                {
                    abilityTwo_Stone.fillAmount = 1;
                   
                }

                if (dimensionManager.dimension == global::DimensionManager.Dimension.Lava)
                {
                    abilityTwo_Lava.fillAmount = 1;
                  
                }




            }

            //UI 
            switch (bufftype)
            {
                case BuffType.AttackDamage:
                    attackDamage_Powerup.gameObject.SetActive(true);
                    break;
                case BuffType.Armor:
                    armor_Powerup.gameObject.SetActive(true);
                    break;
                case BuffType.Health:
                    health_Powerup.gameObject.SetActive(true);
                    break;
                case BuffType.None:
                    attackDamage_Powerup.gameObject.SetActive(false);
                    armor_Powerup.gameObject.SetActive(false);
                    health_Powerup.gameObject.SetActive(false);
                    break;
            }
            red_Healthbar.fillAmount = health * 0.01f;
            healthBar.fillAmount = health * 0.01f;
            if (team == Team.TeamOne)
            {
                TeamAbility.fillAmount = TeamManager.TeamOneMeter * 0.01f;
            }

            if (team == Team.TeamTwo)
            {
                TeamAbility.fillAmount = TeamManager.TeamTwoMeter * 0.01f;
            }

        }
        fire = player.GetAxis("Fire");
        animator.SetFloat("Fire", fire);
        

        if(onFire)
        {
            float hitNumber = Time.deltaTime * (int)(fireDamage * (100 / (100 + currentArmor)));
            health -= Time.deltaTime * (int)(fireDamage * (100 / (100 + currentArmor)));
            animator.SetTrigger("Hit");
        }

       
           
    }


    public void GetInput()
    {
        // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
        // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

        if (player != null)
        {
            moveVector.x = player.GetAxis("Move Horizontal"); // get input by name or action id
            moveVector.y = player.GetAxis("Move Vertical");
            rotateVector.x = player.GetAxis("Rotate Horizontal");
            rotateVector.y = player.GetAxis("Rotate Vertical");
            
        }
        
    }

    public void ProcessInput()
    {
        
        if (player != null)
        {
            // Process movement
            if (moveVector.x != 0.0f || moveVector.y != 0.0f)
            {
                if (!isMelee)
                {
                    rigid.velocity = new Vector3(moveVector.x * moveSpeed, 0, moveVector.y * moveSpeed);
                }
                animator.SetBool("Running",true);
                if(!source.isPlaying)
                {
                    source.PlayOneShot(running);
                }
               
                
            }
            else
            {
                animator.SetBool("Running", false);
            }

            
                // Process fire
                if (player.GetAxis("Fire") > 0 && timeStamp < Time.time && team == Team.TeamOne)
            {
                timeStamp = Time.time + seconds;
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.transform.position, transform.rotation);
                bullet.transform.GetChild(0).gameObject.SetActive(true);
                bullet.GetComponent<Bullet>().atkDamage = current_attackDamage;
                bullet.tag = "TeamOneBullet";
                bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
                global::DimensionManager.garbage.Add(bullet);
                source.PlayOneShot(shooting);
            }

            // Process fire
            if (player.GetAxis("Fire") > 0 && timeStamp < Time.time && team == Team.TeamTwo)
            {
                timeStamp = Time.time + seconds;
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.transform.position, transform.rotation);
                bullet.transform.GetChild(1).gameObject.SetActive(true);
                bullet.GetComponent<Bullet>().atkDamage = current_attackDamage;
                bullet.tag = "TeamTwoBullet";
                bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
                global::DimensionManager.garbage.Add(bullet);
                source.PlayOneShot(shooting);
            }


            if(player.GetAxis("Melee") > 0 && !isMelee)
            {
                animator.SetTrigger("Melee");
                rigid.AddForce(transform.forward * 120, ForceMode.Impulse);
                arm.SetActive(true);
                isMelee = true;
                StartCoroutine("Melee");
            }


            // Player Rotation
          

            if ((Mathf.Abs(rotateVector.x) >= 0.1f && Mathf.Abs(rotateVector.y) >= 0.1f) || (Mathf.Abs(moveVector.x) <= -0.1f && Mathf.Abs(moveVector.y) <= -0.1f))
            {
                heading = Mathf.Atan2(rotateVector.x, rotateVector.y);
                transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0f);
                //Debug.Log(transform.eulerAngles.y);
            }
           
        


            // Transfer Power-Up to either player on your team

            if (player.GetButtonDown("Power-UpLeft"))
            {

                //Team One player One
                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[1] != null && playerNumber == 1
                 && !teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Health)
                { 
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().bufftype = BuffType.Health;
                    teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[1] != null && playerNumber == 1
                 && !teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().has_Powerup && bufftype == BuffType.AttackDamage)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().bufftype = BuffType.AttackDamage;
                    teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[1] != null && playerNumber == 1
                 && !teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Armor)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().bufftype = BuffType.Armor;
                    teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                //Team One player Two

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[0] != null && playerNumber == 2
                 && !teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Health)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().bufftype = BuffType.Health;
                    teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[0] != null && playerNumber == 2
                 && !teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup && bufftype == BuffType.AttackDamage)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().bufftype = BuffType.AttackDamage;
                    teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[0] != null && playerNumber == 2
                 && !teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Armor)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().bufftype = BuffType.Armor;
                    teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                //Team One player Three

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[0] != null && playerNumber == 3
                 && !teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Health)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().bufftype = BuffType.Health;
                    teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[0] != null && playerNumber == 3
                 && !teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup && bufftype == BuffType.AttackDamage)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().bufftype = BuffType.AttackDamage;
                    teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[0] != null && playerNumber == 3
                 && !teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Armor)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().bufftype = BuffType.Armor;
                    teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                //Team Two player One
                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[1] != null && playerNumber == 1
                 && !teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Health)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().bufftype = BuffType.Health;
                    teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[1] != null && playerNumber == 1
                 && !teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().has_Powerup && bufftype == BuffType.AttackDamage)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().bufftype = BuffType.AttackDamage;
                    teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[1] != null && playerNumber == 1
                 && !teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Armor)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().bufftype = BuffType.Armor;
                    teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                //Team Two player Two

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[0] != null && playerNumber == 2
                 && !teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Health)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().bufftype = BuffType.Health;
                    teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[0] != null && playerNumber == 2
                 && !teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().has_Powerup && bufftype == BuffType.AttackDamage)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().bufftype = BuffType.AttackDamage;
                    teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[0] != null && playerNumber == 2
                 && !teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Armor)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().bufftype = BuffType.Armor;
                    teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                //Team Two player Three

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[0] != null && playerNumber == 3
                 && !teamManager.GetComponent<TeamManager>().teamOne[0].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Health)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().bufftype = BuffType.Health;
                    teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[0] != null && playerNumber == 3
                 && !teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().has_Powerup && bufftype == BuffType.AttackDamage)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().bufftype = BuffType.AttackDamage;
                    teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[0] != null && playerNumber == 3
                 && !teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Armor)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().bufftype = BuffType.Armor;
                    teamManager.GetComponent<TeamManager>().teamTwo[0].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }




            }

            if (player.GetButtonDown("Power-UpRight"))
            {
                //Team One player One
                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[2] != null && playerNumber == 1
                 && !teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Health)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().bufftype = BuffType.Health;
                    teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[2] != null && playerNumber == 1
                 && !teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().has_Powerup && bufftype == BuffType.AttackDamage)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().bufftype = BuffType.AttackDamage;
                    teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[2] != null && playerNumber == 1
                 && !teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Armor)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().bufftype = BuffType.Armor;
                    teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                //Team One player Two

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[2] != null && playerNumber == 2
                 && !teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Health)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().bufftype = BuffType.Health;
                    teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[2] != null && playerNumber == 2
                 && !teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().has_Powerup && bufftype == BuffType.AttackDamage)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().bufftype = BuffType.AttackDamage;
                    teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[2] != null && playerNumber == 2
                 && !teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Armor)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().bufftype = BuffType.Armor;
                    teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                //Team One player Three

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[1] != null && playerNumber == 3
                 && !teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Health)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().bufftype = BuffType.Health;
                    teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[1] != null && playerNumber == 3
                 && !teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().has_Powerup && bufftype == BuffType.AttackDamage)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().bufftype = BuffType.AttackDamage;
                    teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamOne && teamManager.GetComponent<TeamManager>().teamOne[1] != null && playerNumber == 3
                 && !teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Armor)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().bufftype = BuffType.Armor;
                    teamManager.GetComponent<TeamManager>().teamOne[1].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                //Team Two player One
                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[2] != null && playerNumber == 1
                 && !teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Health)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().bufftype = BuffType.Health;
                    teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[2] != null && playerNumber == 1
                 && !teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().has_Powerup && bufftype == BuffType.AttackDamage)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().bufftype = BuffType.AttackDamage;
                    teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[2] != null && playerNumber == 1
                 && !teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Armor)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().bufftype = BuffType.Armor;
                    teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                //Team Two player Two

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[2] != null && playerNumber == 2
                 && !teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Health)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().bufftype = BuffType.Health;
                    teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[2] != null && playerNumber == 2
                 && !teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().has_Powerup && bufftype == BuffType.AttackDamage)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().bufftype = BuffType.AttackDamage;
                    teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[2] != null && playerNumber == 2
                 && !teamManager.GetComponent<TeamManager>().teamOne[2].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Armor)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().bufftype = BuffType.Armor;
                    teamManager.GetComponent<TeamManager>().teamTwo[2].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                //Team Two player Three

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[1] != null && playerNumber == 3
                 && !teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Health)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().bufftype = BuffType.Health;
                    teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[1] != null && playerNumber == 3
                 && !teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().has_Powerup && bufftype == BuffType.AttackDamage)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().bufftype = BuffType.AttackDamage;
                    teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }

                if (team == Team.TeamTwo && teamManager.GetComponent<TeamManager>().teamTwo[1] != null && playerNumber == 3
                 && !teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().has_Powerup && bufftype == BuffType.Armor)
                {
                    has_Powerup = false;
                    teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().bufftype = BuffType.Armor;
                    teamManager.GetComponent<TeamManager>().teamTwo[1].GetComponent<MechController>().has_Powerup = true;
                    bufftype = BuffType.None;
                }
            }

            //Melee

            if (player.GetButtonDown("A") && !isMelee)
            {
                animator.SetTrigger("Melee");
                arm.SetActive(true);
                isMelee = true;
                StartCoroutine("Melee");
            }

          
            if (player.GetButtonDown("X") && timer_One <= 0)
            {
                animator.SetTrigger("Shotgun");
                if (dimensionManager.dimension == global::DimensionManager.Dimension.Stone)
                {
                    abilityOne_Stone.fillAmount = 0;
                    dimensionManager.StoneShard(bulletSpawn, teamNumber, transform, current_attackDamage);
                    timer_One = 8f;
                    coolDownTimerOne = 8f;
                    source.PlayOneShot(playerAbility);
                }

                if (dimensionManager.dimension == global::DimensionManager.Dimension.Lava)
                {
                    abilityOne_Lava.fillAmount = 0;
                    GameObject fireWaveClone = (GameObject)Instantiate(fireWave, new Vector3(bulletSpawn.transform.position.x, bulletSpawn.transform.position.y - 3f, bulletSpawn.transform.position.z), bulletSpawn.transform.rotation);
                    if(team == Team.TeamOne)
                    {
                        fireWaveClone.gameObject.tag = "FireWave_Blue";
                        fireWaveClone.gameObject.GetComponent<FireWaveCollision>().attackDamage = current_attackDamage;
                        source.PlayOneShot(playerAbility);
                    }
                    if (team == Team.TeamTwo)
                    {
                        fireWaveClone.gameObject.tag = "FireWave_Red";
                        fireWaveClone.gameObject.GetComponent<FireWaveCollision>().attackDamage = current_attackDamage;
                        source.PlayOneShot(playerAbility);
                    }

                    timer_One = 8f;
                    coolDownTimerOne = 8f;
                }

            }

            if (player.GetButtonDown("Y") && timer_Two <= 0)
            {
                animator.SetTrigger("Shotgun");
                if (dimensionManager.dimension == global::DimensionManager.Dimension.Stone)
                {
                    abilityTwo_Stone.fillAmount = 0;
                    armor += 20;
                    currentArmor += armor;
                    timer_Two = 15f;
                    coolDownTimerTwo = 15f;
                    source.PlayOneShot(playerAbility);
                }
                if (dimensionManager.dimension == global::DimensionManager.Dimension.Lava)
                {
                    abilityTwo_Lava.fillAmount = 0;
                    attackDamage += 20;
                    current_attackDamage += attackDamage;
                    timer_Two = 15f;
                    coolDownTimerTwo = 15f;
                    source.PlayOneShot(playerAbility);
                }
            }

            if (player.GetButton("B"))
            {
                animator.SetTrigger("Dance");
            }

            if (player.GetButtonDown("Use-Powerup"))
            {
                if (has_Powerup)
                {
                    if (bufftype == BuffType.Health && health < 200)
                    {
                        bufftype = BuffType.None;
                        health += 10;
                        has_Powerup = false;
                    }
                    if (bufftype == BuffType.AttackDamage)
                    {
                        bufftype = BuffType.None;
                        current_attackDamage += 10;
                        StartCoroutine("AttackDamage");
                        has_Powerup = false;
                    }
                    if (bufftype == BuffType.Armor)
                    {
                        bufftype = BuffType.None;
                        currentArmor += 10;
                        StartCoroutine("Armor");
                        has_Powerup = false;
                    }
                }
            }

            if (player.GetButtonDown("Center") && tutorial)
            {
                TutorialManager.numOfPlayersReady++;
                startingGame = true;
            }

            if (player.GetButtonDown("ClickToStart") && tutorial && !startingGame)
            {
                if(!paused)
                {
                    paused = true;
                    Time.timeScale = 0f;
                    tutorialManager.GetComponent<TutorialManager>().gameplay_Menu.SetActive(true);
                    return;
                }
                else
                {
                    paused = false;
                    Time.timeScale = 1f;
                    tutorialManager.GetComponent<TutorialManager>().gameplay_Menu.SetActive(false);
                    return;
                }
               
            }

            if (player.GetButtonDown("ClickToStart") && !tutorial && !GameOver)
            {
                if (!paused)
                {
                    paused = true;
                    Time.timeScale = 0f;
                    pauseMenu.SetActive(true);
                    return;
                }
                else
                {
                    paused = false;
                    Time.timeScale = 1f;
                    pauseMenu.SetActive(false);
                    return;
                }

            }




            if (player.GetButtonDown("TeamVote") && team == Team.TeamOne && TeamManager.TeamOneMeter == 100 && !voted)
            {
                TeamManager.T1_Votes++;
                voted = true;
            }

            if (player.GetButtonDown("TeamVote") && team == Team.TeamTwo && TeamManager.TeamTwoMeter == 100 && !voted)
            {
                TeamManager.T2_Votes++;
                voted = true;
            }
        }
    }

    private void ProcessDriverInput()
    {
        // Process movement
        if (moveVector.x != 0.0f || moveVector.y != 0.0f)
        {
            teamManager.GetComponent<TeamManager>().machineOne_Driver.GetComponent<Rigidbody>().velocity = new Vector3(moveVector.x * 10f, 0, moveVector.y * 10f);
            if(!source.isPlaying)
            {
                source.PlayOneShot(running);
            }
            blue_SuperMech.GetComponent<Animator>().SetBool("Walk", true);
             
        }
        else
        {
            blue_SuperMech.GetComponent<Animator>().SetBool("Walk", false);
        }

        if (Mathf.Abs(rotateVector.x) >= 0.1f && Mathf.Abs(rotateVector.y) >= 0.1f)
        {
            heading = Mathf.Atan2(rotateVector.x, rotateVector.y);
            teamManager.GetComponent<TeamManager>().machineOne_Driver.transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0f);
        }
        if (player.GetAxis("Fire") > 0 && timeStamp < Time.time && team == Team.TeamOne)
        {
            for(int i = 0; i < 5;i++)
            {
                timeStamp = Time.time + 0.8f;
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, arrayShot.transform.GetChild(i).transform.position, arrayShot.transform.GetChild(i).transform.rotation);
                bullet.GetComponent<Bullet>().atkDamage = 20;
                bullet.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                bullet.transform.GetChild(0).gameObject.SetActive(true);
                bullet.tag = "TA1_Bullet";
                bullet.GetComponent<Rigidbody>().velocity = arrayShot.transform.GetChild(i).transform.forward * bulletSpeed;
                global::DimensionManager.garbage.Add(bullet);
                source.PlayOneShot(supermech_Blast);
            }
            
        }
    }

    private void ProcessGunnerOneInput()
    {

        /*if (rotationY == 150 && start == 0)
        {
            rotationY = 180f;
            start++;
        }*/

        if (moveVector.x == 0)
        {
            Ysensitivity = 0.0f;

        }

        if (moveVector.x == -1 || moveVector.x == 1)
        {
            Ysensitivity = 1f;
        }
        
        rotationY += player.GetAxis("Move Horizontal") * Ysensitivity;


        //rotationY = Mathf.Clamp(rotationY, -30, 30);
        teamManager.GetComponent<TeamManager>().machineOne_TurretOne.transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, rotationY);
        if (player.GetAxis("Fire") > 0 && timeStamp < Time.time && team == Team.TeamOne)
        {
            timeStamp = Time.time + seconds;
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, teamManager.GetComponent<TeamManager>().machineOne_TurretOne.transform.GetChild(5).position
                , teamManager.GetComponent<TeamManager>().machineOne_TurretOne.transform.GetChild(5).rotation);
            bullet.GetComponent<Bullet>().atkDamage = 20;
            //bullet.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bullet.transform.GetChild(0).gameObject.SetActive(true);
            bullet.tag = "TA1_Bullet";
            bullet.GetComponent<Rigidbody>().velocity = teamManager.GetComponent<TeamManager>().machineOne_TurretOne.transform.GetChild(5).transform.forward * bulletSpeed;
            global::DimensionManager.garbage.Add(bullet);
            source.PlayOneShot(supermech_Blast);
        }
    }

    private void ProcessGunnerTwoInput()
    {
        /*if (rotationY == 150 && start == 0)
        {
            rotationY = 180f;
            start++;
        }*/

        
            /*float xRot = teamManager.GetComponent<TeamManager>().machineOne_TurretTwo.transform.rotation.x;
            xRot = -90;*/
        

        if (moveVector.x == 0)
        {
            Ysensitivity = 0.0f;
        }

        if (moveVector.x == -1 || moveVector.x == 1)
        {
            Ysensitivity = 1f;
        }

      

        rotationY += player.GetAxis("Move Horizontal") * Ysensitivity;


        //rotationY = Mathf.Clamp(rotationY, 150, 210);
        teamManager.GetComponent<TeamManager>().machineOne_TurretTwo.transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, rotationY);
        if (player.GetAxis("Fire") > 0 && timeStamp < Time.time && team == Team.TeamOne)
        {
            timeStamp = Time.time + seconds;
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, teamManager.GetComponent<TeamManager>().machineOne_TurretTwo.transform.GetChild(5).position
               , teamManager.GetComponent<TeamManager>().machineOne_TurretTwo.transform.GetChild(5).rotation);
            bullet.GetComponent<Bullet>().atkDamage = 20;
            //bullet.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bullet.transform.GetChild(0).gameObject.SetActive(true);
            bullet.tag = "TA1_Bullet";
            bullet.GetComponent<Rigidbody>().velocity = teamManager.GetComponent<TeamManager>().machineOne_TurretTwo.transform.GetChild(5).forward * bulletSpeed;
            global::DimensionManager.garbage.Add(bullet);
            source.PlayOneShot(supermech_Blast);
        }

    }

    private void ProcessDriverInput_2()
    {
        // Process movement
        if (moveVector.x != 0.0f || moveVector.y != 0.0f)
        {
            teamManager.GetComponent<TeamManager>().machineTwo_Driver.GetComponent<Rigidbody>().velocity = new Vector3(moveVector.x * 10f, 0, moveVector.y * 10f);
            if (!source.isPlaying)
            {
                source.PlayOneShot(running);
            }
            red_SuperMech.GetComponent<Animator>().SetBool("Walk", true);
        }
        else
        {
            red_SuperMech.GetComponent<Animator>().SetBool("Walk", false);
        }

        if (Mathf.Abs(rotateVector.x) >= 0.1f && Mathf.Abs(rotateVector.y) >= 0.1f)
        {
            heading = Mathf.Atan2(rotateVector.x, rotateVector.y);
            teamManager.GetComponent<TeamManager>().machineTwo_Driver.transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0f);
        }
        if (player.GetAxis("Fire") > 0 && timeStamp < Time.time && team == Team.TeamTwo)
        {
            for (int i = 0; i < 5; i++)
            {
                timeStamp = Time.time + 0.8f;
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, arrayShot.transform.GetChild(i).transform.position, arrayShot.transform.GetChild(i).transform.rotation);
                bullet.GetComponent<Bullet>().atkDamage = 20;
                bullet.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                bullet.transform.GetChild(1).gameObject.SetActive(true);
                bullet.tag = "TA2_Bullet";
                bullet.GetComponent<Rigidbody>().velocity = arrayShot.transform.GetChild(i).transform.forward * bulletSpeed;
                global::DimensionManager.garbage.Add(bullet);
                source.PlayOneShot(supermech_Blast);
            }

        }
    }

    private void ProcessGunnerOneInput_2()
    {

        /*if (rotationY == 150 && start == 0)
        {
            rotationY = 180f;
            start++;
        }
        */

        if (moveVector.x == 0)
        {
            Ysensitivity = 0.0f;

        }

        if (moveVector.x == -1 || moveVector.x == 1)
        {
            Ysensitivity = 1f;
        }

        rotationY += player.GetAxis("Move Horizontal") * Ysensitivity;


        //rotationY = Mathf.Clamp(rotationY, -30, 30);
        teamManager.GetComponent<TeamManager>().machineTwo_TurretOne.transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, rotationY);
        if (player.GetAxis("Fire") > 0 && timeStamp < Time.time && team == Team.TeamTwo)
        {
            timeStamp = Time.time + seconds;
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, teamManager.GetComponent<TeamManager>().machineTwo_TurretOne.transform.GetChild(5).position
                , teamManager.GetComponent<TeamManager>().machineTwo_TurretOne.transform.GetChild(5).rotation);
            bullet.GetComponent<Bullet>().atkDamage = 20;
            //bullet.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bullet.transform.GetChild(1).gameObject.SetActive(true);
            bullet.tag = "TA2_Bullet";
            bullet.GetComponent<Rigidbody>().velocity = teamManager.GetComponent<TeamManager>().machineTwo_TurretOne.transform.GetChild(5).transform.forward * bulletSpeed;
            global::DimensionManager.garbage.Add(bullet);
            source.PlayOneShot(supermech_Blast);
        }
    }

    private void ProcessGunnerTwoInput_2()
    {
        /*if (rotationY == 150 && start == 0)
       {
           rotationY = 180f;
           start++;
       }*/


        /*float xRot = teamManager.GetComponent<TeamManager>().machineOne_TurretTwo.transform.rotation.x;
        xRot = -90;*/


        if (moveVector.x == 0)
        {
            Ysensitivity = 0.0f;
        }

        if (moveVector.x == -1 || moveVector.x == 1)
        {
            Ysensitivity = 1f;
        }



        rotationY += player.GetAxis("Move Horizontal") * Ysensitivity;


        //rotationY = Mathf.Clamp(rotationY, 150, 210);
        teamManager.GetComponent<TeamManager>().machineTwo_TurretTwo.transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, rotationY);
        if (player.GetAxis("Fire") > 0 && timeStamp < Time.time && team == Team.TeamTwo)
        {
            timeStamp = Time.time + seconds;
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, teamManager.GetComponent<TeamManager>().machineTwo_TurretTwo.transform.GetChild(5).position
               , teamManager.GetComponent<TeamManager>().machineTwo_TurretTwo.transform.GetChild(5).rotation);
            bullet.GetComponent<Bullet>().atkDamage = 20;
            //bullet.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bullet.transform.GetChild(1).gameObject.SetActive(true);
            bullet.tag = "TA2_Bullet";
            bullet.GetComponent<Rigidbody>().velocity = teamManager.GetComponent<TeamManager>().machineTwo_TurretTwo.transform.GetChild(5).forward * bulletSpeed;
            global::DimensionManager.garbage.Add(bullet);
            source.PlayOneShot(supermech_Blast);
        }
    }


    private void ProcessCannonInput()
    {
          /*if (rotationY == 150 && start == 0)
        {
            rotationY = 180f;
            start++;
        }*/

        if (moveVector.x == 0)
        {
            Ysensitivity = 0.0f;

        }

        if (moveVector.x == -1 || moveVector.x == 1)
        {
            Ysensitivity = 1f;
        }
        
        rotationY += player.GetAxis("Move Horizontal") * Ysensitivity;


        //rotationY = Mathf.Clamp(rotationY, -30, 30);
        teamManager.GetComponent<TeamManager>().machineOne_TurretOne.transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, -rotationY);
        if (player.GetAxis("Fire") > 0 && timeStamp < Time.time && team == Team.TeamOne)
        {
            timeStamp = Time.time + seconds;
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, teamManager.GetComponent<TeamManager>().machineOne_TurretOne.transform.GetChild(5).position
                , teamManager.GetComponent<TeamManager>().machineOne_TurretOne.transform.GetChild(5).rotation);
            bullet.GetComponent<Bullet>().atkDamage = 20;
            //bullet.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bullet.transform.GetChild(0).gameObject.SetActive(true);
            bullet.tag = "TA1_Bullet";
            bullet.GetComponent<Rigidbody>().velocity = teamManager.GetComponent<TeamManager>().machineOne_TurretOne.transform.GetChild(5).transform.forward * bulletSpeed;
            global::DimensionManager.garbage.Add(bullet);
        }
    }

    private void ProcessCannonInput_2()
    {
        /*if (rotationY == 150 && start == 0)
         {
             rotationY = 180f;
             start++;
         }
         */

        if (moveVector.x == 0)
        {
            Ysensitivity = 0.0f;

        }

        if (moveVector.x == -1 || moveVector.x == 1)
        {
            Ysensitivity = 1f;
        }

        rotationY += player.GetAxis("Move Horizontal") * Ysensitivity;


        //rotationY = Mathf.Clamp(rotationY, -30, 30);
        teamManager.GetComponent<TeamManager>().machineTwo_TurretOne.transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, -rotationY);
        if (player.GetAxis("Fire") > 0 && timeStamp < Time.time && team == Team.TeamTwo)
        {
            timeStamp = Time.time + seconds;
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, teamManager.GetComponent<TeamManager>().machineTwo_TurretOne.transform.GetChild(5).position
                , teamManager.GetComponent<TeamManager>().machineTwo_TurretOne.transform.GetChild(5).rotation);
            bullet.GetComponent<Bullet>().atkDamage = 20;
            //bullet.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bullet.transform.GetChild(1).gameObject.SetActive(true);
            bullet.tag = "TA1_Bullet";
            bullet.GetComponent<Rigidbody>().velocity = teamManager.GetComponent<TeamManager>().machineTwo_TurretOne.transform.GetChild(5).transform.forward * bulletSpeed;
            global::DimensionManager.garbage.Add(bullet);
        }
    }

    void OnCollsionEnter(Collision col)
    {
       
    }

    void OnTriggerEnter(Collider col)
   {

        if (col.gameObject.tag == "Boss")
        {
            float damage = 30;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            rigid.AddForce(col.gameObject.transform.forward * 200, ForceMode.Impulse);
            Destroy(particle_Clone, 1f);
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
            //StartCoroutine("Hit");
        }
        /*if (col.gameObject.tag == "ArmTwo" && team == Team.TeamTwo && !invinsibleMode)
        {
            inRange = true;

        }

        if (col.gameObject.tag == "ArmOne" && team == Team.TeamTwo)
        {
            inRange = true;

        }*/

        if (col.gameObject.tag == "TeamOneBullet" && team == Team.TeamTwo && !invinsibleMode)
        {
            float damage = col.GetComponent<Bullet>().atkDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.blue_TotalDamage += (int)hitNumber;
            TeamManager.TeamOneMeter += 1;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            rigid.AddForce(col.gameObject.transform.forward * 30, ForceMode.Impulse);
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
            //StartCoroutine("Hit");

        }

        if (col.gameObject.tag == "TeamTwoBullet" && team == Team.TeamOne && !invinsibleMode)
        {
            float damage = col.GetComponent<Bullet>().atkDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.red_TotalDamage += (int)hitNumber;
            TeamManager.TeamTwoMeter += 1;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            rigid.AddForce(col.gameObject.transform.forward * 30, ForceMode.Impulse);
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
            //StartCoroutine("Hit");
        }

        if(col.gameObject.tag == "TeamOneStoneShard" && team == Team.TeamTwo && !invinsibleMode)
        {
            float damage = col.gameObject.GetComponent<StoneShard>().atkDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.blue_TotalDamage += (int)hitNumber;
            TeamManager.TeamOneMeter += 5;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            ////StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }

        if (col.gameObject.tag == "TeamTwoStoneShard" && team == Team.TeamOne && !invinsibleMode)
        {
            float damage = col.gameObject.GetComponent<StoneShard>().atkDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.red_TotalDamage += (int)hitNumber;
            TeamManager.TeamTwoMeter += 5;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            ////StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }

        if (col.gameObject.tag == "TeamOnePellet" && team == Team.TeamTwo && !invinsibleMode)
        {
            float damage = col.gameObject.GetComponent<Pellet>().attackDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.blue_TotalDamage += (int)hitNumber;
            TeamManager.TeamOneMeter += 5;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            Destroy(col.gameObject);
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            ////StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }

        if (col.gameObject.tag == "TeamTwoPellet" && team == Team.TeamOne && !invinsibleMode)
        {
            float damage = col.gameObject.GetComponent<Pellet>().attackDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.red_TotalDamage += (int)hitNumber;
            TeamManager.TeamTwoMeter += 5;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }

        if(col.gameObject.tag == "Boulder" && !invinsibleMode)
        {
            float damage = col.gameObject.GetComponent<Boulder>().attackDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }

        

        if (col.gameObject.tag == "AIBulletRed" && team == Team.TeamOne && !invinsibleMode)
        {
            float damage = col.gameObject.GetComponent<SmallAIBullet>().attackDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }


        if (col.gameObject.tag == "AIBulletBlue" && team == Team.TeamTwo && !invinsibleMode)
        {
            float damage = col.gameObject.GetComponent<SmallAIBullet>().attackDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }

        if (col.gameObject.tag == "TA2_Bullet" && team == Team.TeamOne && !invinsibleMode)
        {
            float damage = col.gameObject.GetComponent<Bullet>().atkDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.red_TotalDamage += (int)hitNumber;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }

        if (col.gameObject.tag == "TA1_Bullet" && team == Team.TeamTwo && !invinsibleMode)
        {
            float damage = col.gameObject.GetComponent<Bullet>().atkDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.blue_TotalDamage += (int)hitNumber;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }


        if (col.gameObject.tag == "ArmOne" && col.gameObject.transform.root.GetComponent<MechController>().isMelee && team == Team.TeamTwo && !invinsibleMode)
        {
            float damage = col.gameObject.transform.root.GetComponent<MechController>().current_attackDamage + 5;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.blue_TotalDamage += (int)hitNumber;
            TeamManager.TeamOneMeter += 1;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            rigid.AddForce(col.gameObject.transform.forward * 200, ForceMode.Impulse);
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            StartCoroutine("Pushed_Red");
            source.PlayOneShot(hit);
        }

        if (col.gameObject.tag == "ArmTwo" && col.gameObject.transform.root.GetComponent<MechController>().isMelee && team == Team.TeamOne && !invinsibleMode)
        {
            float damage = col.gameObject.transform.root.GetComponent<MechController>().current_attackDamage + 5;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.red_TotalDamage += (int)hitNumber;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            TeamManager.TeamTwoMeter += 1;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            rigid.AddForce(col.gameObject.transform.forward * 200, ForceMode.Impulse);
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            StartCoroutine("Pushed_Blue");
            source.PlayOneShot(hit);
        }


        if (col.gameObject.tag == "ArmOne" && team == Team.TeamTwo)
        {
            inRange = true;

        }

        if (col.gameObject.tag == "ArmTwo" && team == Team.TeamTwo && !invinsibleMode)
        {
            inRange = true;

        }

        if (col.gameObject.tag == "HazardTwo" && team == Team.TeamOne && !invinsibleMode)
        {

            float damage = col.gameObject.GetComponent<Boulder>().attackDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.red_TotalDamage += (int)hitNumber;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            TeamManager.TeamTwoMeter += 15;
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }

        if (col.gameObject.tag == "HazardOne" && team == Team.TeamTwo && !invinsibleMode)
        {
            float damage = col.gameObject.GetComponent<Boulder>().attackDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + armor)));
            TeamManager.blue_TotalDamage += (int)hitNumber;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            TeamManager.TeamOneMeter += 15;
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);

        }

        if (col.gameObject.tag == "SpecialHazardTwo" && team == Team.TeamOne && !invinsibleMode)
        {
            float damage = 20;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.red_TotalDamage += (int)hitNumber;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            TeamManager.TeamTwoMeter += 20;
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }

        if (col.gameObject.tag == "SpecialHazardOne" && team == Team.TeamTwo && !invinsibleMode)
        {
            float damage = 20;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.blue_TotalDamage += (int)hitNumber;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            TeamManager.TeamOneMeter += 20;
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }


        if (col.gameObject.tag == "SpecialHazard" && !invinsibleMode)
        {

            float damage = 25;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }

        if (col.gameObject.tag == "SpecialHazardOne" && team == Team.TeamOne && !invinsibleMode)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
        }
        if (col.gameObject.tag == "SpecialHazardTwo" && team == Team.TeamTwo && !invinsibleMode)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
        }

        if (col.gameObject.tag == "HazardOne" && team == Team.TeamOne && !invinsibleMode)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
        }

        if (col.gameObject.tag == "HazardTwo" && team == Team.TeamTwo && !invinsibleMode)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);

        }        

        if(col.gameObject.tag == "FireWave_Blue" && team == Team.TeamTwo && !invinsibleMode)
        {
            float damage = col.gameObject.GetComponent<FireWaveCollision>().atkDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.blue_TotalDamage += (int)hitNumber;
            TeamManager.TeamOneMeter += 5;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }


        if (col.gameObject.tag == "FireWave_Red" && team == Team.TeamOne && !invinsibleMode)
        {
            float damage = col.gameObject.GetComponent<FireWaveCollision>().atkDamage;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            TeamManager.red_TotalDamage += (int)hitNumber;
            TeamManager.TeamTwoMeter += 5;
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }

        if(col.gameObject.tag == "VolcanoFlame" && dimensionManager.dimension == global::DimensionManager.Dimension.Lava)
        {
            float damage = 10;
            float hitNumber = (damage * (100 / (100 + currentArmor)));
            health -= (int)(damage * (100 / (100 + currentArmor)));
            StartCoroutine("HitNumber", hitNumber);
            damage = 0;
            pos = gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            //StartCoroutine("Hit");
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }

        if (col.gameObject.tag == "Untagged" && dimensionManager.dimension == global::DimensionManager.Dimension.Lava)
        {
            
            pos = gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            fireDamage = 10f;
            onFire = true;
            //source.PlayOneShot(hit);
            source.PlayOneShot(hit);

        }


        if (col.gameObject.tag == "SpecialFlame" && dimensionManager.dimension == global::DimensionManager.Dimension.Lava)
        {
            pos = gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            fireDamage = 15f;
            onFire = true;
            source.PlayOneShot(hit);

        }

        if (col.gameObject.tag == "Untagged" && dimensionManager.dimension == global::DimensionManager.Dimension.Lava && lava_PushBlue && team == Team.TeamOne)
        {
            pos = gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamTwoMeter += 15;
            fireDamage = 10f;
            onFire = true;
            source.PlayOneShot(hit);
        }

        if (col.gameObject.tag == "Untagged" && dimensionManager.dimension == global::DimensionManager.Dimension.Lava && lava_PushRed && team == Team.TeamTwo)
        {
            pos = gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamOneMeter += 15;
            fireDamage = 10f;
            onFire = true;
            source.PlayOneShot(hit);
            
        }

        if (col.gameObject.tag == "SpecialFlame" && dimensionManager.dimension == global::DimensionManager.Dimension.Lava && lava_PushBlue && team == Team.TeamOne)
        {
            pos = gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamTwoMeter += 20;
            fireDamage = 15f;
            onFire = true;
            source.PlayOneShot(hit);
            
        }

        if (col.gameObject.tag == "SpecialFlame" && dimensionManager.dimension == global::DimensionManager.Dimension.Lava && lava_PushRed && team == Team.TeamTwo)
        {
            pos = gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            TeamManager.TeamOneMeter += 20;
            fireDamage = 15f;
            onFire = true;
            source.PlayOneShot(hit);
        }



        if (col.gameObject.tag == "Untagged" && health <= 0)
        {
            pos = gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            if(team == Team.TeamOne)
            {
                TeamManager.numofTeamOnePlayers--;
            }
            if (team == Team.TeamTwo)
            {
                TeamManager.numofTeamTwoPlayers--;
            }
            animator.SetBool("Running", false);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
            
        }

        if (col.gameObject.tag == "SpecialFlame" && health <= 0 && dimensionManager.dimension == global::DimensionManager.Dimension.Lava)
        {
            pos = gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            if (team == Team.TeamOne)
            {
                TeamManager.numofTeamOnePlayers--;
            }
            if (team == Team.TeamTwo)
            {
                TeamManager.numofTeamTwoPlayers--;
            }
            animator.SetBool("Running", false);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
           
        }

        if (col.gameObject.tag == "VolcanoFlame" && health <= 0 && dimensionManager.dimension == global::DimensionManager.Dimension.Lava)
        {
            pos = gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            if (team == Team.TeamOne)
            {
                TeamManager.numofTeamOnePlayers--;
            }
            if (team == Team.TeamTwo)
            {
                TeamManager.numofTeamTwoPlayers--;
            }
            animator.SetBool("Running", false);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
            
        }


        if (col.gameObject.tag == "Boulder" && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            if (team == Team.TeamOne)
            {
                TeamManager.numofTeamOnePlayers--;
            }
            if (team == Team.TeamTwo)
            {
                TeamManager.numofTeamTwoPlayers--;
            }
            animator.SetBool("Running", false);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");

        }




        if (col.gameObject.tag == "TeamOneBullet" && team == Team.TeamTwo && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamTwoPlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");

        }

        if (col.gameObject.tag == "TeamTwoBullet" && team == Team.TeamOne && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamOnePlayers--;
            teamManager.GetComponent<TeamManager>().teamOne.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "TeamOneStoneShard" && team == Team.TeamTwo && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamTwoPlayers--;
            teamManager.GetComponent<TeamManager>().teamOne.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "TeamTwoStoneShard" && team == Team.TeamOne && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamOnePlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "TeamOnePellet" && team == Team.TeamTwo && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamTwoPlayers--;
            teamManager.GetComponent<TeamManager>().teamOne.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "TeamTwoPellet" && team == Team.TeamOne && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamOnePlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "TA2_Bullet" && team == Team.TeamOne && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamOnePlayers--;
            teamManager.GetComponent<TeamManager>().teamOne.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "TA1_Bullet" && team == Team.TeamTwo && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamTwoPlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }


        if (col.gameObject.tag == "ArmOne" && col.gameObject.transform.root.GetComponent<MechController>().isMelee && team == Team.TeamTwo && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamTwoPlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            animator.SetBool("Running", false);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }




        if (col.gameObject.tag == "ArmTwo" && col.gameObject.transform.root.GetComponent<MechController>().isMelee && team == Team.TeamOne && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamOnePlayers--;
            teamManager.GetComponent<TeamManager>().teamOne.Remove(gameObject);
            animator.SetBool("Running", false);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "Boulder" && !invinsibleMode && health <= 0 && team == Team.TeamOne)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamOnePlayers--;
            teamManager.GetComponent<TeamManager>().teamOne.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "Boulder" && !invinsibleMode && health <= 0 && team == Team.TeamTwo)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamTwoPlayers--;
            teamManager.GetComponent<TeamManager>().teamOne.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }





        if (col.gameObject.tag == "HazardOne" && team == Team.TeamTwo && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamTwoPlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "HazardTwo" && team == Team.TeamOne && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamOnePlayers--;
            teamManager.GetComponent<TeamManager>().teamOne.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "SpecialHazard" && team == Team.TeamOne && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamOnePlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "SpecialHazard" && team == Team.TeamTwo && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamTwoPlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "SpecialHazardTwo" && team == Team.TeamTwo && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamOnePlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "SpecialHazardOne" && team == Team.TeamTwo && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamTwoPlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "FireWave_Blue" && team == Team.TeamTwo && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamTwoPlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }


        if (col.gameObject.tag == "FireWave_Red" && team == Team.TeamOne && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamOnePlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);

            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "AIBulletRed" && team == Team.TeamOne && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamOnePlayers--;
            teamManager.GetComponent<TeamManager>().teamOne.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }


        if (col.gameObject.tag == "AIBulletBlue" && team == Team.TeamTwo && !invinsibleMode && health <= 0)
        {
            pos = col.gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamTwoPlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "Boss" && team == Team.TeamOne && !invinsibleMode && health <= 0)
        {
            pos = gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamOnePlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        if (col.gameObject.tag == "Boss" && team == Team.TeamTwo && !invinsibleMode && health <= 0)
        {
            pos = gameObject.transform.position;
            GameObject particle_Clone = (GameObject)Instantiate(particle_Hit, pos, particle_Hit.transform.rotation);
            Destroy(particle_Clone, 1f);
            transform.GetComponent<BoxCollider>().enabled = false;
            //transform.GetComponent<Rigidbody>().isKinematic = true;
            TeamManager.numofTeamTwoPlayers--;
            teamManager.GetComponent<TeamManager>().teamTwo.Remove(gameObject);
            animator.SetBool("Running", false);
            Destroy(col.gameObject);
            source.PlayOneShot(hit);
            source.PlayOneShot(player_Explosion);
            StartCoroutine("Death");
        }

        /*if (col.gameObject.tag == "AIMelee")
        {
            
            col.gameObject.transform.parent.transform.GetComponent<SmallAI>().melee = true;
            col.gameObject.transform.parent.transform.GetComponent<SmallAI>().InvokeRepeating("Melee", 2f, 5f);     
        }*/

        if (col.gameObject.tag == "AttackDamage_Buff" && !has_Powerup && team == Team.TeamOne)
        {
            has_Powerup = true;
            bufftype = BuffType.AttackDamage;
            Destroy(col.gameObject);
            source.PlayOneShot(powerupGrab);
        }

        if (col.gameObject.tag == "Armor_Buff" && !has_Powerup && team == Team.TeamOne)
        {

            has_Powerup = true;
            bufftype = BuffType.Armor;
            Destroy(col.gameObject);
            source.PlayOneShot(powerupGrab);
        }

        if (col.gameObject.tag == "Health_Increase" && !has_Powerup && team == Team.TeamOne)
        {
            has_Powerup = true;
            bufftype = BuffType.Health;
            Destroy(col.gameObject);
            source.PlayOneShot(powerupGrab);
        }

        if (col.gameObject.tag == "AttackDamage_Buff_2" && !has_Powerup && team == Team.TeamTwo)
        {
            has_Powerup = true;
            bufftype = BuffType.AttackDamage;
            Destroy(col.gameObject);
            source.PlayOneShot(powerupGrab);
        }

        if (col.gameObject.tag == "Armor_Buff_2" && !has_Powerup && team == Team.TeamTwo)
        {

            has_Powerup = true;
            bufftype = BuffType.Armor;
            Destroy(col.gameObject);
            source.PlayOneShot(powerupGrab);
        }

        if (col.gameObject.tag == "Health_Increase_2" && !has_Powerup && team == Team.TeamTwo)
        {
            has_Powerup = true;
            bufftype = BuffType.Health;
            Destroy(col.gameObject);
            source.PlayOneShot(powerupGrab);
        }



    }

    void OnTriggerStay(Collider col)
    {
        /*if(col.gameObject.tag == "ArmOne" && team == Team.TeamTwo && !inRange)
        {
            inRange = true;
        }*/

        /*if (col.gameObject.tag == "ArmOne" && col.gameObject.transform.parent.GetComponent<MechController>().isMelee && team == Team.TeamTwo)
        {
            float damage = 10;
            health -= (int)(damage * (100 / (100 + armor)));
            damage = 0;
        }*/


    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "AIMelee")
        {
            col.gameObject.transform.parent.transform.GetComponent<SmallAI>().melee = false;
            col.gameObject.transform.parent.transform.GetComponent<SmallAI>().CancelInvoke("Melee");
        }

        if(col.gameObject.tag == "Untagged")
        {
            onFire = false;
        }

        if (col.gameObject.tag == "SpecialFlame")
        {
            onFire = false;
        }
    }
    IEnumerator AttackDamage()
    {
        yield return new WaitForSeconds(15);
        current_attackDamage -= 10;
        has_Powerup = false;
    }

    IEnumerator Armor()
    {
        yield return new WaitForSeconds(15);
        currentArmor -= 10;
        has_Powerup = false;
    }

    IEnumerator Melee()
    {
       
        yield return new WaitForSeconds(0.5f);
        arm.SetActive(false);
        isMelee = false;
    }

    IEnumerator Hit()
    {
        mech_UpperBody.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        mech_UpperBody.material = material;
    }

    IEnumerator Pushed_Red()
    {
        lava_PushRed = true;
        yield return new WaitForSeconds(1f);
        lava_PushRed = false;
    }

    IEnumerator Pushed_Blue()
    {
        lava_PushBlue = true;
        yield return new WaitForSeconds(1f);
        lava_PushBlue = false;
    }

    IEnumerator HitNumber(int damage)
    {
        if(damage >= 10)
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

    IEnumerator Death()
    {

        animator.SetTrigger("Death");
        explosion.SetActive(true);
        yield return new WaitForSeconds(10f);
        transform.gameObject.SetActive(false);
    }

   
}
