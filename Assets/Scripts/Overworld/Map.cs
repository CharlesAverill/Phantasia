using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    
    public Transform entry_position;
    
    private PlayerController p;
    
    public bool suppress_map_just_changed;
    
    public bool encounters;
    
    private MusicHandler a_s;
    
    public bool play_music;
    
    void Awake(){
        p = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
        if(suppress_map_just_changed){
            p.map_just_changed = false;
        }
        a_s = GetComponentInChildren<MusicHandler>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AudioSource active = a_s.get_active();
        if(!active.isPlaying && play_music){
            active.enabled = true;
            active.volume = 1f;
            active.Play();
        }
    
        if(suppress_map_just_changed){
            p.map_just_changed = false;
        }
    }
}
