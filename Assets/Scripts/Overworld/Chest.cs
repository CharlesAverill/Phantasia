using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{

    public string ID;
    bool obtained;

    public bool gold;
    public int gold_val;

    public bool item;
    public string item_val;

    public bool weapon;
    public string weapon_val;

    public bool armor;
    public string armor_val;

    public IEnumerator interact(PlayerController p)
    {
        p.can_move = false;
        p.pause_menu_container.SetActive(false);

        Vector3 p_pos = Vector3.zero;
        Vector3 location = Vector3.zero;

        if (!obtained)
        {
            p_pos = p.gameObject.transform.position;

            location = new Vector3(p_pos.x, p_pos.y - 7.5f, p_pos.z);

            if (gold)
            {
                dialogue = "Obtained " + gold_val + " G";
                SaveSystem.SetInt("gil", SaveSystem.GetInt("gil") + gold_val);
            }
            else if (item || weapon || armor)
            {
                dialogue = "Obtained " + item_val;

                Dictionary<string, int> items = SaveSystem.GetStringIntDict("items");
                if (items.ContainsKey(item_val))
                {
                    items[item_val] = items[item_val] + 1;
                }
                else
                    items.Add(item_val, 1);
            }

            SaveSystem.SetBool("chest_" + ID, true);
            obtained = true;
        }
        else
        {

            p_pos = p.gameObject.transform.position;

            location = new Vector3(p_pos.x, p_pos.y - 7.5f, p_pos.z);

            dialogue = "Nothing";
        }

        display_textbox(location);

        yield return new WaitForSeconds(.8f);
        while (!Input.GetKey(CustomInputManager.cim.select))
        {
            yield return null;
        }

        hide_textbox();

        p.can_move = true;
        p.pause_menu_container.SetActive(true);

        p.frames_since_last_interact = 0;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        obtained = SaveSystem.GetBool("chest_" + ID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
