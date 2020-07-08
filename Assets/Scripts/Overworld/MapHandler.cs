using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    
    public PlayerController player;
    public ScreenTransition st;
    public GameObject grids;
    public GameObject active_map;
    
    public float overworldX;
    public float overworldY;
    
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
        active_map.GetComponent<Map>().play_music = true;
        
        player.reh.encounters = active_map.GetComponent<Map>().encounters;
    }

    // Update is called once per frame
    void Update()
    {
        if(active_map.name == "Overworld"){
            overworldX = player.transform.position.x;
            overworldY = player.transform.position.y;
            if(!active_map.GetComponent<Map>().play_music && done_changing){
                active_map.GetComponent<Map>().play_music = true;
            }
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
            SaveSystem.SetBool("inside_of_room", false);
        }
        else{
            SaveSystem.SetBool("in_submap", true);
            SaveSystem.SetString("submap_name", active_map.name);
        
            RoomHandler rh = active_map.GetComponentInChildren<RoomHandler>();

            if (rh)
            {
                SaveSystem.SetBool("inside_of_room", rh.rooms.active);
            }
        }
    }

    public void save_inn()
    {
        SaveSystem.SetFloat("overworldX", overworldX);
        SaveSystem.SetFloat("overworldY", overworldY);

        SaveSystem.SetBool("in_submap", false);
        SaveSystem.SetBool("inside_of_room", false);
    }
    
    void deactivate_maps_except(GameObject map){
        Map[] maps = grids.GetComponentsInChildren<Map>();
        foreach(Map m in maps){
            if(m.enabled == false){
                m.play_music = false;
                continue;
            }
            if(m.gameObject.GetInstanceID() != map.GetInstanceID()){
                m.play_music = false;
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

        player.can_move = false;
        
        Map active = active_map.GetComponent<Map>();
        active.play_music = false;

        bool use_stairs = active.use_stairs;
        
        AudioSource am_as = active_map.GetComponentInChildren<MusicHandler>().get_active();
        am_as.Stop();
        am_as.volume = 0f;
        
        player.warp_sound.Play();
        
        st.transition();
        while(st.filling){
            yield return null;
        }
        
        deactivate_maps_except(map);
        active = active_map.GetComponent<Map>();
        
        if(active.name != "Overworld"){
            if (use_stairs)
            {
                player.transform.position = active.stair_entry_position.position;
                player.sc.change_direction("down");
            }
            else
            {
                player.transform.position = active.entry_position.position;
                player.sc.change_direction("up");
            }
        }
        else{
            player.transform.position = new Vector3(overworldX, overworldY, 0f);
            player.sc.change_direction("down");
        }
        
        player.move_point.position = player.transform.position;
        player.map_just_changed = true;
        
        active_map.SetActive(true);
        
        st.unfilling = true;
        
        while(player.warp_sound.isPlaying || st.unfilling){
            yield return null;
        }
        
        active.play_music = true;

        player.can_move = true;
        
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
