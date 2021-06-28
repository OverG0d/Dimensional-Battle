using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{


    public List<GameObject> players = new List<GameObject>();
    public Text blueTeam;
    public Text redTeam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        blueTeam.text = "" + TeamManager.blue_TotalDamage;
        redTeam.text = "" + TeamManager.red_TotalDamage;


    }
}
