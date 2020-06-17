using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OverworldUpdate : MonoBehaviour
{
    
    public GameObject old;
    public GameObject updated;
    
    public bool start_with_update;
    
    // Start is called before the first frame update
    void Start()
    {
        date_map();
        if(start_with_update){
            update_map();
        }
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
