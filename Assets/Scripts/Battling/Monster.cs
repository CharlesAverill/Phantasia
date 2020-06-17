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
    public string[] weaknesses;
    public string[] resistances;
    
    public string[] spells;
    public string[] specials;
    
    public string action;
    public GameObject target;
    
    public BattleHandler bh;
    
    void choose_player(){
        PartyMember[] party = bh.party;
        
        choose_party_member:
            int n = Random.Range(1, 8);
            if(n >= 1 && n <= 4){
                target = party[0].gameObject;
                if(party[0].HP <= 0){
                    goto choose_party_member;
                }
            }
            if(n == 5 || n == 6){
                target = party[1].gameObject;
                if(party[1].HP <= 0){
                    goto choose_party_member;
                }
            }
            if(n == 7){
                target = party[2].gameObject;
                if(party[2].HP <= 0){
                    goto choose_party_member;
                }
            }
            if(n == 8){
                target = party[3].gameObject;
                if(party[3].HP <= 0){
                    goto choose_party_member;
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
        
        Debug.Log(bh);
        PartyMember leader = bh.party[0];
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
