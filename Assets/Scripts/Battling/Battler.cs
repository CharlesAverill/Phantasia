using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Battler : MonoBehaviour
{
    public int HP;
    public float hit;
    public float magic_defense;
    public string[] conditions;
    
    public void fight(Monster attack, PartyMember defend){
        float damage_rating = UnityEngine.Random.Range((float)attack.damage_low, (float)attack.damage_high);
        
        int absorb_rating = 0;
        if(defend.job == "black_belt" || defend.job == "master"){
            absorb_rating = defend.level;
        }
        else{
            absorb_rating = sum_armor(defend);
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
        
        if(Array.Exists(conditions, condition => condition=="blind")){
            chance_to_hit -= 40f;
        }
        if(Array.Exists(conditions, condition => condition=="blind")){
            chance_to_hit += 40f;
        }
        
        if(damage < 0f){
            damage = 1f;
        }
        
        if(UnityEngine.Random.Range(0f, 200f) <= chance_to_hit){
            Debug.Log(attack.name + " does " + (int)damage + " damage to " + defend.name);
            defend.HP -= (int)damage;
        }
        else{
            Debug.Log("Missed");
        }
    }
    
    public void fight(PartyMember attack, Monster defend){
    
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
        
        if((int)UnityEngine.Random.Range(0f, 100f) <= (int)(100f * weapon_crit(attack.weapon))){
            float range = UnityEngine.Random.Range(damage_rating, 2f * damage_rating);
            damage = range + range - (float)defend.absorb;
        }
        else{
            damage = UnityEngine.Random.Range(damage_rating, 2f * damage_rating) - (float)defend.absorb;
        }
        
        float chance_to_hit = 168f + attack.hit - defend.evade;
        
        if(Array.Exists(conditions, condition => condition=="blind")){
            chance_to_hit -= 40f;
        }
        if(Array.Exists(conditions, condition => condition=="blind")){
            chance_to_hit += 40f;
        }
        
        if(damage < 0f){
            damage = 1f;
        }
        
        if(UnityEngine.Random.Range(0f, 200f) <= chance_to_hit){
            Debug.Log(attack.name + " does " + (int)damage + " damage to " + defend.name);
            defend.HP -= (int)damage;
        }
        else{
            Debug.Log("Missed");
        }
    }
    
    int weapon_damage(string weapon){
        return 0;
    }
    
    float weapon_crit(string weapon){
        return .01f;
    }
    
    int sum_armor(PartyMember member){
        return 1;
    }
    
    string weapon_type(PartyMember member){
        return "";
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
