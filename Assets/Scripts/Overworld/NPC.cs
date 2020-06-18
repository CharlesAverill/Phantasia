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
    
    public Sprite down;
    public Sprite left;
    public Sprite up;
    
    private int frames_until_move;
    private int frame_count;
    
    private SpriteRenderer child_sr;
    
    private float last_direction;
    
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

    // Update is called once per frame
    void Update()
    {
        frame_count += 1;
        
        float multiplier = 2f;
        float hor = 0f;
        float ver = 0f;
        
        if(frame_count >= frames_until_move && can_move){
            float direction = Random.Range(0, 3);
            
            float continue_from_last_direction = Random.Range(0, 3);
            if(continue_from_last_direction == 1f){
                direction = last_direction;
            }
            
            switch(direction){
                case 0:
                    ver = 1f;
                    break;
                case 1:
                    ver = -1f;
                    break;
                case 2:
                    hor = 1f;
                    break;
                case 3:
                    hor = -1f;
                    break;
            }
            frame_count = 0;
            frames_until_move = Random.Range(120, 300);
            
            last_direction = direction;
        }
        
        hor *= multiplier;
        ver *= multiplier;
    
        if(Mathf.Abs(hor) == multiplier){
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(hor, 0f, 0f), multiplier, collision | water | river);
            if(hor == multiplier){
                 //anim.SetTrigger("right");
                child_sr.flipX = true;
                child_sr.sprite = left;
            }
            if(hor == -1f * multiplier){
                //anim.SetTrigger("left");
                child_sr.flipX = false;
                child_sr.sprite = left;
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
                 //anim.SetTrigger("up");
                child_sr.sprite = up;
            }
            if(ver == -1f * multiplier){
                //anim.SetTrigger("down");
                child_sr.sprite = down;
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
        can_move = false;
        
        if(p.sc.get_direction() == "down"){
            child_sr.sprite = down;
        }
        if(p.sc.get_direction() == "up"){
            child_sr.sprite = up;
        }
        if(p.sc.get_direction() == "left"){
            child_sr.sprite = left;
            child_sr.flipX = false;
        }
        else if(p.sc.get_direction() == "right"){
            child_sr.sprite = left;
            child_sr.flipX = true;
        }
    
        Vector3 p_pos = p.gameObject.transform.position;
    
        Vector3 location = new Vector3(p_pos.x, p_pos.y - 7.5f, p_pos.z);
    
        display_textbox(location);
        
        yield return new WaitForSeconds(.2f);
        while(Input.GetAxisRaw("Submit") == 0){
            yield return null;
        }
        
        hide_textbox();
        p.can_move = true;
        can_move = true;
        
        p.frames_since_last_interact = 0;
    }
    
    void OnCollisionEnter2D(Collision2D c){
        gameObject.SetActive(false);
    }
}
