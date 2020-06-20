using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Battler
{
    
    public int gold;
    public int exp;
    public int absorb;
    public float evade;
    public int damage_low;
    public int damage_high;
    public int multihit;
    public float crit_percent;
    public string[] family;
    public int morale;
    public string attack_status;
    public string attack_element;
    public string[] weaknesses;
    public string[] resistances;
    
    public string[] spells;
    public string[] specials;
    
    public string action;
    public GameObject target;
    
    public BattleHandler bh;
    
    void choose_player(){
        PartyMember[] party = bh.party;

        List<int> tried = new List<int>();

        bool found_target = false;
        int count = 0;
        while (!found_target)
        {
            count += 1;
            int n = Random.Range(1, 8);
            tried.Add(n);

            if (n >= 1 && n <= 4)
            {
                target = party[0].gameObject;
            }
            if (n == 5 || n == 6)
            {
                target = party[1].gameObject;
            }
            if (n == 7)
            {
                target = party[2].gameObject;
            }
            if (n == 8)
            {
                target = party[3].gameObject;
            }
            if(target.GetComponent<PartyMember>().HP > 0)
            {
                found_target = true;
            }

            if(count >= 8)
            {
                target = null;
                break;
            }
        }
    }
    
    public void turn(){
        List<string> actions = new List<string>();
        
        actions.Add("fight");
        
        if(spells.Length > 0){
            actions.Add("magic");
        }
        
        if(specials.Length > 0){
            actions.Add("special");
        }
        
        action = actions[Random.Range(0, actions.Count)];

        PartyMember leader = bh.party[0];
        for (int i = 0; i < bh.party.Length; i++)
        {
            if(bh.party[i].HP > 0)
            {
                leader = bh.party[i];
                break;
            }
        }

        if(morale - (2 * leader.level) + Random.Range(0, 50) < 80){
            action = "run";
        }
        
        if(action == "fight"){
            choose_player();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(!bh){
            bh = FindObjectsOfType<BattleHandler>()[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
