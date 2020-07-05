using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{

    [Serializable]
    public class SpriteGroup{
    
        public string name;
    
        public Sprite up1;
        public Sprite up2;
        public Sprite down1;
        public Sprite down2;
        public Sprite step1;
        public Sprite step2;
    }
    
    public SpriteGroup[] characters;
    
    public int character_index;
    public SpriteGroup active_character;
    
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
        sr.sprite = active_character.down1;
        
        frames_since_last_increment = 15;

        display = true;
    }

    public void set_character(int index)
    {
        character_index = index;
        active_character = characters[character_index];
        sr.sprite = active_character.down1;
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

        sr.sprite = characters[character_index].down1;
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

        sr.sprite = characters[character_index].down1;
    }
    
    public void change_direction(string dir){
        direction = dir;
        switch (direction)
        {
            case "up":
                sr.sprite = active_character.up1;
                break;
            case "down":
                sr.sprite = active_character.down1;
                break;
            case "left":
                sr.flipX = false;
                sr.sprite = active_character.step1;
                break;
            case "right":
                sr.flipX = true;
                sr.sprite = active_character.step1;
                break;
        }
    }
    
    public string get_direction(){
        return direction;
    }
    
    public string get_class(){
        return active_character.name;
    }

    public bool walk_animation;

    public IEnumerator walk()
    {
        float wait = 0.13189315f;
        walk_animation = true;
        switch (direction)
        {
            case "up":
                sr.sprite = active_character.up1;
                yield return new WaitForSeconds(wait);
                sr.sprite = active_character.up2;
                yield return new WaitForSeconds(wait);
                break;
            case "down":
                sr.sprite = active_character.down1;
                yield return new WaitForSeconds(wait);
                sr.sprite = active_character.down2;
                yield return new WaitForSeconds(wait);
                break;
            case "left":
                sr.flipX = false;
                sr.sprite = active_character.step1;
                yield return new WaitForSeconds(wait);
                sr.flipX = false;
                sr.sprite = active_character.step2;
                yield return new WaitForSeconds(wait);
                break;
            case "right":
                sr.flipX = true;
                sr.sprite = active_character.step1;
                yield return new WaitForSeconds(wait);
                sr.sprite = active_character.step2;
                yield return new WaitForSeconds(wait);
                break;
        }
        walk_animation = false;
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
    }
}
