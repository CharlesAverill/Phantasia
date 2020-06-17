using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldGrid : MonoBehaviour
{
    
    [System.Serializable]
    public class MonsterEncounter{
        public GameObject monster_party;
        public float encounter_rate;
    }
    
    public MonsterEncounter[] monster_encounters;
    
    public GameObject get_monster_party(){
        int index = Random.Range(0, 99);
        
        List<float> bounds = new List<float>();
        bounds.Add(0f);
        
        float highest = 0f;
        
        for(int i = 1; i < monster_encounters.Length; i++){
            MonsterEncounter current = monster_encounters[i - 1];
            MonsterEncounter next = monster_encounters[i];
            
            float first_rate = (current.encounter_rate * 100f) + highest;
            float second_rate = (current.encounter_rate * 100f) + first_rate;
            highest = first_rate;
            
            if(index <= first_rate){
                return current.monster_party;
            }
            
            if(index >= first_rate && index <= second_rate){
                return next.monster_party;
            }
            
            highest += current.encounter_rate;
        }
        
        Debug.Log("broken");
        return monster_encounters[0].monster_party;
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
