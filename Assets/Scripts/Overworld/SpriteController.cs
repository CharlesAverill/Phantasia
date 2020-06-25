using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{

    [Serializable]
    public class SpriteGroup{
    
        public string name;
    
        public Sprite up;
        public Sprite down;
        public Sprite step1;
        public Sprite step2;
    }
    
    public SpriteGroup[] characters;
    
    public int character_index;
    private SpriteGroup active_character;
    
    private string direction;
    
    public SpriteRenderer sr;
    
    public bool title_screen_mode;

    public bool display;

    // Start is called before the first frame update
    void Start()
    {
        if(!title_screen_mode){
            character_index = SaveSystem.GetInt("character_index");
        }
    
        active_character = characters[character_index];
        
        direction = "down";
    
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = active_character.down;
        
        frames_since_last_increment = 15;

        display = true;
    }

    public void set_character(int index)
    {
        character_index = index;
        active_character = characters[character_index];
    }
    
    private int frames_since_last_increment;
    
    public void increment_character(){
        if(frames_since_last_increment < 15){
            return;
        }
        
        character_index += 1;
        if(character_index >= characters.Length){
            character_index = 0;
        }
        
        frames_since_last_increment = 0;
        
        if(!title_screen_mode){
            SaveSystem.SetInt("character_index", character_index);
        }
    }
    
    public void decrement_character(){
        if(frames_since_last_increment < 15){
            return;
        }
        
        character_index -= 1;
        if(character_index < 0){
            character_index = characters.Length - 1;
        }
        
        frames_since_last_increment = 0;
        
        if(!title_screen_mode){
            SaveSystem.SetInt("character_index", character_index);
        }
    }
    
    public void change_direction(string dir){
        direction = dir;
    }
    
    public string get_direction(){
        return direction;
    }
    
    public string get_class(){
        return active_character.name;
    }

    // Update is called once per frame
    void Update()
    {

        if(display && sr.enabled == false)
        {
            sr.enabled = true;
        }
        else if(!display)
        {
            sr.enabled = false;
        }

        frames_since_last_increment += 1;
        
        if(active_character != characters[character_index]){
            active_character = characters[character_index];
        }
        
        switch(direction){
            case "up":
                sr.sprite = active_character.up;
                break;
            case "down":
                sr.sprite = active_character.down;
                break;
            case "left":
                sr.sprite = active_character.step1;
                sr.flipX = false;
                break;
            case "right":
                sr.sprite = active_character.step1;
                sr.flipX = true;
                break;
        }
    }
}
