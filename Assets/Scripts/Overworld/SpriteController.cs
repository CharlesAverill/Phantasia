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
    
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        active_character = characters[character_index];
        
        direction = "down";
    
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = active_character.down;
        
        frames_since_last_increment = 15;
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
        if(character_index < 0){
            character_index = characters.Length - 1;
        }
        
        SaveSystem.SetInt("character_index", character_index);
    }
    
    public void change_direction(string dir){
        direction = dir;
    }
    
    public string get_direction(){
        return direction;
    }

    // Update is called once per frame
    void Update()
    {
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
