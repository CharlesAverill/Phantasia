using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{

    public static GlobalControl instance;

    public GameObject overworld_scene_container;
    public GameObject monster_party;

    public PlayerController player;

    public string shopmode;
    public int inn_clinic_price;
    public Dictionary<string, int> shop_products;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
