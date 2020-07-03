using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSprite : MonoBehaviour
{

    public Sprite magic_sprite;
    public SpriteRenderer sr;
    public bool display;

    // Start is called before the first frame update
    void Start()
    {
        sr.enabled = false;
        display = false;
    }

    public void set_sprite(Sprite n)
    {
        magic_sprite = n;
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
}
