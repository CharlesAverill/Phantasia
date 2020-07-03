using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSprite : MonoBehaviour
{

    public Sprite weapon_sprite;
    public SpriteRenderer sr;
    public Transform forward;
    public Transform back;
    public bool display;

    // Start is called before the first frame update
    void Start()
    {
        sr.enabled = false;
        display = false;
    }

    public void set_sprite(Sprite n)
    {
        weapon_sprite = n;
        sr.sprite = n;
    }

    public void show()
    {
        display = true;
        sr.enabled = true;
    }

    public void hide()
    {
        display = false;
        sr.enabled = false;
    }

    public void go_forward()
    {
        sr.gameObject.transform.position = forward.position;
        sr.gameObject.transform.rotation = forward.rotation;
    }

    public void go_back()
    {
        sr.gameObject.transform.position = back.position;
        sr.gameObject.transform.rotation = back.rotation;
    }
}
