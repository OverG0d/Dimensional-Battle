using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TeamManager : MonoBehaviour
{


    Vector3 machineOne_StartPosition;
    Vector3 machineTwo_StartPosition;

    Quaternion machineOne_StartRotation;
    Quaternion machineTwo_StartRotation;

    public List<GameObject> teamOne = new List<GameObject>();
    public List<GameObject> teamTwo = new List<GameObject>();

    

    public static GameObject playerOnePos;
    public static GameObject playerTwoPos;
    public static GameObject playerThreePos;

    public static GameObject playerFourPos;
    public static GameObject playerFivePos;
    public static GameObject playerSixPos;


    public static float TeamOneMeter = 0;
    public static float TeamTwoMeter = 0;


    public static int numofTeamOnePlayers = 3;
    public static int numofTeamTwoPlayers = 3;

    public static int T1_Votes = 0;
    public static int T2_Votes = 0;

    public float T1A_Timer;
    public float T2A_Timer;

    //Machine with three players
    public GameObject machineOne_Driver;
    public GameObject machineOne_TurretOne;
    public GameObject machineOne_TurretTwo;
    public GameObject machineOne_Cannon;

    public GameObject machineTwo_Driver;
    public GameObject machineTwo_TurretOne;
    public GameObject machineTwo_TurretTwo;
    public GameObject machineTwo_Cannon;

    public List<GameObject> machineOne_spawns = new List<GameObject>();
    public List<GameObject> machineTwo_spawns = new List<GameObject>();

    //Randomize List
    public List<GameObject> randomizedTeamOne = new List<GameObject>();
    public List<GameObject> randomizedTeamTwo = new List<GameObject>();

    public bool no_TeamAbility;
    public bool no_TeamAbility_2;
    public bool isTeamAbility;
    public bool isTeamAbility_2;

    public Image teamTimer_1;
    public Image teamTimer_2;
    //Win Condition
    public Text text;

    //Player UI's
    public GameObject BP1_UI;
    public GameObject BP2_UI;
    public GameObject BP3_UI;

    public GameObject RP1_UI;
    public GameObject RP2_UI;
    public GameObject RP3_UI;

    public GameObject Team_Manager_One;
    public GameObject Team_Manager_Two;

    //SuperMech UI's
    public GameObject SuperMech_Blue;
    public GameObject SuperMech_Red;

    public GameObject blueScreen;

    public static float blue_TotalDamage;
    public static float red_TotalDamage;

    public AudioClip clip;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {

        blueScreen.gameObject.SetActive(false);
        playerOnePos = teamOne[0];
        playerTwoPos = teamOne[1];
        playerThreePos = teamOne[2];

        playerFourPos = teamTwo[0];
        playerFivePos = teamTwo[1];
        playerSixPos = teamTwo[2];
        machineOne_StartPosition = machineOne_Driver.transform.position;
        machineTwo_StartPosition = machineTwo_Driver.transform.position;
        machineOne_StartRotation = machineOne_Driver.transform.rotation;
        machineTwo_StartRotation = machineTwo_Driver.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        
        teamTimer_1.fillAmount = T1A_Timer * 0.01f;



        teamTimer_2.fillAmount = T2A_Timer * 0.01f;
        

        if (TeamOneMeter >= 100)
        {
            TeamOneMeter = 100;
        }

        if (TeamTwoMeter >= 100)
        {
            TeamTwoMeter = 100;
        }

        if(T1_Votes == 2 && TeamOneMeter == 100 && numofTeamOnePlayers == 3)
        {

            BP1_UI.gameObject.SetActive(false);
            BP2_UI.gameObject.SetActive(false);
            BP3_UI.gameObject.SetActive(false);
            Team_Manager_One.gameObject.SetActive(false);
            SuperMech_Blue.gameObject.SetActive(true);
            SmallAI.supermech_Blue = true;
            if (!isTeamAbility)
            {
                RandomizeAndAssign();
            }
            T1_Votes = 0;
            source.PlayOneShot(clip);
           
        }


        if (T1_Votes == 2 && TeamOneMeter == 100 && numofTeamOnePlayers == 2)
        {
            BP1_UI.gameObject.SetActive(false);
            BP2_UI.gameObject.SetActive(false);
            BP3_UI.gameObject.SetActive(false);
            Team_Manager_One.gameObject.SetActive(false);
            SuperMech_Blue.gameObject.SetActive(true);
            SmallAI.supermech_Blue = true;
            if (!isTeamAbility)
            {
                RandomizeAndAssign_TwoPlayers();
            }
            T1_Votes = 0;
            source.PlayOneShot(clip);
           
        }


        if (T1_Votes == 1 && TeamOneMeter == 100 && numofTeamOnePlayers == 1)
        {
            BP1_UI.gameObject.SetActive(false);
            BP2_UI.gameObject.SetActive(false);
            BP3_UI.gameObject.SetActive(false);
            Team_Manager_One.gameObject.SetActive(false);
            SuperMech_Blue.gameObject.SetActive(true);
            SmallAI.supermech_Blue = true;
            if (!isTeamAbility)
            {
                FinalPlayer();
            }
            T1_Votes = 0;
            source.PlayOneShot(clip);
           
        }

        if (T2_Votes == 2 && TeamTwoMeter == 100 && numofTeamTwoPlayers == 3)
        {
            RP1_UI.gameObject.SetActive(false);
            RP2_UI.gameObject.SetActive(false);
            RP3_UI.gameObject.SetActive(false);
            Team_Manager_Two.gameObject.SetActive(false);
            SuperMech_Red.gameObject.SetActive(true);
            SmallAI.supermech_Red = true;
            
            if (!isTeamAbility_2)
            {
                RandomizeAndAssign_2();
            }
            T2_Votes = 0;
            source.PlayOneShot(clip);
        }


        if (T2_Votes == 2 && TeamTwoMeter == 100 && numofTeamTwoPlayers == 2)
        {
            RP1_UI.gameObject.SetActive(false);
            RP2_UI.gameObject.SetActive(false);
            RP3_UI.gameObject.SetActive(false);
            Team_Manager_Two.gameObject.SetActive(false);
            SuperMech_Red.gameObject.SetActive(true);
            SmallAI.supermech_Red = true;
            if (!isTeamAbility_2)
            {
                RandomizeAndAssign_TwoPlayers_2();
            }
            T2_Votes = 0;
            source.PlayOneShot(clip);
           
        }


        if (T2_Votes == 1 && TeamTwoMeter == 100 && numofTeamTwoPlayers == 1)
        {
            RP1_UI.gameObject.SetActive(false);
            RP2_UI.gameObject.SetActive(false);
            RP3_UI.gameObject.SetActive(false);
            Team_Manager_Two.gameObject.SetActive(false);
            SuperMech_Red.gameObject.SetActive(true);
            SmallAI.supermech_Red = true;
            if (!isTeamAbility_2)
            {
                FinalPlayer_2();
            }
            T2_Votes = 0;
            source.PlayOneShot(clip);
           
        }



        if (T1A_Timer > 0 && no_TeamAbility && !DimensionManager.switchingDimension)
        {
            T1A_Timer -= Time.deltaTime * 3;
        }

        if (T1A_Timer <= 0 || machineOne_Driver.GetComponent<AllForOneMachine>().health <= 0)
        {
            for (int i = 0; i < randomizedTeamOne.Count; i++)
            {
                if (randomizedTeamOne[i] != null)
                {
                    randomizedTeamOne[i].transform.position = machineOne_spawns[i].transform.position;
                    randomizedTeamOne[i].transform.rotation = machineOne_spawns[i].transform.rotation;

                }
            }
            BP1_UI.gameObject.SetActive(true);
            BP2_UI.gameObject.SetActive(true);
            BP3_UI.gameObject.SetActive(true);
            Team_Manager_One.gameObject.SetActive(true);
            SuperMech_Blue.gameObject.SetActive(false);
            isTeamAbility = false;
            no_TeamAbility = false;
            TeamOneMeter = 0;
            T1A_Timer = 1;
            SmallAI.supermech_Blue = false;
            Reset();
            

        }

        if (T2A_Timer > 0 && no_TeamAbility_2 && !DimensionManager.switchingDimension)
        {
            T2A_Timer -= Time.deltaTime * 3;
        }

        if (T2A_Timer <= 0 || machineTwo_Driver.GetComponent<AllForOneMachine>().health <= 0)
        {
        for (int i = 0; i < randomizedTeamTwo.Count; i++)
        {
            if (randomizedTeamTwo[i] != null)
            {
                randomizedTeamTwo[i].transform.position = machineTwo_spawns[i].transform.position;
                randomizedTeamTwo[i].transform.rotation = machineTwo_spawns[i].transform.rotation;

            }
        }
            RP1_UI.gameObject.SetActive(true);
            RP2_UI.gameObject.SetActive(true);
            RP3_UI.gameObject.SetActive(true);
            Team_Manager_Two.gameObject.SetActive(true);
            SuperMech_Red.gameObject.SetActive(false);
            isTeamAbility = false;
            no_TeamAbility = false;
            TeamTwoMeter = 0;
            T2A_Timer = 1;
            SmallAI.supermech_Red = false;
            Reset_2();
            

        }

        if(numofTeamOnePlayers > 0)
        {
            text.text = "";
        }

        if (numofTeamTwoPlayers > 0)
        {
            text.text = "";
        }

        if (numofTeamOnePlayers == 0)
        {
            TeamOneMeter = 0;
            TeamTwoMeter = 0;
            DimensionManager.waves = 0;
            DimensionManager.Blue_AI = 0;
            DimensionManager.Red_AI = 0;
            DimensionManager.MaxAI_BlueTeam = 1;
            DimensionManager.MaxAI_RedTeam = 1;
            GarbageManager.Garbage();
            text.text = "Red Team Wins!";
            text.color = Color.red;
            blueScreen.gameObject.SetActive(true);
            StartCoroutine("Lobby");
        }

        if (numofTeamTwoPlayers == 0)
        {
            TeamOneMeter = 0;
            TeamTwoMeter = 0;
            DimensionManager.waves = 0;
            DimensionManager.Blue_AI = 0;
            DimensionManager.Red_AI = 0;
            DimensionManager.MaxAI_BlueTeam = 1;
            DimensionManager.MaxAI_RedTeam = 1;
            GarbageManager.Garbage();
            text.text = "Blue Team Wins!";
            text.color = Color.blue;
            blueScreen.gameObject.SetActive(true);
            StartCoroutine("Lobby");

        }

    }

    void RandomizeAndAssign()
    {
        no_TeamAbility = true;
        isTeamAbility = true;
        for (int i = 0; i < teamOne.Count;i++)
        {
            randomizedTeamOne[i] = teamOne[i];
        }
        


        for (int i = 0; i < randomizedTeamOne.Count; i++)
        {
            GameObject temp = randomizedTeamOne[i];
            int randomIndex = Random.Range(i, randomizedTeamOne.Count);
            randomizedTeamOne[i] = randomizedTeamOne[randomIndex];
            randomizedTeamOne[randomIndex] = temp;
        }
        
        
        randomizedTeamOne[0].GetComponent<MechController>().isDriver = true;
        randomizedTeamOne[1].GetComponent<MechController>().isGunnerOne = true;
        randomizedTeamOne[2].GetComponent<MechController>().isGunnerTwo = true;

        randomizedTeamOne[0].transform.position = new Vector3(randomizedTeamOne
            [0].transform.position.x - 1000, randomizedTeamOne
            [0].transform.position.y,
            randomizedTeamOne
            [0].transform.position.z);
        randomizedTeamOne[1].transform.position = new Vector3(randomizedTeamOne
           [1].transform.position.x - 1000, randomizedTeamOne
           [1].transform.position.y,
           randomizedTeamOne
           [1].transform.position.z);
        randomizedTeamOne[2].transform.position = new Vector3(randomizedTeamOne
            [2].transform.position.x - 1000, randomizedTeamOne
            [2].transform.position.y,
            randomizedTeamOne
            [2].transform.position.z);
        machineOne_Driver.SetActive(true);
        T1A_Timer = 100;
        //machineOne_Driver.GetComponent<AllForOneMachine>().StartCoroutine("Invincible");
    }



    public void RandomizeAndAssign_TwoPlayers()
    {
        if (numofTeamOnePlayers == 2)
        {
            randomizedTeamOne.RemoveAt(2);
        }
  
        no_TeamAbility = true;
        isTeamAbility = true;
        for (int i = 0; i < teamOne.Count; i++)
        {
            randomizedTeamOne[i] = teamOne[i];
        }
        for (int i = 0; i < randomizedTeamOne.Count; i++)
        {
            GameObject temp = randomizedTeamOne[i];
            int randomIndex = Random.Range(i, randomizedTeamOne.Count);
            randomizedTeamOne[i] = randomizedTeamOne[randomIndex];
            randomizedTeamOne[randomIndex] = temp;
        }


            
            randomizedTeamOne[0].GetComponent<MechController>().isDriver = true;
            randomizedTeamOne[1].GetComponent<MechController>().isCannon = true;




        randomizedTeamOne[0].transform.position = new Vector3(randomizedTeamOne
           [0].transform.position.x - 1000, randomizedTeamOne
           [0].transform.position.y,
           randomizedTeamOne
           [0].transform.position.z);
        randomizedTeamOne[1].transform.position = new Vector3(randomizedTeamOne
           [1].transform.position.x - 1000, randomizedTeamOne
           [1].transform.position.y,
           randomizedTeamOne
           [1].transform.position.z);
        machineOne_TurretOne.SetActive(false);
        machineOne_TurretTwo.SetActive(false);
        machineOne_Cannon.SetActive(true);
        machineOne_Driver.SetActive(true);
        T1A_Timer = 100;
        //machineOne_Driver.GetComponent<AllForOneMachine>().StartCoroutine("Invincible");

    }

    public void FinalPlayer()
    {

        if (numofTeamOnePlayers == 1)
        {
            randomizedTeamOne.RemoveAt(1);
        }
        no_TeamAbility = true;
        isTeamAbility = true;
        randomizedTeamOne[0] = teamOne[0];
        
        randomizedTeamOne[0].GetComponent<MechController>().isDriver = true;
       
        randomizedTeamOne[0].transform.position = new Vector3(randomizedTeamOne
           [0].transform.position.x - 1000, randomizedTeamOne
           [0].transform.position.y,
           randomizedTeamOne
           [0].transform.position.z);
        machineOne_TurretOne.SetActive(false);
        machineOne_TurretTwo.SetActive(false);
        machineOne_Cannon.SetActive(false);
        machineOne_Driver.SetActive(true);
        T1A_Timer = 100;
        //machineOne_Driver.GetComponent<AllForOneMachine>().StartCoroutine("Invincible");

    }

    void Reset()
    {
        machineOne_Driver.GetComponent<AllForOneMachine>().health = 400;
        machineOne_Driver.SetActive(false);
        machineOne_Driver.transform.position = machineOne_StartPosition;
        machineOne_Driver.transform.rotation = machineOne_StartRotation;

        for(int i = 0; i < randomizedTeamOne.Count;i++)
        {
            if (randomizedTeamOne[i] != null)
            {
                randomizedTeamOne[i].GetComponent<MechController>().voted = false;
            }
        }

        for (int i = 0; i < randomizedTeamOne.Count; i++)
        {
            if (randomizedTeamOne[i] != null)
            {
                randomizedTeamOne[i].GetComponent<MechController>().isDriver = false;
                randomizedTeamOne[i].GetComponent<MechController>().isGunnerOne = false;
                randomizedTeamOne[i].GetComponent<MechController>().isGunnerTwo = false;
                randomizedTeamOne[i].GetComponent<MechController>().isCannon = false;
                randomizedTeamOne[i].GetComponent<MechController>().voted = false;
            }
            
        }
        for (int i = 0; i < randomizedTeamOne.Count; i++)
        {
            if (randomizedTeamOne[i] != null)
            {
                randomizedTeamOne[i].SetActive(true);
            }
        }

        
        for (int i = 0; i < randomizedTeamOne.Count; i++)
        {
            if (randomizedTeamOne[i] != null)
            {
                randomizedTeamOne[i] = null;
            }
        }
       
    }

    void RandomizeAndAssign_2()
    {
        no_TeamAbility_2 = true;
        isTeamAbility_2 = true;
        for (int i = 0; i < teamTwo.Count; i++)
        {
            randomizedTeamTwo[i] = teamTwo[i];
        }



        for (int i = 0; i < randomizedTeamTwo.Count; i++)
        {
            GameObject temp = randomizedTeamTwo[i];
            int randomIndex = Random.Range(i, randomizedTeamTwo.Count);
            randomizedTeamTwo[i] = randomizedTeamTwo[randomIndex];
            randomizedTeamTwo[randomIndex] = temp;
        }


        randomizedTeamTwo[0].GetComponent<MechController>().isDriver = true;
        randomizedTeamTwo[1].GetComponent<MechController>().isGunnerOne = true;
        randomizedTeamTwo[2].GetComponent<MechController>().isGunnerTwo = true;

        randomizedTeamTwo[0].transform.position = new Vector3(randomizedTeamTwo
            [0].transform.position.x + 1000, randomizedTeamTwo
            [0].transform.position.y,
            randomizedTeamTwo
            [0].transform.position.z);
        randomizedTeamTwo[1].transform.position = new Vector3(randomizedTeamTwo
           [1].transform.position.x + 1000, randomizedTeamTwo
           [1].transform.position.y,
           randomizedTeamTwo
           [1].transform.position.z);
        randomizedTeamTwo[2].transform.position = new Vector3(randomizedTeamTwo
            [2].transform.position.x + 1000, randomizedTeamTwo
            [2].transform.position.y,
            randomizedTeamTwo
            [2].transform.position.z);
        machineTwo_Driver.SetActive(true);
        T2A_Timer = 100;
        //machineTwo_Driver.GetComponent<AllForOneMachine>().StartCoroutine("Invincible");
    }



    public void RandomizeAndAssign_TwoPlayers_2()
    {

        if (numofTeamTwoPlayers == 2)
        {
            randomizedTeamTwo.RemoveAt(2);
        }

        no_TeamAbility_2 = true;
        isTeamAbility_2 = true;
        for (int i = 0; i < teamTwo.Count; i++)
        {
            randomizedTeamTwo[i] = teamTwo[i];
        }
        for (int i = 0; i < randomizedTeamTwo.Count; i++)
        {
            GameObject temp = randomizedTeamTwo[i];
            int randomIndex = Random.Range(i, randomizedTeamTwo.Count);
            randomizedTeamTwo[i] = randomizedTeamTwo[randomIndex];
            randomizedTeamTwo[randomIndex] = temp;
        }
        



        randomizedTeamTwo[0].transform.position = new Vector3(randomizedTeamTwo
           [0].transform.position.x + 1000, randomizedTeamTwo
           [0].transform.position.y,
           randomizedTeamTwo
           [0].transform.position.z);
        randomizedTeamTwo[1].transform.position = new Vector3(randomizedTeamTwo
           [1].transform.position.x + 1000, randomizedTeamTwo
           [1].transform.position.y,
           randomizedTeamTwo
           [1].transform.position.z);
        machineTwo_TurretOne.SetActive(false);
        machineTwo_TurretTwo.SetActive(false);
        machineTwo_Cannon.SetActive(true);
        machineTwo_Driver.SetActive(true);
        T2A_Timer = 100;
        //machineTwo_Driver.GetComponent<AllForOneMachine>().StartCoroutine("Invincible");
    }

    public void FinalPlayer_2()
    {

        if (numofTeamTwoPlayers == 1)
        {
            randomizedTeamTwo.RemoveAt(1);
        }
        no_TeamAbility_2 = true;
        isTeamAbility_2 = true;
        randomizedTeamTwo[0] = teamTwo[0];

        randomizedTeamTwo[0].GetComponent<MechController>().isDriver = true;

        randomizedTeamTwo[0].transform.position = new Vector3(randomizedTeamTwo
           [0].transform.position.x - 1000, randomizedTeamTwo
           [0].transform.position.y,
           randomizedTeamTwo
           [0].transform.position.z);
        machineTwo_TurretOne.SetActive(false);
        machineTwo_TurretTwo.SetActive(false);
        machineTwo_Cannon.SetActive(false);
        machineTwo_Driver.SetActive(true);
        T2A_Timer = 100;
        //machineTwo_Driver.GetComponent<AllForOneMachine>().StartCoroutine("Invincible");
    }

    void Reset_2()
    {
        machineTwo_Driver.GetComponent<AllForOneMachine>().health = 400;
        machineTwo_Driver.SetActive(false);
        machineTwo_Driver.transform.position = machineTwo_StartPosition;
        machineTwo_Driver.transform.rotation = machineOne_StartRotation;

        for (int i = 0; i < randomizedTeamTwo.Count; i++)
        {
            if (randomizedTeamTwo[i] != null)
            {
                randomizedTeamTwo[i].GetComponent<MechController>().voted = false;
            }
        }
        for (int i = 0; i < randomizedTeamTwo.Count; i++)
        {
            if (randomizedTeamTwo[i] != null)
            {
                randomizedTeamTwo[i].GetComponent<MechController>().isDriver = false;
                randomizedTeamTwo[i].GetComponent<MechController>().isGunnerOne = false;
                randomizedTeamTwo[i].GetComponent<MechController>().isGunnerTwo = false;
                randomizedTeamTwo[i].GetComponent<MechController>().isCannon = false;
                randomizedTeamTwo[i].GetComponent<MechController>().voted = false;
            }

        }
        for (int i = 0; i < randomizedTeamTwo.Count; i++)
        {
            if (randomizedTeamTwo[i] != null)
            {
                randomizedTeamTwo[i].SetActive(true);
            }
        }


        for (int i = 0; i < randomizedTeamTwo.Count; i++)
        {
            if (randomizedTeamTwo[i] != null)
            {
                randomizedTeamTwo[i] = null;
            }
        }
    }

    IEnumerator Lobby()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(0);

       
    }
}
