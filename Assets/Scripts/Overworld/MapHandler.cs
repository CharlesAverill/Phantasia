using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    
    public PlayerController player;
    public ScreenTransition st;
    public string[] map_names;
    public GameObject grids;
    public GameObject active_map;
    
    float overworldX;
    float overworldY;
    
    public bool done_changing;
    
    // Start is called before the first frame update
    void Start()
    {
        deactivate_maps_except(active_map);
        done_changing = false;
        active_map.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(active_map.name == "Overworld"){
            overworldX = player.transform.position.x;
            overworldY = player.transform.position.y;
        }
    }
    
    void deactivate_maps_except(GameObject map){
        Map[] maps = grids.GetComponentsInChildren<Map>();
        foreach(Map m in maps){
            if(m.gameObject.GetInstanceID() != map.GetInstanceID()){
                m.gameObject.SetActive(false);
            }
        }
        
        active_map = map;
    }
    
    public void change_maps(GameObject map){
        StartCoroutine(change(map));
    }
    
    IEnumerator change(GameObject map){
        done_changing = false;
        active_map.GetComponent<AudioSource>().Stop();
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
        
        st.unfilling = true;
        
        while(player.warp_sound.isPlaying){
            yield return null;
        }
        active_map.GetComponent<AudioSource>().Play();
        
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
