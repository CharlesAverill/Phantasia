using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{

    public static GlobalControl instance;

    public MapHandler mh;

    public Boss boss;
    public bool bossvictory;

    public GameObject overworld_scene_container;
    public GameObject monster_party;

    public PlayerController player;

    public string shopmode;
    public int inn_clinic_price;
    public Dictionary<string, int> shop_products;

    public bool bossmode;

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
        if (bossvictory)
        {
            SaveSystem.SetBool(boss.flag, boss.flagval);

            Destroy(boss.gameObject);

            player.can_move = true;
            player.multiplier = 2f;

            player.pause_menu_container.SetActive(true);

            player.map_handler.save_position();
            player.reh.battling = false;
            bossmode = false;
            bossvictory = false;

            overworld_scene_container.SetActive(true);
        }
    }
}
