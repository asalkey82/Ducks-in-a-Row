using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public static int hearts = 3;
    public TMP_Text lives;

    public static int ducklings;
    public TMP_Text squad;

    void Start()
    {
        for(int i = 0; i <hearts; i++)
        {
            lives.text += "<sprite name=Heart (24x24)>";
        }
    }

    // Update is called once per frame
    void Update()
    {
        lives.text = "";
        for(int i = 0; i <hearts; i++)
        {
            lives.text += "<sprite name=Heart (24x24)>";
        }
        squad.text = "Following Ducklings = " + ducklings;
    }
}
