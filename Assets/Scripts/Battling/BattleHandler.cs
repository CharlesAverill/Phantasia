using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class BattleHandler : MonoBehaviour
{
    private Monster[] monsters;
    public PartyMember[] party;
    
    private List<GameObject> battlers;
    
    public CursorController monster_cursor;
    public CursorController menu_cursor;
    public EventSystem es;
    public Canvas c;
    
    public AudioSource battle_music;
    public AudioSource victory_music;
    
    public PartyMember active_party_member;
    
    public bool battle_complete;
    public bool win;
    public bool stalemate;
    
    public float party_average_level(){
        float level = 0;
        float count = 0;
        foreach(PartyMember p in party){
            if(p.HP > 0){
                level += (float)p.level;
                count += 1f;
            }
        }
        return (level/count);
    }
    
     public void remove_from_array<T>(ref T[] arr, int index)
     {
         for (int a = index; a < arr.Length - 1; a++)
         {
             // moving elements downwards, to fill the gap at [index]
             arr[a] = arr[a + 1];
         }
         // finally, let's decrement Array's size by one
         Array.Resize(ref arr, arr.Length - 1);
     }
    
    IEnumerator battle(){
    
        foreach(PartyMember p in party){
            while(!p.done_set_up){
                yield return null;
            }
        }
    
        battlers = new List<GameObject>();
        
        foreach(Monster m in monsters){
            battlers.Add(m.gameObject);
        }
        
        foreach(PartyMember p in party){
            battlers.Add(p.gameObject);
        }
        
        while(!battle_complete){
            //Party selection
            foreach(PartyMember p in party){
                if(p.HP > 0){
                    active_party_member = p;
                    p.turn();
                    
                    while(p.action == "" || p.target == null){
                        if(p.action == "run")
                            break;
                        yield return null;
                    }
                    if(p.target){
                        Debug.Log(p.name + " " + p.action + " -> " + p.target.name);
                    }
                    else{
                        Debug.Log(p.name + " -> " + p.action);
                    }
                }
            }
            
            monster_cursor.gameObject.SetActive(false);
            menu_cursor.gameObject.SetActive(false);
            
            //Monster selection
            foreach(Monster m in monsters){
                if(m.HP > 0){
                    m.turn();
                    
                    if(m.target == null){
                        continue;
                    }
                    
                    Debug.Log(m.name + " " + m.action + "->" + m.target.name);
                }
            }
            
            //Scheduling
            List<int> schedule = new List<int>();
            
            foreach(Monster m in monsters){
                schedule.Add(schedule.Count);
            }
            int added_party_members = 0;
            foreach(PartyMember p in party){
                schedule.Add(80 + added_party_members);
                added_party_members += 1;
            }
            for(int i = 0; i < 17; i++){
                int idx1 = UnityEngine.Random.Range(0, battlers.Count);
                int idx2 = UnityEngine.Random.Range(0, battlers.Count);
                
                int temp = schedule[idx1];
                schedule[idx1] = schedule[idx2];
                schedule[idx2] = temp;
            }
            
            foreach(PartyMember p in party){
                while(p.is_moving()){
                    yield return null;
                }
            }
            
            int living = 0;
            foreach(Monster m in monsters){
                if(m.HP > 0){
                    living += 1;
                }
                else{
                    m.gameObject.SetActive(false);
                }
            }
    
            if(living == 0){
                stalemate = true;
                break;
            }
            
            //Display battle
            foreach(int x in schedule){
                if(x >= 80){
                    PartyMember p = party[x - 80];
                    
                    if(p.HP <= 0){
                        continue;
                    }
                    
                    if(p.action == "fight"){
                        StartCoroutine(p.show_battle());
                        while(!p.done_showing){
                            yield return null;
                        }
                        
                        while(p.target == null){
                            p.target = monsters[UnityEngine.Random.Range(0, monsters.Length)].gameObject;
                        }
                        
                        p.GetComponent<Battler>().fight(p, p.target.GetComponent<Monster>());
                    }
                    
                    if(p.action == "run"){
                        if(!p.can_run){
                            Debug.Log("Can't run!");
                        }
                        else{
                            int run_seed = UnityEngine.Random.Range(0, p.level + 15);
                            if(p.luck > run_seed){
                                battle_complete = true;
                                stalemate = true;
                                break;
                            }
                            else{
                                Debug.Log("Running was unsuccessful...");
                            }
                        }
                    }
                }
                else{
                    GameObject b = battlers[x];
                    Monster m = b.GetComponent<Monster>();
                    
                    if(m.HP > 0){
                        if(m.action == "fight"){
                            m.GetComponent<Battler>().fight(m, m.target.GetComponent<PartyMember>());
                        }
                        
                        if(m.action == "run"){
                            Debug.Log(m.gameObject.name + " ran away");
                            Destroy(m.gameObject);
                            remove_from_array(ref monsters, x);
                        }
                    }
                }
                //Check if players won
                living = 0;
                foreach(Monster m in monsters){
                    if(m.HP > 0){
                        living += 1;
                    }
                    else{
                        m.gameObject.SetActive(false);
                    }
                }
        
                if(living == 0){
                    if(monsters.Length == 0){
                        stalemate = true;
                    }
                    else{
                        win = true;
                    }
                    break;
                }
            }
            if(stalemate || win){
                break;
            }
        }
        
        if(win){
            battle_music.Stop();
            victory_music.gameObject.SetActive(true);
            
            while(victory_music.time <= victory_music.gameObject.GetComponent<IntroLoop>().loop_start_seconds){
                yield return null;
            }
            
            Debug.Log("You won!");
            
            while(Input.GetAxis("Submit") == 0){
                yield return null;
            }
        }
        
        Destroy(monster_party);
        
        GlobalControl.instance.monster_party = null;
        
        SceneManager.UnloadScene("Battle");
        
    }
    
    public void fight_choose(){
        StartCoroutine(active_party_member.choose_monster("fight"));
    }
    
    public void player_run(){
        active_party_member.action = "run";
        active_party_member.walk_back();
    }
    
    private GameObject monster_party;
    
    // Start is called before the first frame update
    void Awake()
    {
        monster_party = (GameObject)Instantiate(GlobalControl.instance.monster_party, new Vector3(0f, 0f, 1f), Quaternion.identity);
        Debug.Log(monster_party);
        monster_party.SetActive(true);
        
        monster_cursor = monster_party.GetComponentInChildren<CursorController>();
        monster_cursor.event_system = es;
    
        battle_complete = false;
        win = false;
        active_party_member = party[0];
        
        monsters = (from m in monster_cursor.monsters select m.GetComponent<Monster>()).ToArray();
        
        StartCoroutine(battle());
    }

    // Update is called once per frame
    void Update()
    {
        if(monsters.Length == 0){
            battle_complete = true;
        }
    }
}
