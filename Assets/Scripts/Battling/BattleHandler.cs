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

    public Transform[] party_placement;
    
    private List<GameObject> battlers;
    
    public CursorController monster_cursor;
    public CursorController menu_cursor;
    public EventSystem es;
    public Canvas c;
    
    public MusicHandler battle_music;
    public MusicHandler victory_music;
    
    public PartyMember active_party_member;
    
    public bool battle_complete;
    public bool win;
    public bool lose;
    public bool stalemate;

    private Dictionary<int, int> level_up_chart;

    public Dictionary<int, int> get_level_chart()
    {
        Dictionary<int, int> level_chart = new Dictionary<int, int>();

        level_chart.Add(2, 40);
        level_chart.Add(3, 196);
        level_chart.Add(4, 547);
        level_chart.Add(5, 1171);
        level_chart.Add(6, 2146);
        level_chart.Add(7, 3550);
        level_chart.Add(8, 5461);
        level_chart.Add(9, 7957);
        level_chart.Add(10, 11116);
        level_chart.Add(11, 15016);
        level_chart.Add(12, 19753);
        level_chart.Add(13, 25351);
        level_chart.Add(14, 31942);
        level_chart.Add(15, 39586);
        level_chart.Add(16, 48361);
        level_chart.Add(17, 58345);
        level_chart.Add(18, 69617);
        level_chart.Add(19, 82253);
        level_chart.Add(20, 96332);
        level_chart.Add(21, 111932);
        level_chart.Add(22, 129131);
        level_chart.Add(23, 148008);
        level_chart.Add(24, 168639);
        level_chart.Add(25, 191103);
        level_chart.Add(26, 215479);
        level_chart.Add(27, 241843);
        level_chart.Add(28, 270275);
        level_chart.Add(29, 300851);
        level_chart.Add(30, 333651);
        level_chart.Add(31, 366450);
        level_chart.Add(32, 399250);
        level_chart.Add(33, 432049);
        level_chart.Add(34, 464849);
        level_chart.Add(35, 497648);
        level_chart.Add(36, 530448);
        level_chart.Add(37, 563247);
        level_chart.Add(38, 596047);
        level_chart.Add(39, 628846);
        level_chart.Add(40, 661646);
        level_chart.Add(41, 694445);
        level_chart.Add(42, 727245);
        level_chart.Add(43, 760044);
        level_chart.Add(44, 792844);
        level_chart.Add(45, 825643);
        level_chart.Add(46, 858443);
        level_chart.Add(47, 891242);
        level_chart.Add(48, 924042);
        level_chart.Add(49, 956841);
        level_chart.Add(50, 989641);

        return level_chart;
    }
    
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
                        lose = true;
                        break;
                    }
                    
                    Debug.Log(m.name + " " + m.action + "->" + m.target.name);
                }
            }

            if (lose)
            {
                battle_complete = true;
                break;
            }

            //Scheduling
            Debug.Log("Scheduling...");
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
                    
                    if(p.action == "run") {
                        /*
                        if(!p.can_run){
                            Debug.Log("Can't run!");
                        }
                        else{
                        */
                        int run_seed = UnityEngine.Random.Range(0, p.level + 15);
                        if(p.luck > run_seed){
                            battle_complete = true;
                            stalemate = true;
                            break;
                        }
                        else{
                            Debug.Log("Running was unsuccessful...");
                        }
                        //}
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

                //Check if players lost
                lose = true;
                foreach (PartyMember p in party)
                {
                    if (p.HP > 0)
                        lose = false;
                }

                if (win || lose || stalemate)
                {
                    battle_complete = true;
                    break;
                }
            }

            
        }
        
        if(win)
        {

            battle_music.get_active().Stop();
            victory_music.gameObject.SetActive(true);
            victory_music.get_active().Play();
            
            while(victory_music.get_active().time <= victory_music.get_active().gameObject.GetComponent<IntroLoop>().loop_start_seconds){
                yield return null;
            }
            
            Debug.Log("You won!");

            foreach (PartyMember p in party)
            {
                p.save_player();
                Destroy(p.gameObject);
            }

            while (Input.GetAxis("Submit") == 0){
                yield return null;
            }
        }
        else if (lose)
        {
                Debug.Log("Game over!");
                SceneManager.LoadSceneAsync("Title Screen");
                SceneManager.UnloadScene("Overworld");
        }
        else if (stalemate)
        {
            foreach (PartyMember p in party)
            {
                p.save_player();
                Destroy(p.gameObject);
            }
        }

        SaveSystem.SaveToDisk();

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

    public void load_party()
    {
        for(int i = 0; i < 4; i++)
        {
            string player_n = "player" + (i + 1) + "_";
            string job = SaveSystem.GetString("player" + (i + 1) + "_class");
            Debug.Log(job);
            switch (job)
            {
                case "fighter":
                    party[i] = Instantiate(Resources.Load<GameObject>("party/fighter"), party_placement[i].position, Quaternion.identity).GetComponent<PartyMember>();
                    break;
                case "black_belt":
                    party[i] = Instantiate(Resources.Load<GameObject>("party/black_belt"), party_placement[i].position, Quaternion.identity).GetComponent<PartyMember>();
                    break;
                case "red_mage":
                    party[i] = Instantiate(Resources.Load<GameObject>("party/red_mage"), party_placement[i].position, Quaternion.identity).GetComponent<PartyMember>();
                    break;
                case "thief":
                    party[i] = Instantiate(Resources.Load<GameObject>("party/thief"), party_placement[i].position, Quaternion.identity).GetComponent<PartyMember>();
                    break;
                case "white_mage":
                    party[i] = Instantiate(Resources.Load<GameObject>("party/white_mage"), party_placement[i].position, Quaternion.identity).GetComponent<PartyMember>();
                    break;
                case "black_mage":
                    party[i] = Instantiate(Resources.Load<GameObject>("party/black_mage"), party_placement[i].position, Quaternion.identity).GetComponent<PartyMember>();
                    break;
            }

            party[i].gameObject.name = SaveSystem.GetString(player_n + "name");
            party[i].bh = this;

            party[i].strength = SaveSystem.GetInt(player_n + "strength");
            party[i].agility = SaveSystem.GetInt(player_n + "agility");
            party[i].intelligence = SaveSystem.GetInt(player_n + "intelligence");
            party[i].vitality = SaveSystem.GetInt(player_n + "vitality");
            party[i].luck = SaveSystem.GetInt(player_n + "luck");
            party[i].HP = SaveSystem.GetInt(player_n + "HP");
            party[i].hit = SaveSystem.GetFloat(player_n + "hit_percent");
            party[i].magic_defense = SaveSystem.GetFloat(player_n + "magic_defense");

            if(SaveSystem.GetBool(player_n + "poison"))
            {
                party[i].conditions.Add("poison");
            }
            if (SaveSystem.GetBool(player_n + "stone"))
            {
                party[i].conditions.Add("stone");
            }
        }
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        load_party();

        active_party_member = party[0];

        level_up_chart = get_level_chart();
        
        monster_party = (GameObject)Instantiate(GlobalControl.instance.monster_party, new Vector3(0f, 0f, 1f), Quaternion.identity);
        Debug.Log(monster_party);
        monster_party.SetActive(true);
        
        monster_cursor = monster_party.GetComponentInChildren<CursorController>();
        monster_cursor.event_system = es;
    
        battle_complete = false;
        win = false;
        lose = false;
        
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
