using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{

    Image fade_img;

    bool fade_in;
    bool fade_out;

    public Color set_color;

    // Start is called before the first frame update
    void Start()
    {
        fade_img = GetComponentInChildren<Image>();
        fade_in = false;
        fade_out = false;
        set_color = new Color32(0, 0, 0, 0);
    }

    public void start_fade(bool to_black)
    {
        if (to_black)
        {
            set_color.a = 0;
            fade_out = true;
            fade_in = false;
        }
        else
        {
            set_color.a = 1;
            fade_in = true;
            fade_out = false;
        }
    }

    bool can_fade()
    {
        int n = frame % 10;
        int[] allowed = new int[] { 1, 3, 4, 6, 7, 8, 9};
        return (allowed.Contains(n));
    }

    int frame = 0;

    // Update is called once per frame
    void Update()
    {

        frame += 1;

        if (fade_out && can_fade())
        {
            fade_img.color = set_color;
            set_color.a = set_color.a + .01f;

            if (set_color.a >= 1f)
            {
                fade_out = false;
                set_color.a = 1f;
            }
        }
        else if (fade_in && can_fade())
        {
            fade_img.color = set_color;
            set_color.a = set_color.a - .01f;

            if (set_color.a <= 0f)
            {
                fade_out = false;
                set_color.a = 0f;
            }
        }
    }

    public bool is_fading()
    {
        return fade_out || fade_in;
    }
}
