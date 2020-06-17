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
        old.SetActive(true);
        updated.SetActive(false);
        if(start_with_update){
            old.SetActive(false);
            updated.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void UpdateMap(){
        old.SetActive(false);
        updated.SetActive(true);
    }
}
