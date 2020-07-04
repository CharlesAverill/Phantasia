using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Battler : MonoBehaviour
{
    public int HP;
    public float hit;
    public float magic_defense;
    public List<String> conditions;

    Equips equip;

    public int fight(Monster attack, PartyMember defend){

        if(equip == null)
        {
            equip = new Equips();
        }
    
        string[] conditions_array = conditions.ToArray();
    
        float damage_rating = UnityEngine.Random.Range((float)attack.damage_low, (float)attack.damage_high);
        
        int absorb_rating = 0;
        if(defend.job == "black_belt" || defend.job == "master"){
            absorb_rating = defend.level;
        }
        else{
            absorb_rating = equip.sum_armor(defend.index);
        }
        
        float damage = 0f;
        
        if((int)UnityEngine.Random.Range(0f, 100f) <= (int)(100f * attack.crit_percent)){
            Debug.Log("Critical hit");
            float range = UnityEngine.Random.Range(damage_rating, 2f * damage_rating);
            damage = range + range - (float)absorb_rating;
        }
        else{
            damage = UnityEngine.Random.Range(damage_rating, 2f * damage_rating) - (float)absorb_rating;
        }
        
        float chance_to_hit = 168f + attack.hit - (48 + defend.agility);
        
        if(Array.Exists(conditions_array, condition => condition=="blind")){
            chance_to_hit -= 40f;
        }
        if(Array.Exists(conditions_array, condition => condition=="blind")){
            chance_to_hit += 40f;
        }
        
        if(damage < 0f){
            damage = 1f;
        }
        
        if(UnityEngine.Random.Range(0f, 200f) <= chance_to_hit){
            defend.HP -= (int)damage;

            if (defend.HP < 0)
                defend.HP = 0;

            return (int)damage;
        }
        else{
            return -1;
        }
    }
    
    public int fight(PartyMember attack, Monster defend){

        if (equip == null)
        {
            equip = new Equips();
        }

        string[] conditions_array = conditions.ToArray();
    
        float damage_rating = 0;
        if(attack.job == "black_mage" || attack.job == "black_wizard"){
            damage_rating = (float)(attack.strength / 2) + 1f + (float)weapon_damage(attack.weapon);
        }
        else if(attack.job == "black_belt" || attack.job == "master"){
            if(weapon_damage(attack.weapon) == 0){
                damage_rating = (float)(attack.level * 2);
            }
            else{
                damage_rating = (float)(attack.strength / 2) + 1f + (float)weapon_damage(attack.weapon);
            }
        }
        else{
            damage_rating = (float)(attack.strength / 2) + (float)weapon_damage(attack.weapon);
        }

        float damage = 0f;
        
        float chance_to_hit = 168f + attack.hit - defend.evade;
        
        if(Array.Exists(conditions_array, condition => condition=="blind")){
            chance_to_hit -= 40f;
        }
        
        if(damage < 1f){
            damage = 1f;
        }

        float crit_hit_chance = UnityEngine.Random.Range(0f, 200f);


        if (crit_hit_chance <= chance_to_hit){

            bool crit = false;

            if (UnityEngine.Random.Range(0f, 100f) <= weapon_crit(attack.weapon))
            {
                float range = UnityEngine.Random.Range(damage_rating, 2f * damage_rating);
                damage = range + range - (float)defend.absorb;
                crit = true;
            }
            else
            {
                damage = UnityEngine.Random.Range(damage_rating, 2f * damage_rating) - (float)defend.absorb;
            }

            if (damage < 1f)
            {
                damage = 1f;
            }

            defend.HP -= (int)damage;

            if (defend.HP < 0)
                defend.HP = 0;

            if (crit)
            {
                return -(int)damage;
            }

            return (int)damage;
        }
        else{
            return -9999999;
        }
    }
    
    int weapon_damage(string weapon){
        if (weapon == "")
            return 0;
        return equip.get_weapon(weapon).damage;
    }
    
    float weapon_crit(string weapon){
        if (weapon == "")
            return 0f;
        return equip.get_weapon(weapon).crit;
    }
    
    string weapon_type(PartyMember member){
        return equip.get_weapon(SaveSystem.GetString("player" + (member.index + 1) + "_weapon")).element;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        equip = new Equips();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
