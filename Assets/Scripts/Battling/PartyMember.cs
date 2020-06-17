using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : Battler
{
    public string job;
    public string weapon;
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;
    public int luck;
    public int level;
    
    public bool can_run;
    
    public BattleHandler bh;
    
    public Vector3 move_point;
    
    private CursorController monster_cursor;
    private CursorController menu_cursor;
    
    private Animator anim;
    
    public string action;
    public GameObject target;
    
    public IEnumerator choose_monster(string act){
    
        monster_cursor.gameObject.SetActive(true);
        menu_cursor.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(.2f);
        
        while(Input.GetAxis("Submit") == 0){
            yield return null;
        }
        
        target = monster_cursor.get_monster().gameObject;
        
        anim.SetTrigger("walk");
        walk_back();
        while(is_moving()){
            yield return null;
        }
        
        action = act;
    }
    
    public void walk_back(){
        move_point = new Vector3(3.66f, transform.position.y, transform.position.z);
    }
    
    public bool is_moving(){
        return transform.position != move_point;
    }
    
    public bool is_playing_animation(){
        return anim.GetCurrentAnimatorStateInfo(0).length >
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    
    public bool done_showing;
    
    public IEnumerator show_battle(){
        done_showing = false;
        anim.SetTrigger("walk");
        move_point = new Vector3(1.66f, transform.position.y, transform.position.z);
        while(is_moving()){
            yield return null;
        }
        anim.SetTrigger("fight");
        while(is_playing_animation()){
            yield return null;
        }
        anim.SetTrigger("walk");
        move_point = new Vector3(3.66f, transform.position.y, transform.position.z);
        while(is_moving()){
            yield return null;
        }
        done_showing = true;
    }
    
    private void check_load(){
        if(!monster_cursor)
            monster_cursor = bh.monster_cursor;
        if(!anim)
            anim = GetComponent<Animator>();
        if(!menu_cursor)
            menu_cursor = bh.menu_cursor;
    }
    
    public void turn(){
        check_load();
    
        action = "";
        target = null;
        
        monster_cursor.gameObject.SetActive(false);
        
        anim.SetTrigger("walk");
        
        move_point = new Vector3(1.66f, transform.position.y, transform.position.z);

        menu_cursor.gameObject.SetActive(true);
    }
    
    public bool done_set_up;
    
    // Start is called before the first frame update
    void Start()
    {
        done_set_up = false;
        
        move_point = transform.position;
        monster_cursor = bh.monster_cursor;
        menu_cursor = bh.menu_cursor;
        
        menu_cursor.gameObject.SetActive(true);
        monster_cursor.gameObject.SetActive(false);
        
        anim = GetComponent<Animator>();
        anim.speed = 3f;
        
        done_set_up = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, move_point, 8 * Time.deltaTime);
    }
}
