using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    // Start is called before the first frame update
    
    public Transform move_point;
    
    public LayerMask collision;
    public LayerMask water;
    public LayerMask river;
    
    public float move_speed;

    public SpriteController sc;
    
    private int frames_until_move;
    private int frame_count;
    
    private SpriteRenderer child_sr;
    
    private float last_direction;

    public bool immobile_npc;
    private bool can_move;
    
    private BoxCollider2D cld;
    
    void Awake()
    {
        move_point.parent = transform.parent;
        
        cld = GetComponent<BoxCollider2D>();
        
        frames_until_move = Random.Range(30, 300);
        frame_count = 0;
        last_direction = 1f;
        
        child_sr = GetComponent<SpriteRenderer>();
        
        can_move = true;
    }

    int move_away_frames;

    // Update is called once per frame
    void Update()
    {
        frame_count += 1;
        move_away_frames += 1;
        
        float multiplier = 2f;
        float hor = 0f;
        float ver = 0f;
        
        if(frame_count >= frames_until_move && can_move && !immobile_npc){
            if (!is_player_within_radius(5f))
            {
                float direction = Random.Range(0, 3);

                float continue_from_last_direction = Random.Range(0, 3);
                if (continue_from_last_direction == 1f)
                {
                    direction = last_direction;
                }

                Vector3 dirvec = new Vector3(0, 0, 0);

                switch (direction)
                {
                    case 0:
                        ver = 1f;
                        dirvec = new Vector3(0, 1, 0);
                        break;
                    case 1:
                        ver = -1f;
                        dirvec = new Vector3(0, -1, 0);
                        break;
                    case 2:
                        hor = 1f;
                        dirvec = new Vector3(1, 0, 0);
                        break;
                    case 3:
                        hor = -1f;
                        dirvec = new Vector3(-1, 0, 0);
                        break;
                }

                while (is_layer_in_direction("Warp", dirvec) && is_layer_in_direction("NPC", dirvec))
                {
                    switch (direction)
                    {
                        case 0:
                            ver = 1f;
                            dirvec = new Vector3(0, 1, 0);
                            break;
                        case 1:
                            ver = -1f;
                            dirvec = new Vector3(0, -1, 0);
                            break;
                        case 2:
                            hor = 1f;
                            dirvec = new Vector3(1, 0, 0);
                            break;
                        case 3:
                            hor = -1f;
                            dirvec = new Vector3(-1, 0, 0);
                            break;
                    }
                }
                frame_count = 0;
                frames_until_move = Random.Range(120, 300);

                last_direction = direction;
            }
            else if (is_player_within_radius(2.5f) && move_away_frames > 90)
            {
                Collider2D[] player = Physics2D.OverlapCircleAll(transform.position, 2.5f, 1 << LayerMask.NameToLayer("Player"));
                Vector3 direction_to_player = player[0].gameObject.transform.position - transform.position;

                float vert = direction_to_player.y;
                float horz = direction_to_player.x;

                if(vert > horz)
                {
                    if(vert > 0)
                    {
                        ver = -1;
                    }
                    else
                    {
                        ver = 1;
                    }
                }
                else
                {
                    if (horz > 0)
                    {
                        hor = -1;
                    }
                    else
                    {
                        hor = 1;
                    }
                }

                move_away_frames = 0;
            }
        }
        
        hor *= multiplier;
        ver *= multiplier;
    
        if(Mathf.Abs(hor) == multiplier){
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(hor, 0f, 0f), multiplier, collision | water | river);
            if(hor == multiplier){
                sc.change_direction("right");
            }
            if(hor == -1f * multiplier){
                sc.change_direction("left");
            }
            if(hit.collider == null || hit.collider.gameObject.layer == 0){
                move_point.position += new Vector3(hor, 0f, 0f);
            }
            else{
                last_direction = Random.Range(0, 3);
            }
        }
        
        else if(Mathf.Abs(ver) == multiplier){
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(0f, ver, 0f), 1f * multiplier, collision | water | river);
            if(ver == multiplier){
                sc.change_direction("up");
            }
            if(ver == -1f * multiplier){
                //anim.SetTrigger("down");
                sc.change_direction("down");
            }
            if(hit.collider == null || hit.collider.gameObject.layer == 0){
                move_point.position += new Vector3(0f, ver, 0f);
            }
            else{
                last_direction = Random.Range(0, 3);
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, move_point.position, move_speed * Time.deltaTime);
    }
    
    public IEnumerator interact(PlayerController p){
        if (is_player_within_radius(2.5f))
        {
            can_move = false;

            if (p.sc.get_direction() == "down")
            {
                sc.change_direction("up");
            }
            else if (p.sc.get_direction() == "up")
            {
                sc.change_direction("down");
            }
            else if (p.sc.get_direction() == "left")
            {
                sc.change_direction("right");
            }
            else if (p.sc.get_direction() == "right")
            {
                sc.change_direction("left");
            }

            Vector3 p_pos = p.gameObject.transform.position;

            Vector3 location = new Vector3(p_pos.x, p_pos.y - 7.5f, p_pos.z);

            display_textbox(location);

            yield return new WaitForSeconds(.6f);
            while (Input.GetAxisRaw("Submit") == 0)
            {
                yield return null;
            }

            hide_textbox();
            p.can_move = true;
            can_move = true;

            p.frames_since_last_interact = 0;
        }
    }

    bool is_player_within_radius(float radius)
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(transform.position, radius, 1 << LayerMask.NameToLayer("Player"));
        /*
        Collider2D[] NPCs = Physics2D.OverlapCircleAll(transform.position, radius, 1 << LayerMask.NameToLayer("NPC"));
        Collider2D[] warps = Physics2D.OverlapCircleAll(transform.position, radius, 1 << LayerMask.NameToLayer("Warp"));
        return player.Length + (NPCs.Length - 1) + warps.Length > 0;
        */
        return player.Length > 0;
    }

    bool is_layer_in_direction(string layer_name, Vector3 direction)
    {
        float radius = .1f;
        Collider2D[] layer = Physics2D.OverlapCircleAll(transform.position + direction, radius, 1 << LayerMask.NameToLayer(layer_name));
        if (layer_name == "NPC")
            return layer.Length - 1 > 0;
        return layer.Length > 0;
    }
    
    void OnCollisionEnter2D(Collision2D c){
        GetComponent<BoxCollider2D>().enabled = false;
        can_move = false;
    }

    void OnCollisionExit2D(Collision2D c)
    {
        can_move = true;
    }
}
