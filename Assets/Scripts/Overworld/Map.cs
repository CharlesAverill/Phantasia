using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    
    public Transform entry_position;
    
    private PlayerController p;
    
    public bool suppress_map_just_changed;
    
    public bool encounters;
    
    private AudioSource a_s;
    
    void Awake(){
        p = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
        if(suppress_map_just_changed){
            p.map_just_changed = false;
        }
        a_s = GetComponent<AudioSource>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!a_s.isPlaying){
            a_s.Play();
        }
    
        if(suppress_map_just_changed){
            p.map_just_changed = false;
        }
    }
}
