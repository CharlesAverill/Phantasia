using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    
    public PlayerController player;
    public ScreenTransition st;
    public GameObject grids;
    public GameObject active_map;
    
    float overworldX;
    float overworldY;
    
    float submapX;
    float submapY;
    
    public bool done_changing;
    
    // Start is called before the first frame update
    void Start()
    {
        overworldX = SaveSystem.GetFloat("overworldX");
        overworldY = SaveSystem.GetFloat("overworldY");
        
        submapX = SaveSystem.GetFloat("submapX");
        submapY = SaveSystem.GetFloat("submapY");
    
        if(SaveSystem.GetBool("in_submap")){
            Map[] maps = grids.GetComponentsInChildren<Map>(true);
            foreach(Map m in maps){
                if(m.gameObject.name == SaveSystem.GetString("submap_name")){
                    active_map = m.gameObject;
                    break;
                }
            }
            
            player.move_point.position = new Vector3(submapX, submapY, 0f);
            player.gameObject.transform.position = new Vector3(submapX, submapY, 0f);
        }
        else{
            
            player.move_point.position = new Vector3(overworldX, overworldY, 0f);
            player.gameObject.transform.position = new Vector3(overworldX, overworldY, 0f);
        }
    
        deactivate_maps_except(active_map);
        done_changing = true;
        active_map.SetActive(true);
        
        player.reh.encounters = active_map.GetComponent<Map>().encounters;
    }

    // Update is called once per frame
    void Update()
    {
        if(active_map.name == "Overworld"){
            overworldX = player.transform.position.x;
            overworldY = player.transform.position.y;
        }
        else{
            submapX = player.transform.position.x;
            submapY = player.transform.position.y;
        }
    }
    
    public void save_position(){
        SaveSystem.SetFloat("overworldX", overworldX);
        SaveSystem.SetFloat("overworldY", overworldY);
        SaveSystem.SetFloat("submapX", submapX);
        SaveSystem.SetFloat("submapY", submapY);
        
        if(active_map.name == "Overworld"){
            SaveSystem.SetBool("in_submap", false);
        }
        else{
            SaveSystem.SetBool("in_submap", true);
            SaveSystem.SetString("submap_name", active_map.name);
        }
    }
    
    void deactivate_maps_except(GameObject map){
        Map[] maps = grids.GetComponentsInChildren<Map>();
        foreach(Map m in maps){
            if(m.enabled == false){
                continue;
            }
            if(m.gameObject.GetInstanceID() != map.GetInstanceID()){
                m.gameObject.SetActive(false);
            }
        }
        
        active_map = map;
    }
    
    public void change_maps(GameObject map){
        if(done_changing)
            StartCoroutine(change(map));
    }
    
    IEnumerator change(GameObject map){
        done_changing = false;
        
        active_map.GetComponent<AudioSource>().Stop();
        active_map.GetComponent<AudioSource>().volume = 0f;
        
        player.warp_sound.Play();
        
        st.transition();
        while(st.filling){
            yield return null;
        }
        
        deactivate_maps_except(map);
        Map active = active_map.GetComponent<Map>();
        
        if(active.name != "Overworld"){
            player.transform.position = active.entry_position.position;
        }
        else{
            player.transform.position = new Vector3(overworldX, overworldY, 0f);
        }
        
        player.move_point.position = player.transform.position;
        player.map_just_changed = true;
        
        active_map.GetComponent<AudioSource>().playOnAwake = false;
        active_map.SetActive(true);
        active_map.GetComponent<AudioSource>().volume = 0f;
        
        st.unfilling = true;
        
        while(player.warp_sound.isPlaying){
            yield return null;
        }
        
        active_map.GetComponent<AudioSource>().Play();
        active_map.GetComponent<AudioSource>().volume = 1f;
        
        done_changing = true;
    }
    
    GameObject get_map(string map_name){
        Transform[] children = grids.transform.GetComponentsInChildren<Transform>();
        foreach(var child in children){
            if(child.GetComponent<Map>() && child.name == map_name){
                return child.gameObject;
            }
        }
        return null;
    }
}
