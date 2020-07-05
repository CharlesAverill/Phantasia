using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomEncounterHandler : MonoBehaviour
{

    public int seed;
    
    public PlayerController player;
    public Transform cam;
    
    public bool encounters;
    
    public void set_encounters(bool onoff){
        encounters = onoff;
    }
    
    public void gen_seed(){
        seed = Random.Range(50, 255);
    }
    
    public void decrement(int d){
        if(encounters){
            seed -= d;
        }
    }
    
    public bool battling;
    
    IEnumerator initiate_encounter(){
    
        battling = true;

        player.pause_menu_container.SetActive(false);

        GlobalControl.instance.monster_party = player.og.get_monster_party();
    
        int countLoaded = SceneManager.sceneCount;
        if(countLoaded == 1){

            player.can_move = false;
            player.multiplier = 0f;

            while(player.transform.position != player.move_point.position){
                yield return null;
            }
            
            //gameObject.AddComponent(typeof(Camera));
            cam.transform.parent = gameObject.transform;

            GlobalControl.instance.overworld_scene_container.SetActive(false);
            
            //gameObject.AddComponent(typeof(AudioListener));
            
            AudioSource source = GetComponent<AudioSource>();
            source.Play();
            while(source.isPlaying){
                yield return null;
            }
            
            Destroy(GetComponent<AudioListener>());
            
            SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
            Destroy(GetComponent<Camera>());
            
            while(SceneManager.sceneCount > 1){
                yield return null;
            }
            GlobalControl.instance.overworld_scene_container.SetActive(true);
        }
        
        countLoaded = SceneManager.sceneCount;
        
        while(countLoaded > 1){
            countLoaded = SceneManager.sceneCount;
            yield return null;
        }
        
        player.can_move = true;
        player.multiplier = 2f;

        gen_seed();
        SaveSystem.SetInt("reh_seed", seed);

        player.pause_menu_container.SetActive(true);

        player.map_handler.save_position();
        battling = false;
        
        yield return null;
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        if(!player == null)
            encounters = player.map_handler.active_map.GetComponent<Map>().encounters;
        seed = SaveSystem.GetInt("reh_seed");
    }

    // Update is called once per frame
    void Update()
    {
        if(seed <= 0 && !battling){
            Debug.Log("Random encounter initiated");
            StartCoroutine(initiate_encounter());
        }
    }
}
