using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSpriteController : MonoBehaviour
{
    public Sprite idle;
    public Sprite walking1;
    public Sprite walking2;
    public Sprite attack1;
    public Sprite attack2;
    public Sprite magic_victory1;
    public Sprite magic_victory2;
    public Sprite tired;
    public Sprite stone;
    public Sprite dead;
    
    public SpriteRenderer sr;

    string state;

    // Start is called before the first frame update
    void Start()
    {
        state = "idle";
    
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = idle;
    }
    
    public void change_state(string st, WeaponSprite ws=null){
        state = st;
        switch (state)
        {
            case "idle":
                sr.sprite = idle;
                break;
            case "walk":
                StartCoroutine(walk());
                break;
            case "fight":
                StartCoroutine(fight(ws));
                break;
            case "magic":
                StartCoroutine(cast());
                break;
            case "tired":
                sr.sprite = tired;
                break;
            case "stone":
                sr.sprite = stone;
                break;
            case "dead":
                sr.sprite = dead;
                GetComponent<PartyMember>().move_point = new Vector3(sr.gameObject.transform.position.x - .66f, sr.gameObject.transform.position.y, sr.gameObject.transform.position.z);
                //sr.gameObject.transform.position = new Vector3(sr.gameObject.transform.position.x - .66f, sr.gameObject.transform.position.y, sr.gameObject.transform.position.z);
                break;
            case "run":
                sr.sprite = idle;
                sr.flipX = true;
                break;
        }
    }
    
    public string get_state(){
        return state;
    }

    public bool is_walking;
    public IEnumerator walk()
    {
        is_walking = true;
        float wait = 0.071657625f;

        sr.sprite = walking1;
        yield return new WaitForSeconds(wait);
        sr.sprite = walking2;
        yield return new WaitForSeconds(wait);
        sr.sprite = walking1;
        yield return new WaitForSeconds(wait);
        sr.sprite = walking2;
        yield return new WaitForSeconds(wait);

        is_walking = false;

        yield return null;
    }

    public bool is_fighting;
    public IEnumerator fight(WeaponSprite ws)
    {

        if(ws == null)
        {
            is_fighting = true;
            float wait = 0.071657625f;

            sr.sprite = attack1;
            yield return new WaitForSeconds(wait);
            sr.sprite = attack2;
            yield return new WaitForSeconds(wait);
            sr.sprite = attack1;
            yield return new WaitForSeconds(wait);
            sr.sprite = attack2;
            yield return new WaitForSeconds(wait);

        }
        else
        {
            is_fighting = true;
            float wait = 0.071657625f;

            ws.show();

            ws.go_forward();
            sr.sprite = attack1;
            yield return new WaitForSeconds(wait);
            ws.go_back();
            sr.sprite = attack2;
            yield return new WaitForSeconds(wait);
            ws.go_forward();
            sr.sprite = attack1;
            yield return new WaitForSeconds(wait);
            ws.go_back();
            sr.sprite = attack2;
            yield return new WaitForSeconds(wait);

            ws.hide();
        }

        is_fighting = false;

        yield return null;
    }

    public bool is_casting;
    public IEnumerator cast()
    {
        is_casting = true;
        Debug.Log("casting");
        is_casting = false;

        yield return null;
    }

    /*
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
    */
}
