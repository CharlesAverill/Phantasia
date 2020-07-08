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

        player.can_move = false;

        battling = true;

        while (player.transform.position != player.move_point.position)
        {
            yield return null;
        }

        if (player.travel_mode != "none")
        {

            player.pause_menu_container.SetActive(false);

            GlobalControl.instance.monster_party = player.og.get_monster_party();

            int countLoaded = SceneManager.sceneCount;
            if (countLoaded == 1)
            {

                player.can_move = false;
                player.multiplier = 0f;

                //gameObject.AddComponent(typeof(Camera));
                cam.transform.parent = gameObject.transform;

                GlobalControl.instance.overworld_scene_container.SetActive(false);

                //gameObject.AddComponent(typeof(AudioListener));

                AudioSource source = GetComponent<AudioSource>();
                source.Play();
                while (source.isPlaying)
                {
                    yield return null;
                }

                Destroy(GetComponent<AudioListener>());

                SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
                Destroy(GetComponent<Camera>());

                while (SceneManager.sceneCount > 1)
                {
                    yield return null;
                }

                GlobalControl.instance.overworld_scene_container.SetActive(true);
            }

            countLoaded = SceneManager.sceneCount;

            while (countLoaded > 1)
            {
                countLoaded = SceneManager.sceneCount;
                yield return null;
            }

            player.can_move = true;
            player.multiplier = 2f;

            player.pause_menu_container.SetActive(true);

            player.map_handler.save_position();
            battling = false;
        }
        else
        {
            seed = 1;
        }
        
        yield return null;
    }

    bool won_boss_battle;

    public IEnumerator start_boss_battle(PlayerController player, GameObject boss, string flag, bool flagval, GameObject overworld_boss)
    {

        player.can_move = false;

        battling = true;
        player.pause_menu_container.SetActive(false);

        GlobalControl.instance.monster_party = boss;

        int countLoaded = SceneManager.sceneCount;
        if (countLoaded == 1)
        {
            player.multiplier = 0f;

            cam.transform.parent = gameObject.transform;

            GlobalControl.instance.overworld_scene_container.SetActive(false);
            GlobalControl.instance.bossmode = true;

            //AudioSource source = GetComponent<AudioSource>();
            //source.Play();
            //yield return new WaitForSeconds(1.127f);

            SceneManager.LoadScene("Battle", LoadSceneMode.Additive);

            while (SceneManager.sceneCount > 1)
            {
                yield return null;
            }
        }

        

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

            gen_seed();
            SaveSystem.SetInt("reh_seed", seed);
        }
    }
}
