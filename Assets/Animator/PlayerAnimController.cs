using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{

    public bool isVictory;
    public bool isDead;

    private Player player;

    private Animator anim; 
    private string victoryID;
    private string deadID;
    private bool vicFlag = false;
    private bool loseFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
        victoryID = "IsVictory";
        deadID = "IsDead";
    }

    // Update is called once per frame
    void Update()
    {
        
        if(player.victory && !vicFlag)
        {
            anim.SetTrigger(victoryID);
            vicFlag= true;
        }

        if (player.lose && !loseFlag)
        {
            anim.SetTrigger(deadID);
            loseFlag= true;
        }
    }
}
