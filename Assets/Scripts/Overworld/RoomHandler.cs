using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomHandler : MonoBehaviour
{

    public GameObject rooms;
    public GameObject outside_collision;

    // Start is called before the first frame update
    void Start()
    {
        if(SaveSystem.GetBool("inside_of_room")){
            Debug.Log(SaveSystem.GetBool("inside_of_room"));
            rooms.SetActive(true);
            outside_collision.SetActive(false);
        }
        else{
            rooms.SetActive(false);
            outside_collision.SetActive(true);
        }
        GetComponent<TilemapRenderer>().enabled = false;
    }
    
    public void change(){
        rooms.SetActive(!rooms.active);
        outside_collision.SetActive(!outside_collision.active);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
