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
            else if (item)
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
            else if (weapon)
            {
                dialogue = "Obtained " + weapon_val;

                List<string> weapons = SaveSystem.GetStringList("weapons");
                weapons.Add(weapon_val);
                SaveSystem.SetStringList("weapons", weapons);
            }
            else if (armor)
            {
                dialogue = "Obtained " + armor_val;

                List<string> armors = SaveSystem.GetStringList("armor");
                armors.Add(armor_val);
                SaveSystem.SetStringList("armor", armors);
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

        Debug.Log("displaying text");

        display_textbox(location);

        yield return new WaitForSeconds(1.5f);
        while (Input.GetAxisRaw("Submit") == 0)
        {
            yield return null;
        }

        hide_textbox();

        p.can_move = true;
        p.pause_menu_container.SetActive(true);

        p.frames_since_last_interact = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        obtained = SaveSystem.GetBool("chest_" + ID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
