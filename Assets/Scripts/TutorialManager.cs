using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static int numOfPlayersReady;
    public static List<GameObject> garbage = new List<GameObject>();
    public List<MechController> players = new List<MechController>();
    public GameObject gameplay_Menu;
    public GameObject loading_Menu;
    public float timer;
    public bool everyone_Voted;
    public AudioClip loading;
    public AudioSource source;
    public Image image;

    public GameObject[] attackDamage;
    public GameObject[] healthBuff;
    public GameObject[] armorBuff;
    public GameObject[] attackDamage_2;
    public GameObject[] healthBuff_2;
    public GameObject[] armorBuff_2;
    void Start()
    {

        attackDamage = GameObject.FindGameObjectsWithTag("AttackDamage_Buff");
        healthBuff = GameObject.FindGameObjectsWithTag("Health_Increase");
        armorBuff = GameObject.FindGameObjectsWithTag("Armor_Buff");
        attackDamage_2 = GameObject.FindGameObjectsWithTag("AttackDamage_Buff_2");
        healthBuff_2 = GameObject.FindGameObjectsWithTag("Health_Increase_2");
        armorBuff_2 = GameObject.FindGameObjectsWithTag("Armor_Buff_2");
        ClearPowerUps();
        timer = 0;
        everyone_Voted = false;
        numOfPlayersReady = 0;
        gameplay_Menu.gameObject.SetActive(false);
        TeamManager.numofTeamOnePlayers = 3;
        TeamManager.numofTeamTwoPlayers = 3;
        //screen_One.gameObject.SetActive(true);
        //screen_Two.gameObject.SetActive(true);
        for (int i = 0; i < players.Count;i++)
        {
            players[i].ready = false;
        }
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GameOver = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (numOfPlayersReady == 6 && !everyone_Voted)
        { 
            for(int i = 0; i < garbage.Count; i++)
            {
                Destroy(garbage[i]);
            }
            everyone_Voted = true;
            TeamManager.numofTeamOnePlayers = 3;
            TeamManager.numofTeamTwoPlayers = 3;
            loading_Menu.gameObject.SetActive(true);
            //screen_One.gameObject.SetActive(false);
            //screen_Two.gameObject.SetActive(false);
           
            source.clip = loading;
            source.Play();

        }
        image.fillAmount = timer * 0.1f;

        if (timer < 10 && everyone_Voted)
        {
            timer += Time.deltaTime;
            
        }
        if (timer >= 10 && everyone_Voted)
        {
            SceneManager.LoadScene(1);
        }
    }

    void ClearPowerUps()
    {
        for (int i = 0; i < attackDamage.Length; i++)
        {
            Destroy(attackDamage[i]);
        }
        for (int i = 0; i < healthBuff.Length; i++)
        {
            Destroy(healthBuff[i]);
        }
        for (int i = 0; i < armorBuff.Length; i++)
        {
            Destroy(armorBuff[i]);
        }
        for (int i = 0; i < attackDamage_2.Length; i++)
        {
            Destroy(attackDamage_2[i]);
  
        }
        for (int i = 0; i < healthBuff_2.Length; i++)
        {
            Destroy(healthBuff_2[i]);
        }
        for (int i = 0; i < armorBuff_2.Length; i++)
        {
            Destroy(armorBuff_2[i]);
        }
    }



}
