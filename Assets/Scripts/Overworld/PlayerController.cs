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

    public GameObject pause_menu_container;
    
    public SpriteController sc;
    
    public MapHandler map_handler;
    
    public bool map_just_changed;
    
    public AudioSource warp_sound;
    
    public RandomEncounterHandler reh;
    
    public OverworldGrid og;
    
    public int frames_since_last_interact;

    void OnEnable()
    {
        Cursor.visible = false;
        if (can_move)
            pause_menu_container.SetActive(true);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
        move_point.parent = transform.parent;
        move_point.transform.position = transform.position;
        can_move = true;
        map_just_changed = false;
        
        frames_since_last_interact = 0;
        
        reh.gameObject.SetActive(true);
        reh.seed = SaveSystem.GetInt("reh_seed");
    }

    float timer = -1;
    List<float> times;

    void startTimer()
    {
        if(times == null)
        {
            times = new List<float>();
        }
        timer = 0;
    }

    void stopTimer()
    {
        Debug.Log(timer);
        times.Add(timer);

        float total = 0f;
        foreach(float f in times)
        {
            total += f;
        }
        Debug.Log("Average: " + (total / (float)times.Count));

        timer = -1f;
    }

    public float multiplier = 2f;

    // Update is called once per frame
    void Update()
    {
        
        if(reh.gameObject.active == false){
            reh.gameObject.SetActive(true);
        }
        /*
        if(Input.GetKeyDown("i") && can_move && reh.seed > 0){
            Debug.Log("Saving...");
            SaveSystem.SetInt("reh_seed", reh.seed);
            map_handler.save_position();
        }
        */

        /*
        if(Input.GetKeyDown("c") && can_move && reh.seed > 0){
            sc.increment_character();
        }
        */
    
        //Movement
        transform.rotation = Quaternion.identity;
        
        transform.position = Vector3.MoveTowards(transform.position, move_point.position, move_speed * Time.deltaTime);
        
        if(can_move){
            if(Vector3.Distance(transform.position, move_point.position) <= .025f){
                
                float hor = Input.GetAxisRaw("Horizontal") * multiplier;
                float ver = Input.GetAxisRaw("Vertical") * multiplier;

                if (hor == 0f && ver == 0f && transform.position == move_point.position)
                {
                    switch (sc.get_direction())
                    {
                        case "up":
                            sc.change_direction("up");
                            break;
                        case "down":
                            sc.change_direction("down");
                            break;
                        case "left":
                            sc.change_direction("left");
                            break;
                        case "right":
                            sc.change_direction("right");
                            break;
                    }
                }

                else if (Mathf.Abs(hor) == 1f * multiplier){
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(hor, 0f, 0f), 1f * multiplier, collision | water | river);
                    if(hor == 1f * multiplier){
                         sc.change_direction("right");
                    }
                    if(hor == -1f * multiplier){
                        sc.change_direction("left");
                    }
                    if(hit.collider == null || hit.collider.gameObject.layer == 0 || hit.collider.isTrigger){
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
                        StartCoroutine(sc.walk());
                    }
                }
                
                else if(Mathf.Abs(ver) == 1f * multiplier){
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(0f, ver, 0f), 1f * multiplier, collision | water | river);
                    if(ver == 1f * multiplier){
                        sc.change_direction("up");
                    }
                    if(ver == -1f * multiplier){
                        sc.change_direction("down");
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
                        StartCoroutine(sc.walk());
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
                Chest inter_chest = obj.GetComponent<Chest>();
                LockedDoor lockedDoor = obj.GetComponent<LockedDoor>();
                
                if(inter_npc){
                    if(inter_npc.move_point.position == inter_npc.transform.position)
                        StartCoroutine(inter_npc.interact(this));
                }
                else if (inter_chest)
                {
                    StartCoroutine(inter_chest.interact(this));
                }
                else if (lockedDoor)
                {
                    StartCoroutine(lockedDoor.interact(this));
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
        if(sc.get_direction() == "up"){
            direction = new Vector3(0f, 1f, 0f);
        }
        else if(sc.get_direction() == "down"){
            direction = new Vector3(0f, -1f, 0f);
        }
        else if(sc.get_direction() == "left"){
            direction = new Vector3(-1f, 0f, 0f);
        }
        else if(sc.get_direction() == "right"){
            direction = new Vector3(1f, 0f, 0f);
        }
        return direction;
    }
    
    void OnTriggerEnter2D(Collider2D c){

        if(transform.position == move_point.position)
        {
            return;
        }
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
        }
        else if (c.gameObject.GetComponent<ShopWarp>() && move_point.position != transform.position)
        {
            map_just_changed = true;
            StartCoroutine(shop_warp(c.gameObject.GetComponent<ShopWarp>()));
        }
        else if(map_handler.done_changing)
        {
            can_move = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D c){
        map_just_changed = false;
        reh.gameObject.SetActive(true);
        if (map_handler.done_changing)
        {
            can_move = true;
        }
    }

    IEnumerator shop_warp(ShopWarp warp)
    {
        can_move = false;

        while (transform.position != move_point.position)
        {
            yield return null;
        }

        StartCoroutine(warp.warp());

        while (warp.shopping)
        {
            yield return null;
        }

        can_move = true;
    }
    
    IEnumerator change_map(GameObject map){
        can_move = false;

        pause_menu_container.SetActive(false);

        while (transform.position != move_point.position){
            yield return null;
        }
        
        map_handler.change_maps(map);
        
        while(!map_handler.done_changing){
            yield return null;
        }
        
        can_move = true;

        pause_menu_container.SetActive(true);
    }
}
