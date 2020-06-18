using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenHandler : MonoBehaviour
{
    
    public LoadingCircle loading_circle;
    
    public AudioSource classic;
    public AudioSource remaster;
    
    private string[] names;

    // Start is called before the first frame update
    void Start()
    {
    
        string bin_path = Application.persistentDataPath + "/party.bin";
        
        names = new string[]{"Matt", "Alta", "Ivan", "Cora"};
        
        if(!System.IO.File.Exists(bin_path)){
        
            SaveSystem.SetBool("classic_music", true);
        
            SaveSystem.SetBool("in_submap", false);
            
            SaveSystem.SetInt("reh_seed", 255);
        
            SaveSystem.SetFloat("overworldX", -1f);
            SaveSystem.SetFloat("overworldY", -5f);
            
            SaveSystem.SetString("Player1_name", names[0]);
            SaveSystem.SetString("Player2_name", names[1]);
            SaveSystem.SetString("Player3_name", names[2]);
            SaveSystem.SetString("Player4_name", names[3]);
            
            foreach(string name in names){
                SaveSystem.SetInt(name + "_HP", 100);
                SaveSystem.SetInt(name + "_exp", 0);
                SaveSystem.SetBool(name + "_poison", false);
                SaveSystem.SetBool(name + "_stone", false);
            }
            SaveSystem.SaveToDisk();
            Debug.Log("Done initializing");
        }
        
        if(SaveSystem.GetBool("classic_music")){
             classic.volume = 1f;
            remaster.volume = 0f;
        }
        else{
            remaster.volume = 1f;
            classic.volume = 0f;
        }
    }
    
    int frames_since_music_switch;

    // Update is called once per frame
    void Update()
    {
        
        frames_since_music_switch += 1;
    
        if(Input.GetKeyDown("h") && names.Length > 0){
            foreach(string name in names){
                SaveSystem.SetInt(name + "_HP", 100);
            }
            Debug.Log("Healed your party");
        }
        
        if(Input.GetKeyDown("m") && frames_since_music_switch >= 15){
            SaveSystem.SetBool("classic_music", !SaveSystem.GetBool("classic_music"));
            if(SaveSystem.GetBool("classic_music")){
                classic.volume = 1f;
                remaster.volume = 0f;
            }
            else{
                remaster.volume = 1f;
                classic.volume = 0f;
            }
            
            frames_since_music_switch = 0;
        }
    }
    
    public void exit(){
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void start_game(){
        StartCoroutine(load());
    }
    
    IEnumerator load(){
        loading_circle.start_loading_circle();
        yield return new WaitForSeconds(.75f);
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadSceneAsync("Overworld");
    }
}
