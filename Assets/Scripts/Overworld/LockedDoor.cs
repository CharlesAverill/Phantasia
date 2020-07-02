using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Interactable
{

    public string ID;
    bool unlocked;

    public BoxCollider2D collider;

    public IEnumerator interact(PlayerController p)
    {
        p.can_move = false;
        p.pause_menu_container.SetActive(false);

        Dictionary<string, int> items = SaveSystem.GetStringIntDict("items");

        Vector3 p_pos = p.gameObject.transform.position;

        Vector3 location = new Vector3(p_pos.x, p_pos.y - 7.5f, p_pos.z);

        if (items.ContainsKey("MYSTIC KEY") && !unlocked)
        {
            dialogue = "Unlocked the door with the MYSTIC KEY";
            Destroy(collider);
            unlocked = true;

            SaveSystem.SetBool("door_" + ID, true);
        }
        else if(!items.ContainsKey("MYSTIC KEY") && !unlocked)
        {
            dialogue = "This door is locked";
        }

        display_textbox(location);

        yield return new WaitForSeconds(.8f);
        while (Input.GetAxisRaw("Submit") == 0)
        {
            yield return null;
        }

        hide_textbox();

        p.can_move = true;
        p.pause_menu_container.SetActive(true);

        p.frames_since_last_interact = 0;
    }

    void OnEnable()
    {
        unlocked = SaveSystem.GetBool("door_" + ID);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
