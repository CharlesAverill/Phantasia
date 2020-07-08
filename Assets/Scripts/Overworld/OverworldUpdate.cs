using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OverworldUpdate : MonoBehaviour
{

    [Serializable]
    public class FlagCheck
    {
        public string name;
        public bool flagValToShow;
        public GameObject replacementNPC;

        public bool check()
        {
            return SaveSystem.GetBool(name) == flagValToShow;
        }
    }

    public bool need_flags_to_show;
    public FlagCheck[] flags_to_show;

    public GameObject old;
    public GameObject updated;
    
    public bool start_with_update;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        date_map();
        bool update = true;
        if (start_with_update){
            update = true;
        }
        if (need_flags_to_show)
        {
            foreach (FlagCheck fc in flags_to_show)
            {
                if (!fc.check())
                    update = false;
            }
        }
        if (update)
            update_map();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void update_map(){
        old.SetActive(false);
        updated.SetActive(true);
    }
    
    public void date_map(){
        old.SetActive(true);
        updated.SetActive(false);
    }
}
