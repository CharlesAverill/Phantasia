using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour
{
    public bool monster_mode;
    public bool shop_mode;
    public EventSystem event_system;
    
    public GameObject[] buttons;
    public GameObject[] monsters;
    
    private List<GameObject> active_list;
    
    private GameObject[] active_array;
    
    public int active;
    
    public int frame = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        active = 0;
        if(monster_mode){
            active_array = monsters;
            //monsters = transform.parent.GetComponentsInChildren<Monster>();
        }
        else{
            active_array = buttons;
        }
        
        active_list = active_array.OfType<GameObject>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(monster_mode){
            List<GameObject> to_remove = new List<GameObject>();
            foreach(GameObject obj in active_list){
                if(obj.GetComponent<Monster>().HP <= 0){
                    to_remove.Add(obj);
                }
            }

            int x = to_remove.Count;
            for(int i = 0; i < x; i++)
            {
                remove_from_list(to_remove[i]);
                x = to_remove.Count;
            }
        }

        if(active < active_list.Count)
        {
            event_system.SetSelectedGameObject(active_list[active]);
        }
        frame = frame + 1;
        if(frame >= 20){
        
            float ver = Input.GetAxisRaw("Vertical");
            
            if(ver == 1f){
                active = active - 1;
                if(active < 0){
                    active = active_list.Count - 1;
                }
                frame = 0;
            }
            
            else if(ver == -1f){
                active = active + 1;
                if(active >= active_list.Count){
                    active = 0;
                }
                frame = 0;
            }
            
            else{
                frame = 35;
            }
            
            float xoffset = 0f;
            float yoffset = 0f;
            
            if(monster_mode){
                xoffset = -3.5f;
                yoffset = -0.4f;
            }
            else if (shop_mode)
            {
                xoffset = -2.725f;
                yoffset = -1.05f;
            }
            else{
                xoffset = -4.5f;
                yoffset = -0.4f;
            }
            
            
            transform.position = new Vector3(active_list[active].transform.position.x + xoffset, active_list[active].transform.position.y + yoffset, 1f);
        }
    }
    
    public void remove_from_list(GameObject obj){
        active = 0;
        active_list.Remove(obj);
    }
    
    public Monster get_monster(){
        return active_list[active].GetComponent<Monster>();
    }
    
    public Button get_button(){
        return buttons[active].GetComponent<Button>();
    }
}
