using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DimensionManager : MonoBehaviour
{
    public static DimensionManager manager;
    public float dimensionTimer;
    public Text dimensionTime;
    public enum Dimension {Stone, Lava};
    public Dimension dimension;

    public GameObject Boulder;
    public GameObject stoneShard;

    public List<GameObject> leftSideBoulders = new List<GameObject>();
    public int minNum;
    public int maxNum;

    public List<GameObject> teamOne_Stone = new List<GameObject>();

    public List<GameObject> teamTwo_Stone = new List<GameObject>();

    public List<GameObject> teamOne_Lava = new List<GameObject>();

    public List<GameObject> teamTwo_Lava = new List<GameObject>();

    public GameObject smallAI;
    public List<GameObject> aiTeamOneSpawns = new List<GameObject>();
    public List<GameObject> aiTeamTwoSpawns = new List<GameObject>();

    public int previousNum;
    public int previous_RedAI;
    public int previous_BlueAI;
    public List<ParticleSystem> particles = new List<ParticleSystem>();

    public static int MaxAI_RedTeam = 1;
    public static int MaxAI_BlueTeam = 1;

    public bool bossSpawned;
    public float transition_Timer;
    public GameObject image;

    public GameObject fireflies;

    //AI Materials
    public Material red_AI_Stone;
    public Material blue_AI_Stone;

    public Material red_AI_Lava;
    public Material blue_AI_Lava;

    public GameObject boss_Stone;
    public GameObject boss_Lava;
    public Transform bossSpawn;
    public List<GameObject> players = new List<GameObject>();
    public static List<GameObject> garbage = new List<GameObject>();
    public static float total_BlueTeamDamage;
    public static float total_RedTeamDamage;

    public Material lava;
    public Material mountain;
    public GameObject plain;
    public GameObject mountains;
    public GameObject volcanos;
    public List<GameObject> arenaMountains = new List<GameObject>();
    public List<GameObject> arenaVolcanos = new List<GameObject>();

    public List<ParticleSystem> flameThrowers = new List<ParticleSystem>();
    public List<ParticleSystem> flames = new List<ParticleSystem>();

    public static bool switchingDimension;
    public GameObject flare;
    public static int Red_AI;
    public static int Blue_AI;
    public static int waves = 0;

    public bool tutorial;

    public GameObject superMech_Blue;
    public GameObject superMech_Red;

    public List<ParticleSystem> smallAILeft_Particles = new List<ParticleSystem>();
    public List<ParticleSystem> smallAIRight_Particles = new List<ParticleSystem>();

    public ParticleSystem boss_Particle;

    public Text timer;
    public Text timer1;
    public Text timer2;

    public GameObject drone_One;
    public GameObject drone_Two;
    public GameObject drone_Three;
    public GameObject drone_Four;
    public GameObject drone_Five;

    public AudioClip flameThrower;
    // Start is called before the first frame update
    void Start()
    {
        MaxAI_BlueTeam = 1;
        MaxAI_RedTeam = 1;
        waves = 0;
        Red_AI = 0;
        Blue_AI = 0;
        float randNum = Random.Range(0, 2);
        switch (randNum)
        {
            case 0:
                dimension = Dimension.Stone;
                break;
            case 1:
                dimension = Dimension.Lava;
                break;

        }

        DimensionShift();
        

    }

    // Update is called once per frame
    void Update()
    {
        if(dimensionTimer > 20)
        {
            dimensionTime.gameObject.SetActive(true);
            dimensionTime.text = dimensionTimer.ToString("f0");
            timer1.gameObject.SetActive(false);
            timer2.gameObject.SetActive(false);
        }

        if(dimensionTimer <= 20)
        {
            dimensionTime.gameObject.SetActive(false);
            timer1.text = dimensionTimer.ToString("f0");
            timer1.gameObject.SetActive(true);
        }
        if(dimensionTimer <= 10)
        {
            timer1.gameObject.SetActive(false);
            timer2.gameObject.SetActive(true);
            timer2.text = dimensionTimer.ToString("f0");
        }
        if (!bossSpawned)
        {
            dimensionTimer -= Time.deltaTime;
        }
      
        if(dimensionTimer <= 0 && !bossSpawned)
        {
            bossSpawned = true;
            boss_Particle.Play();
            if (dimension == Dimension.Stone)
            {
               boss_Stone.SetActive(true);
               boss_Stone.GetComponent<Boss>().InvokeRepeating("Attack_360", 0.5f, 0.8f);
               CancelInvoke("StoneDimension");
               CancelInvoke("AI");
            }
            if (dimension == Dimension.Lava)
            {
                boss_Lava.SetActive(true);
                boss_Lava.GetComponent<Boss>().source.clip = flameThrower;
                boss_Lava.GetComponent<Boss>().InvokeRepeating("Attack_360", 0.5f, 0.8f);
                CancelInvoke("AI");
                CancelInvoke("LavaEruption");
                for (int i = 0; i < 11; i++)
                {
                    flameThrowers[i].Stop();
                    flameThrowers[i].gameObject.GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
        if (boss_Stone != null)
        {
            if (boss_Stone.GetComponent<Boss>().health <= 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (players[i] != null)
                        players[i].GetComponent<Rigidbody>().isKinematic = true;
                }

                image.SetActive(true);
                boss_Stone.SetActive(false);
                boss_Stone.transform.position = bossSpawn.position;
                boss_Stone.transform.rotation = bossSpawn.rotation;
                boss_Stone.GetComponent<Boss>().CancelInvoke("Attack_360");
                for (int i = 0; i < 8; i++)
                {
                    boss_Stone.GetComponent<Boss>().flameThrowers[i].SetActive(false);
                }

                boss_Stone.GetComponent<Boss>().health = 300;
                if (total_BlueTeamDamage == total_RedTeamDamage)
                {
                    TeamManager.TeamOneMeter += 50;
                    TeamManager.TeamTwoMeter += 50;
                    total_BlueTeamDamage = 0;
                    total_RedTeamDamage = 0;
              
                }

                if (total_BlueTeamDamage > total_RedTeamDamage)
                {
                    TeamManager.TeamOneMeter += 50;
                    total_BlueTeamDamage = 0;
                    total_RedTeamDamage = 0;
                }
                if (total_BlueTeamDamage < total_RedTeamDamage)
                {
                    TeamManager.TeamTwoMeter += 50;
                    total_BlueTeamDamage = 0;
                    total_RedTeamDamage = 0;
                }
                switchingDimension = true;
                waves = 0;
                Red_AI = 0;
                Blue_AI = 0;
                MaxAI_BlueTeam = 1;
                MaxAI_RedTeam = 1;
                switch (dimension)
                {
                    case Dimension.Stone:
                        dimension = Dimension.Lava;
                        //DimensionShift();
                        break;
                    case Dimension.Lava:
                        dimension = Dimension.Stone;
                        //DimensionShift();
                        break;
                }
            }
        }

        if (boss_Lava != null)
        {
            if (boss_Lava.GetComponent<Boss>().health <= 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (players[i] != null)
                        players[i].GetComponent<Rigidbody>().isKinematic = true;
                }

                image.SetActive(true);
                boss_Lava.SetActive(false);
                boss_Lava.transform.position = bossSpawn.position;
                boss_Lava.transform.rotation = bossSpawn.rotation;
                boss_Lava.GetComponent<Boss>().CancelInvoke("Attack_360");
                boss_Lava.GetComponent<Boss>().source.clip = null;
                for (int i = 0; i < 8; i++)
                {
                    boss_Stone.GetComponent<Boss>().flameThrowers[i].SetActive(false);
                }

                boss_Lava.GetComponent<Boss>().health = 300;
                if (total_BlueTeamDamage == total_RedTeamDamage)
                {
                    TeamManager.TeamOneMeter += 50;
                    TeamManager.TeamTwoMeter += 50;
                    total_BlueTeamDamage = 0;
                    total_RedTeamDamage = 0;
                  
                }

                if (total_BlueTeamDamage > total_RedTeamDamage)
                {
                    TeamManager.TeamOneMeter += 50;
                    total_BlueTeamDamage = 0;
                    total_RedTeamDamage = 0;
                    
                }
                if (total_BlueTeamDamage < total_RedTeamDamage)
                {
                    TeamManager.TeamTwoMeter += 50;
                    total_BlueTeamDamage = 0;
                    total_RedTeamDamage = 0;
                    
                }
                switchingDimension = true;
                waves = 0;
                Red_AI = 0;
                Blue_AI = 0;
                MaxAI_BlueTeam = 1;
                MaxAI_RedTeam = 1;
                
                switch (dimension)
                {
                    case Dimension.Stone:
                        dimension = Dimension.Lava;
                        //DimensionShift();
                        break;
                    case Dimension.Lava:
                        dimension = Dimension.Stone;
                        //DimensionShift();
                        break;
                }
            }
        }




        if (transition_Timer > 0 && switchingDimension)
        {
            for (int i = 0; i < garbage.Count; i++)
            {
                Destroy(garbage[i]);
            }
     
            transition_Timer -= Time.deltaTime;
            flare.SetActive(true);
            timer.gameObject.SetActive(false);
            timer2.gameObject.SetActive(false);
            drone_One.gameObject.SetActive(false);
            drone_Two.gameObject.SetActive(false);
            drone_Three.gameObject.SetActive(false);
            drone_Four.gameObject.SetActive(false);
            drone_Five.gameObject.SetActive(false);

        }
        if(transition_Timer <= 0)
        {

            switchingDimension = false;
            for (int i = 0; i < 6; i++)
            {
                if (players[i] != null)
                    players[i].GetComponent<Rigidbody>().isKinematic = false;
            }
            drone_One.gameObject.SetActive(true);
            drone_Two.gameObject.SetActive(true);
            drone_Three.gameObject.SetActive(true);
            drone_Four.gameObject.SetActive(true);
            image.SetActive(false);
            flare.SetActive(false);
            dimensionTimer = 50;
            bossSpawned = false;
            transition_Timer = 10;
            timer.gameObject.SetActive(true);
            DimensionShift();
            //InvokeRepeating("AI", 0.1f, 15f);
        }

    }

    void DimensionShift()
    {
        //Checks to see which dimension is currently active
        if (dimension == Dimension.Stone)
        {
            fireflies.SetActive(true);
            InvokeRepeating("StoneDimension",5f,1f);
            InvokeRepeating("AI", 10f, 10f);
            for(int i = 0; i < 11; i++)
            {
                flames[i].gameObject.SetActive(false);
            }
            for(int i = 0; i < 8;i++)
            {
                arenaMountains[i].SetActive(true);
                arenaVolcanos[i].SetActive(false);
            }
            mountains.SetActive(true);
            volcanos.SetActive(false);
            plain.GetComponent<MeshRenderer>().material = mountain;
            return;
        }
        if(dimension == Dimension.Lava)
        {
            fireflies.SetActive(false);
            InvokeRepeating("AI", 10f, 10f);
            InvokeRepeating("LavaEruption", 2f, 8f);
            for (int i = 0; i < 11; i++)
            {
                flames[i].gameObject.SetActive(true);

            }
            for (int i = 0; i < 8; i++)
            {
                arenaMountains[i].SetActive(false);
                arenaVolcanos[i].SetActive(true);
            }
            mountains.SetActive(false);
            volcanos.SetActive(true);
            plain.GetComponent<MeshRenderer>().material = lava;
            return;
        }
    }

    //Stone Dimension
    public void StoneDimension()
    {
        //Spawn Boulders
        int randNum = Random.Range(minNum,maxNum);
        int specialNum = Random.Range(0, 101);
        if (randNum == previousNum)
        {
            StoneDimension();
        }
        else
        {
            GameObject boulderSpawn = leftSideBoulders[randNum];
            GameObject boulderInstance = (GameObject)Instantiate(Boulder, boulderSpawn.transform.position, boulderSpawn.transform.rotation);
            particles[randNum].Emit(3);
            previousNum = randNum;
            if (randNum > 2)
            {
                boulderInstance.GetComponent<Boulder>().left = false;
                if(specialNum >= 90 && specialNum <= 100)
                {
                    boulderInstance.gameObject.tag = "SpecialHazard";
                    boulderInstance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                   
                }
            }
            else if (randNum <= 2)
            {
                boulderInstance.GetComponent<Boulder>().left = true;
                if (specialNum >= 90 && specialNum <= 100)
                {
                    boulderInstance.gameObject.tag = "SpecialHazard";
                    boulderInstance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            garbage.Add(boulderInstance);
        }

    }

    public void LavaEruption()
    {
        for(int i = 0; i < flameThrowers.Count - 1;i++)
        {
            flameThrowers[i].GetComponent<BoxCollider>().enabled = false;
            flameThrowers[i].Stop();
        }
        for (int i = 0; i < flameThrowers.Count - 1; i++)
        {
            int randNum = Random.Range(0, 101);
            if (randNum >= 0 && randNum <= 50)
            {
                int rand = Random.Range(0, 101);
                if(rand >= 90 && rand <= 100)
                {
                    ParticleSystem ps = flameThrowers[i].GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule ma = ps.main;
                    ma.startColor = Color.red;
                    flameThrowers[i].GetComponent<BoxCollider>().enabled = true;
                    flameThrowers[i].Play();
                    flameThrowers[i].gameObject.tag = "SpecialFlame";
                }
                else
                {
                    ParticleSystem ps = flameThrowers[i].GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule ma = ps.main;
                    ma.startColor = Color.white;
                    flameThrowers[i].GetComponent<BoxCollider>().enabled = true;
                    flameThrowers[i].Play();
                    flameThrowers[i].gameObject.tag = "Untagged";
                }
            }
        }
    }

    public void AI()
    {



        if (waves < 3)
        {
            waves++;
        }

        if(waves == 1)
        {
            
            MaxAI_RedTeam = 1;
            MaxAI_BlueTeam = 1;
            Red_AI++;
            Blue_AI++;
        }
        
        if (waves == 2)
        {
            MaxAI_RedTeam = 2;
            MaxAI_BlueTeam = 2;
            Red_AI += 2;
            Blue_AI += 2;
        }

        if(waves == 3)
        {
            if (Red_AI == 3)
            {
                MaxAI_RedTeam = 0;
            }
            if(Red_AI == 2)
            {
                MaxAI_RedTeam = 1;
                Red_AI++;
            }
            if (Red_AI == 1)
            {
                MaxAI_RedTeam = 2;
                Red_AI += 2;
            }
            if(Red_AI == 0)
            {
                MaxAI_RedTeam = 3;
                Red_AI += 3;
            }

            if (Blue_AI == 3)
            {
                MaxAI_BlueTeam = 0;
            }
            if (Blue_AI == 2)
            {
                MaxAI_BlueTeam = 1;
                Blue_AI++;
            }
            if (Blue_AI == 1)
            {
                MaxAI_BlueTeam = 2;
                Blue_AI += 2;
            }
            if (Blue_AI == 0)
            {
                MaxAI_BlueTeam = 3;
                Blue_AI += 3;
            }
        }
        if (dimension == Dimension.Stone)
        {
            if (MaxAI_RedTeam < 3)
            {
                for (int i = 0; i < MaxAI_RedTeam; i++)
                {

                    GameObject aiSpawnOne = (GameObject)Instantiate(smallAI, teamOne_Stone[i].transform.position, teamOne_Stone[i].transform.rotation);
                    smallAIRight_Particles[i].Play();

                    aiSpawnOne.GetComponent<SmallAI>().renderer.material = red_AI_Stone;
                    aiSpawnOne.gameObject.tag = "AIRed";
                    aiSpawnOne.GetComponent<SmallAI>().teamNumber = 1;
                    aiSpawnOne.GetComponent<SmallAI>().material = red_AI_Stone;
                    aiSpawnOne.GetComponent<SmallAI>().superMech_Blue = superMech_Blue;

                    if (dimension == Dimension.Stone)
                    {
                        aiSpawnOne.GetComponent<SmallAI>().armor = 10;
                        aiSpawnOne.GetComponent<SmallAI>().attackDamage = 5;
                    }

                    if (dimension == Dimension.Lava)
                    {
                        aiSpawnOne.GetComponent<SmallAI>().armor = 5;
                        aiSpawnOne.GetComponent<SmallAI>().attackDamage = 10;
                    }
                    garbage.Add(aiSpawnOne);


                }
            }


            if (MaxAI_BlueTeam < 3)
            {
                for (int i = 0; i < MaxAI_BlueTeam; i++)
                {

                    GameObject aiSpawnOne = (GameObject)Instantiate(smallAI, teamTwo_Stone[i].transform.position, teamTwo_Stone[i].transform.rotation);
                    smallAILeft_Particles[i].Play();
                    aiSpawnOne.GetComponent<SmallAI>().renderer.material = blue_AI_Stone;
                    aiSpawnOne.gameObject.tag = "AIBlue";
                    aiSpawnOne.GetComponent<SmallAI>().teamNumber = 2;
                    aiSpawnOne.GetComponent<SmallAI>().material = blue_AI_Stone;
                    aiSpawnOne.GetComponent<SmallAI>().superMech_Red = superMech_Red;
                    if (dimension == Dimension.Stone)
                    {
                        aiSpawnOne.GetComponent<SmallAI>().armor = 20;
                        aiSpawnOne.GetComponent<SmallAI>().attackDamage = 5;
                    }
                    if (dimension == Dimension.Lava)
                    {
                        aiSpawnOne.GetComponent<SmallAI>().armor = 5;
                        aiSpawnOne.GetComponent<SmallAI>().attackDamage = 20;
                    }

                    garbage.Add(aiSpawnOne);
                }
            }
        }

        if (dimension == Dimension.Lava)
        {
            if (MaxAI_RedTeam < 3)
            {
                for (int i = 0; i < MaxAI_RedTeam; i++)
                {

                    GameObject aiSpawnOne = (GameObject)Instantiate(smallAI, teamOne_Lava[i].transform.position, teamOne_Lava[i].transform.rotation);
                    smallAIRight_Particles[i].Play();
                    aiSpawnOne.GetComponent<SmallAI>().renderer.material = red_AI_Lava;
                    aiSpawnOne.gameObject.tag = "AIRed";
                    aiSpawnOne.GetComponent<SmallAI>().teamNumber = 1;
                    aiSpawnOne.GetComponent<SmallAI>().material = red_AI_Lava;
                    aiSpawnOne.GetComponent<SmallAI>().superMech_Blue = superMech_Blue;

                    if (dimension == Dimension.Stone)
                    {
                        aiSpawnOne.GetComponent<SmallAI>().armor = 20;
                        aiSpawnOne.GetComponent<SmallAI>().attackDamage = 5;
                    }

                    if (dimension == Dimension.Lava)
                    {
                        aiSpawnOne.GetComponent<SmallAI>().armor = 5;
                        aiSpawnOne.GetComponent<SmallAI>().attackDamage = 20;
                    }
                    garbage.Add(aiSpawnOne);


                }
            }


            if (MaxAI_BlueTeam < 3)
            {
                for (int i = 0; i < MaxAI_BlueTeam; i++)
                {

                    GameObject aiSpawnOne = (GameObject)Instantiate(smallAI, teamTwo_Lava[i].transform.position, teamTwo_Lava[i].transform.rotation);
                    smallAILeft_Particles[i].Play();
                    aiSpawnOne.GetComponent<SmallAI>().renderer.material = blue_AI_Lava;
                    aiSpawnOne.gameObject.tag = "AIBlue";
                    aiSpawnOne.GetComponent<SmallAI>().teamNumber = 2;
                    aiSpawnOne.GetComponent<SmallAI>().material = blue_AI_Lava;
                    aiSpawnOne.GetComponent<SmallAI>().superMech_Red = superMech_Red;
                    if (dimension == Dimension.Stone)
                    {
                        aiSpawnOne.GetComponent<SmallAI>().armor = 20;
                        aiSpawnOne.GetComponent<SmallAI>().attackDamage = 10;
                    }
                    if (dimension == Dimension.Lava)
                    {
                        aiSpawnOne.GetComponent<SmallAI>().armor = 10;
                        aiSpawnOne.GetComponent<SmallAI>().attackDamage = 20;
                    }

                    garbage.Add(aiSpawnOne);
                }
            }
        }
    }
  
    public void StoneShard(GameObject spawnPos, int teamNum, Transform direction, float attackDamage)
    {
        if (!tutorial)
        {
            if (teamNum == 1)
            {
                GameObject stoneShardInstance = (GameObject)Instantiate(stoneShard, spawnPos.transform.position, direction.rotation);
                stoneShardInstance.GetComponent<StoneShard>().teamNumber = teamNum;
                stoneShardInstance.GetComponent<StoneShard>().attackDamage = attackDamage;
                stoneShardInstance.GetComponent<Renderer>().sharedMaterial.color = Color.blue;
                stoneShardInstance.tag = "TeamOneStoneShard";
                garbage.Add(stoneShardInstance);
            }
            if (teamNum == 2)
            {
                GameObject stoneShardInstance = (GameObject)Instantiate(stoneShard, spawnPos.transform.position, direction.rotation);
                stoneShardInstance.GetComponent<StoneShard>().teamNumber = teamNum;
                stoneShardInstance.GetComponent<StoneShard>().attackDamage = attackDamage;
                stoneShardInstance.GetComponent<Renderer>().sharedMaterial.color = Color.red;
                stoneShardInstance.tag = "TeamTwoStoneShard";
                garbage.Add(stoneShardInstance);
            }
        }
    }

    //Stone Dimension

   public void Garbage()
    {
        for (int i = 0; i < garbage.Count; i++)
        {
            Destroy(garbage[i]);
        }
    }

}
