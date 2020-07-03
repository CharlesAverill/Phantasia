using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : Battler
{
    public int index;

    public int maxHP;
    public string job;
    public string weapon;
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;
    public int luck;
    
    public int experience;
    public int level;
    
    public bool can_run;
    
    public BattleHandler bh;
    
    public Vector3 move_point;
    
    private CursorController monster_cursor;
    private CursorController menu_cursor;

    private BattleSpriteController bsc;

    public WeaponSprite weapon_sprite;
    public MagicSprite magic_sprite;
    
    public string action;
    public GameObject target;

    public List<string> level_up()
    {

        level += 1;

        int seed = 0;

        List<string> stats_increased = new List<string>();

        int s_old = strength;
        int a_old = agility;
        int i_old = intelligence;
        int v_old = vitality;
        int l_old = luck;

        switch(SaveSystem.GetString("player" + (index + 1) + "_class"))
        {
            case "fighter":
                seed = UnityEngine.Random.Range(1, 8);
                if(seed > 0)
                {
                    strength += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 2)
                {
                    agility += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 4)
                {
                    intelligence += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 3)
                {
                    vitality += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 2)
                {
                    luck += 1;
                }

                seed = UnityEngine.Random.Range(1, 100);
                if(seed > 50)
                {
                    maxHP += 20 + (vitality / 4) + UnityEngine.Random.Range(1, 6);
                }
                else
                {
                    maxHP += (vitality / 4) + 1;
                }

                hit += .03f;
                magic_defense += .03f;

                break;
            case "black_belt":
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 3)
                {
                    strength += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 3)
                {
                    agility += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 3)
                {
                    intelligence += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 0)
                {
                    vitality += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 3)
                {
                    luck += 1;
                }

                seed = UnityEngine.Random.Range(1, 100);
                if (seed > 60)
                {
                    maxHP += 20 + (vitality / 4) + UnityEngine.Random.Range(1, 6);
                }
                else
                {
                    maxHP += (vitality / 4) + 1;
                }

                hit += .03f;
                magic_defense += .04f;

                break;
            case "red_mage":
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 3)
                {
                    strength += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 3)
                {
                    agility += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 3)
                {
                    intelligence += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 3)
                {
                    vitality += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 3)
                {
                    luck += 1;
                }

                seed = UnityEngine.Random.Range(1, 100);
                if (seed > 75)
                {
                    maxHP += 20 + (vitality / 4) + UnityEngine.Random.Range(1, 6);
                }
                else
                {
                    maxHP += (vitality / 4) + 1;
                }

                hit += .02f;
                magic_defense += .02f;

                break;
            case "thief":
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 1)
                {
                    strength += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 2)
                {
                    agility += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 4)
                {
                    intelligence += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 4)
                {
                    vitality += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 0)
                {
                    luck += 1;
                }

                seed = UnityEngine.Random.Range(1, 100);
                if (seed > 65)
                {
                    maxHP += 20 + (vitality / 4) + UnityEngine.Random.Range(1, 6);
                }
                else
                {
                    maxHP += (vitality / 4) + 1;
                }

                hit += .02f;
                magic_defense += .02f;

                break;
            case "white_mage":
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 4)
                {
                    strength += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 4)
                {
                    agility += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 3)
                {
                    intelligence += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 4)
                {
                    vitality += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 3)
                {
                    luck += 1;
                }

                seed = UnityEngine.Random.Range(1, 100);
                if (seed > 70)
                {
                    maxHP += 20 + (vitality / 4) + UnityEngine.Random.Range(1, 6);
                }
                else
                {
                    maxHP += (vitality / 4) + 1;
                }

                hit += .01f;
                magic_defense += .02f;

                break;
            case "black_mage":
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 4)
                {
                    strength += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 5)
                {
                    agility += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 0)
                {
                    intelligence += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 4)
                {
                    vitality += 1;
                }
                seed = UnityEngine.Random.Range(1, 8);
                if (seed > 4)
                {
                    luck += 1;
                }

                seed = UnityEngine.Random.Range(1, 100);
                if (seed > 75)
                {
                    maxHP += 20 + (vitality / 4) + UnityEngine.Random.Range(1, 6);
                }
                else
                {
                    maxHP += (vitality / 4) + 1;
                }

                hit += .01f;
                magic_defense += .02f;

                break;
        }

        if (s_old != strength)
            stats_increased.Add("Strength");
        if (a_old != strength)
            stats_increased.Add("Agility");
        if (i_old != strength)
            stats_increased.Add("Intelligence");
        if (v_old != strength)
            stats_increased.Add("Vitality");
        if (l_old != strength)
            stats_increased.Add("Luck");
        stats_increased.Add("HP");

        save_player();

        return stats_increased;
    }
        
    public void save_player(){
        string player_n = "player" + (index + 1) + "_";
        SaveSystem.SetInt(player_n + "strength", strength);
        SaveSystem.SetInt(player_n + "agility", agility);
        SaveSystem.SetInt(player_n + "intelligence", intelligence);
        SaveSystem.SetInt(player_n + "vitality", vitality);
        SaveSystem.SetInt(player_n + "luck", luck);
        SaveSystem.SetFloat(player_n + "hit_percent", hit);
        SaveSystem.SetFloat(player_n + "magic_defense", magic_defense);
        SaveSystem.SetInt(player_n + "HP", HP);
        SaveSystem.SetInt(player_n + "maxHP", maxHP);
        SaveSystem.SetInt(player_n + "exp", experience);
        SaveSystem.SetBool(player_n + "poison", conditions.Contains("poison"));
        SaveSystem.SetBool(player_n + "stone", conditions.Contains("stone"));
    }

    public void load_player()
    {
        string player_n = "player" + (index + 1) + "_";
        strength = SaveSystem.GetInt(player_n + "strength");
        agility = SaveSystem.GetInt(player_n + "agility");
        intelligence = SaveSystem.GetInt(player_n + "intelligence");
        vitality = SaveSystem.GetInt(player_n + "vitality");
        luck = SaveSystem.GetInt(player_n + "luck");
        hit = SaveSystem.GetInt(player_n + "hit_percent");
        magic_defense = SaveSystem.GetInt(player_n + "magic_defense");
        HP = SaveSystem.GetInt(player_n + "HP");
        maxHP = SaveSystem.GetInt(player_n + "maxHP");
        experience = SaveSystem.GetInt(player_n + "exp");
        level = bh.get_level_from_exp(experience);

        if(SaveSystem.GetBool(player_n + "stone"))
        {
            conditions.Add("stone");
        }
        if (SaveSystem.GetBool(player_n + "poison"))
        {
            conditions.Add("poison");
        }
    }
    
    public IEnumerator choose_monster(string act){

        monster_cursor.gameObject.SetActive(true);
        monster_cursor.GetComponent<SpriteRenderer>().enabled = false;
        menu_cursor.gameObject.SetActive(false);
        monster_cursor.GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(.2f);

        while (Input.GetAxis("Submit") == 0){
            yield return null;
        }
        
        target = monster_cursor.get_monster().gameObject;

        bsc.change_state("walk");
        walk_back();
        while(is_moving()){
            yield return null;
        }
        
        action = act;
    }
    
    public void walk_back(){
        move_point = new Vector3(3.66f, transform.position.y, transform.position.z);
    }
    
    public bool is_moving(){
        return transform.position != move_point;
    }
    
    public bool is_playing_animation(){
        return bsc.is_casting || bsc.is_fighting || bsc.is_walking;
    }
    
    public bool done_showing;
    
    public IEnumerator show_battle(){
        done_showing = false;

        bsc.change_state("walk");
        move_point = new Vector3(1.66f, transform.position.y, transform.position.z);
        while(is_moving()){
            yield return null;
        }

        if(action == "fight")
        {
            bsc.change_state("fight", weapon_sprite);
            while (is_playing_animation())
            {
                yield return null;
            }
        }

        bsc.change_state("walk");
        move_point = new Vector3(3.66f, transform.position.y, transform.position.z);
        while(is_moving()){
            yield return null;
        }
        done_showing = true;
    }
    
    private void check_load(){
        if(!monster_cursor)
            monster_cursor = bh.monster_cursor;
        if(!bsc)
            bsc = GetComponent<BattleSpriteController>();
        if(!menu_cursor)
            menu_cursor = bh.menu_cursor;
    }
    
    public void turn(){
        check_load();
    
        action = "";
        target = null;
        
        monster_cursor.gameObject.SetActive(false);

        bsc.change_state("walk");
        
        move_point = new Vector3(1.66f, transform.position.y, transform.position.z);

        menu_cursor.gameObject.SetActive(true);
    }
    
    public bool done_set_up;
    
    // Start is called before the first frame update
    void Start()
    {
        done_set_up = false;
        
        move_point = transform.position;
        monster_cursor = bh.monster_cursor;
        menu_cursor = bh.menu_cursor;
        
        menu_cursor.gameObject.SetActive(true);
        monster_cursor.gameObject.SetActive(false);

        bsc = GetComponent<BattleSpriteController>();

        weapon = SaveSystem.GetString("player" + (index + 1) + "_weapon");
        weapon_sprite.set_sprite(bh.mwsh.get_weapon(weapon));
        
        done_set_up = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0 && bsc.get_state() != "dead")
        {
            bsc.change_state("dead");
        }

        if(transform.position == move_point && bsc.get_state() != "idle")
        {
            bsc.change_state("idle");
        }

        transform.position = Vector3.MoveTowards(transform.position, move_point, 8 * Time.deltaTime);
        /*
        if (timer >= 0)
        {
            timer += Time.deltaTime;
            if (transform.position == move_point)
            {
                stopTimer();
            }
        }
        */
    }

    float timer = -1;
    List<float> times;

    void startTimer()
    {
        if (times == null)
        {
            times = new List<float>();
        }
        timer = 0;
    }

    void stopTimer()
    {
        Debug.Log(timer);
        times.Add(timer);

        float total = 0f;
        foreach (float f in times)
        {
            total += f;
        }
        Debug.Log("Average: " + (total / (float)times.Count));

        timer = -1f;
    }
}
