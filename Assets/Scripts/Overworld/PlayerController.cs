using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    
    public float move_speed;
    public Transform move_point;
    public bool can_move;
    
    public string travel_mode;
    
    public LayerMask collision;
    public LayerMask water;
    public LayerMask river;
    public LayerMask NPC;
    
    public SpriteRenderer sr;
    
    public Sprite up;
    public Sprite down;
    public Sprite left;
    
    public MapHandler map_handler;
    
    public bool map_just_changed;
    
    public AudioSource warp_sound;
    
    public RandomEncounterHandler reh;
    
    public OverworldGrid og;
    
    public int frames_since_last_interact;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        move_point.parent = transform.parent;
        move_point.transform.position = transform.position;
        can_move = true;
        map_just_changed = false;
        
        frames_since_last_interact = 0;
        
        reh.gameObject.SetActive(true);
        reh.seed = SaveSystem.GetInt("reh_seed");
    }

    // Update is called once per frame
    void Update()
    {
        
        if(reh.gameObject.active == false){
            reh.gameObject.SetActive(true);
        }
        
        if(Input.GetKeyDown("i") && can_move && reh.seed > 0){
            Debug.Log("Saving...");
            SaveSystem.SetInt("reh_seed", reh.seed);
            map_handler.save_position();
        }
    
        //Movement
        transform.rotation = Quaternion.identity;
        
        transform.position = Vector3.MoveTowards(transform.position, move_point.position, move_speed * Time.deltaTime);
        
        if(can_move){
            if(Vector3.Distance(transform.position, move_point.position) <= .025f){
                
                float multiplier = 2f;
                
                float hor = Input.GetAxisRaw("Horizontal") * multiplier;
                float ver = Input.GetAxisRaw("Vertical") * multiplier;
            
                if(Mathf.Abs(hor) == 1f * multiplier){
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(hor, 0f, 0f), 1f * multiplier, collision | water | river);
                    if(hor == 1f * multiplier){
                         //anim.SetTrigger("right");
                        sr.flipX = true;
                        sr.sprite = left;
                    }
                    if(hor == -1f * multiplier){
                        //anim.SetTrigger("left");
                        sr.flipX = false;
                        sr.sprite = left;
                    }
                    if(hit.collider == null || hit.collider.gameObject.layer == 0){
                        move_point.position += new Vector3(hor, 0f, 0f);
                        switch(travel_mode){
                            case "walking":
                                reh.decrement(6);
                                break;
                            case "walking_dungeon":
                                reh.decrement(5);
                                break;
                            case "sailing":
                                reh.decrement(2);
                                break;
                        }
                    }
                }
                
                else if(Mathf.Abs(ver) == 1f * multiplier){
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(0f, ver, 0f), 1f * multiplier, collision | water | river);
                    if(ver == 1f * multiplier){
                         //anim.SetTrigger("up");
                        sr.sprite = up;
                    }
                    if(ver == -1f * multiplier){
                        //anim.SetTrigger("down");
                        sr.sprite = down;
                    }
                    if(hit.collider == null || hit.collider.gameObject.layer == 0){
                        move_point.position += new Vector3(0f, ver, 0f);
                        switch(travel_mode){
                            case "walking":
                                reh.decrement(6);
                                break;
                            case "walking_dungeon":
                                reh.decrement(5);
                                break;
                            case "sailing":
                                reh.decrement(2);
                                break;
                        }
                    }
                }
            }
        }
        
        //Interaction
        if(Input.GetAxisRaw("Submit") != 0 && can_move && frames_since_last_interact > 30){
            can_move = false;
            
            Vector3 direction = get_direction_facing();
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 2f, NPC);
            if(hit.collider){
                GameObject obj = hit.collider.gameObject;
                NPC inter_npc = obj.GetComponent<NPC>();
                
                if(inter_npc){
                    StartCoroutine(inter_npc.interact(this));
                }
            }
            else{
                can_move = true;
            }
        }
        
        frames_since_last_interact += 1;
    }
    
    public Vector3 get_direction_facing(){
        Vector3 direction = Vector3.zero;
        if(sr.sprite == up){
            direction = new Vector3(0f, 1f, 0f);
        }
        else if(sr.sprite == down){
            direction = new Vector3(0f, -1f, 0f);
        }
        else if(sr.sprite == left){
            if(sr.flipX){
                direction = new Vector3(1f, 0f, 0f);
            }
            else{
                direction = new Vector3(-1f, 0f, 0f);
            }
        }
        return direction;
    }
    
    void OnTriggerEnter2D(Collider2D c){
        if(c.gameObject.GetComponent<RoomHandler>()){
            c.gameObject.GetComponent<RoomHandler>().change();
        }
        else if(c.gameObject.GetComponent<TilemapRenderer>() && !map_just_changed){
            reh.gameObject.SetActive(false);
            GameObject map = c.gameObject.GetComponent<WarpTiles>().warp_to;
            reh.set_encounters(map.GetComponent<Map>().encounters);
            StartCoroutine(change_map(map));
        }
        else if(c.gameObject.GetComponent<OverworldGrid>()){
            og = c.gameObject.GetComponent<OverworldGrid>();
            //SaveSystem.SetString("player_og", og.gameObject.name);
            //SaveSystem.SaveToDisk();
        }
    }
    
    void OnTriggerExit2D(Collider2D c){
        map_just_changed = false;
        reh.gameObject.SetActive(true);
    }
    
    IEnumerator change_map(GameObject map){
        can_move = false;
        
        while(transform.position != move_point.position){
            yield return null;
        }
        
        map_handler.change_maps(map);
        
        while(!map_handler.done_changing){
            yield return null;
        }
        
        can_move = true;
    }
}
