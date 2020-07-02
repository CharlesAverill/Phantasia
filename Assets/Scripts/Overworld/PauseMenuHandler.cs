using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{
    public PlayerController player;
    public GameObject overworld_scene_container;
    public GameObject pausemenu_container;
    public GameObject status_container;
    public GameObject status_bag;
    public Text status_bag_category;
    public Text[] status_bag_items;
    public GameObject music_container;

    public SpriteController[] spriteControllers;
    public Text[] names;
    public Text[] levels;
    public Text[] HPs;
    public Text[] MPs;

    public GameObject bag_obj;
    public Text[] bag_items;

    public Text[] give_names;
    public GameObject givedrop;
    public GameObject equipparty;

    public GameObject give;

    public GameObject areyousure;
    public Text areyousuretext;

    public Text gold;

    public GameObject earth;
    public GameObject fire;
    public GameObject water;
    public GameObject wind;

    public SpriteController sprite;
    public Text status_class;
    public Text status_name;
    public Text status_level;
    public Text status_exp;
    public Text status_to_level_up;
    public Text status_str;
    public Text status_agl;
    public Text status_int;
    public Text status_vit;
    public Text status_luck;
    public Text status_dmg;
    public Text status_abs;
    public Text status_hit;
    public Text status_evade;

    Dictionary<int, int> get_level_chart()
    {
        Dictionary<int, int> level_chart = new Dictionary<int, int>();

        level_chart.Add(2, 40);
        level_chart.Add(3, 196);
        level_chart.Add(4, 547);
        level_chart.Add(5, 1171);
        level_chart.Add(6, 2146);
        level_chart.Add(7, 3550);
        level_chart.Add(8, 5461);
        level_chart.Add(9, 7957);
        level_chart.Add(10, 11116);
        level_chart.Add(11, 15016);
        level_chart.Add(12, 19753);
        level_chart.Add(13, 25351);
        level_chart.Add(14, 31942);
        level_chart.Add(15, 39586);
        level_chart.Add(16, 48361);
        level_chart.Add(17, 58345);
        level_chart.Add(18, 69617);
        level_chart.Add(19, 82253);
        level_chart.Add(20, 96332);
        level_chart.Add(21, 111932);
        level_chart.Add(22, 129131);
        level_chart.Add(23, 148008);
        level_chart.Add(24, 168639);
        level_chart.Add(25, 191103);
        level_chart.Add(26, 215479);
        level_chart.Add(27, 241843);
        level_chart.Add(28, 270275);
        level_chart.Add(29, 300851);
        level_chart.Add(30, 333651);
        level_chart.Add(31, 366450);
        level_chart.Add(32, 399250);
        level_chart.Add(33, 432049);
        level_chart.Add(34, 464849);
        level_chart.Add(35, 497648);
        level_chart.Add(36, 530448);
        level_chart.Add(37, 563247);
        level_chart.Add(38, 596047);
        level_chart.Add(39, 628846);
        level_chart.Add(40, 661646);
        level_chart.Add(41, 694445);
        level_chart.Add(42, 727245);
        level_chart.Add(43, 760044);
        level_chart.Add(44, 792844);
        level_chart.Add(45, 825643);
        level_chart.Add(46, 858443);
        level_chart.Add(47, 891242);
        level_chart.Add(48, 924042);
        level_chart.Add(49, 956841);
        level_chart.Add(50, 989641);

        return level_chart;
    }

    int get_level_from_exp(int exp)
    {
        Dictionary<int, int> level_chart = get_level_chart();
        int level = 1;

        foreach (KeyValuePair<int, int> entry in level_chart)
        {
            if (entry.Value > exp)
                break;
            else
                level = entry.Key;
        }
        return level;
    }

    int exp_till_level(int exp)
    {
        Dictionary<int, int> level_chart = get_level_chart();
        int level = get_level_from_exp(exp);

        return level_chart[level + 1] - exp;
    }

    void Start()
    {
        overworld_scene_container.SetActive(true);
        pausemenu_container.SetActive(false);
        status_container.SetActive(false);
        bag_obj.SetActive(false);
        give.SetActive(false);
        givedrop.SetActive(false);
        equipparty.SetActive(false);
    }

    void OnEnable()
    {
        status_container.SetActive(false);
        bag_obj.SetActive(false);
        givedrop.SetActive(false);
        equipparty.SetActive(false);
    }

    // Start is called before the first frame update
    void setup()
    {
        gold.text = "" + SaveSystem.GetInt("gil");

        for (int i = 0; i < 4; i++)
        {
            string player_n = "player" + (i + 1) + "_";

            names[i].text = SaveSystem.GetString(player_n + "name");

            levels[i].text = "L " + get_level_from_exp(SaveSystem.GetInt(player_n + "exp"));

            HPs[i].text = "HP\n" + SaveSystem.GetInt(player_n + "HP") + "-" + SaveSystem.GetInt(player_n + "maxHP");

            MPs[i].text = "0-0-0-0\n0-0-0-0";

            string job = SaveSystem.GetString(player_n + "class");

            switch (job)
            {
                case "fighter":
                    spriteControllers[i].set_character(0);
                    break;
                case "knight":
                    spriteControllers[i].set_character(1);
                    break;
                case "thief":
                    spriteControllers[i].set_character(2);
                    break;
                case "ninja":
                    spriteControllers[i].set_character(3);
                    break;
                case "black_belt":
                    spriteControllers[i].set_character(4);
                    break;
                case "master":
                    spriteControllers[i].set_character(5);
                    break;
                case "black_mage":
                    spriteControllers[i].set_character(6);
                    break;
                case "black_wizard":
                    spriteControllers[i].set_character(7);
                    break;
                case "white_mage":
                    spriteControllers[i].set_character(8);
                    break;
                case "white_wizard":
                    spriteControllers[i].set_character(9);
                    break;
                case "red_mage":
                    spriteControllers[i].set_character(10);
                    break;
                case "red_wizard":
                    spriteControllers[i].set_character(11);
                    break;
            }
        }



        if (!SaveSystem.GetBool("earth_orb"))
            earth.GetComponent<SpriteController>().change_direction("down");
        else
            earth.GetComponent<SpriteController>().change_direction("up");

        if (!SaveSystem.GetBool("fire_orb"))
            fire.GetComponent<SpriteController>().change_direction("down");
        else
            fire.GetComponent<SpriteController>().change_direction("up");

        if (!SaveSystem.GetBool("water_orb"))
            water.GetComponent<SpriteController>().change_direction("down");
        else
            water.GetComponent<SpriteController>().change_direction("up");

        if (!SaveSystem.GetBool("wind_orb"))
            wind.GetComponent<SpriteController>().change_direction("down");
        else
            wind.GetComponent<SpriteController>().change_direction("up");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (SceneManager.sceneCount == 1 && player.can_move)
                setup();
                on();
        }
    }

    public void status_1()
    {
        status(0);
    }

    public void status_2()
    {
        status(1);
    }

    public void status_3()
    {
        status(2);
    }

    public void status_4()
    {
        status(3);
    }

    public void bag()
    {
        Dictionary<string, int> items = SaveSystem.GetStringIntDict("items");
        foreach(Text t in bag_items)
        {
            t.text = "";
        }

        int i = 0;

        foreach(KeyValuePair<string, int> kvp in items)
        {
            bag_items[i].text = kvp.Key + " x" + kvp.Value;
            i += 1;
        }

        bag_obj.SetActive(true);
    }

    string status_player_n;

    public void status(int index)
    {
        string player_n = "player" + (index + 1) + "_";
        status_player_n = player_n;
        pausemenu_container.SetActive(false);
        bag_obj.SetActive(false);
        status_bag.SetActive(false);

        string job = SaveSystem.GetString(player_n + "class");

        switch (job)
        {
            case "fighter":
                sprite.set_character(0);
                status_class.text = "FIGHTER";
                break;
            case "knight":
                sprite.set_character(1);
                status_class.text = "KNIGHT";
                break;
            case "thief":
                sprite.set_character(2);
                status_class.text = "THIEF";
                break;
            case "ninja":
                sprite.set_character(3);
                status_class.text = "NINJA";
                break;
            case "black_belt":
                sprite.set_character(4);
                status_class.text = "BLACK BELT";
                break;
            case "master":
                sprite.set_character(5);
                status_class.text = "MASTER";
                break;
            case "black_mage":
                sprite.set_character(6);
                status_class.text = "BLACK MAGE";
                break;
            case "black_wizard":
                sprite.set_character(7);
                status_class.text = "BLACK WIZARD";
                break;
            case "white_mage":
                sprite.set_character(8);
                status_class.text = "WHITE MAGE";
                break;
            case "white_wizard":
                sprite.set_character(9);
                status_class.text = "WHITE WIZARD";
                break;
            case "red_mage":
                sprite.set_character(10);
                status_class.text = "RED MAGE";
                break;
            case "red_wizard":
                sprite.set_character(11);
                status_class.text = "RED WIZARD";
                break;
        }

        status_name.text = SaveSystem.GetString(player_n + "name");
        status_level.text = "LVL " + get_level_from_exp(SaveSystem.GetInt(player_n + "exp"));
        status_exp.text = "" + SaveSystem.GetInt(player_n + "exp");
        status_to_level_up.text = "" + exp_till_level(SaveSystem.GetInt(player_n + "exp"));

        status_str.text = "" + SaveSystem.GetInt(player_n + "strength");
        status_agl.text = "" + SaveSystem.GetInt(player_n + "agility");
        status_int.text = "" + SaveSystem.GetInt(player_n + "intelligence");
        status_vit.text = "" + SaveSystem.GetInt(player_n + "vitality");
        status_luck.text = "" + SaveSystem.GetInt(player_n + "luck");

        status_dmg.text = "NA";
        status_hit.text = "" + (int)(100 * SaveSystem.GetFloat(player_n + "hit_percent"));
        status_abs.text = "NA";
        status_evade.text = "" + (48 + SaveSystem.GetInt(player_n + "agility"));

        status_container.SetActive(true);
    }

    int item_select_index;

    public void select_item_1()
    {
        if (bag_items[0].text == "")
            return;
        item_select_index = 0;
        select();
    }

    public void select_item_2()
    {
        if (bag_items[1].text == "")
            return;
        item_select_index = 1;
        select();
    }

    public void select_item_3()
    {
        if (bag_items[2].text == "")
            return;
        item_select_index = 2;
        select();
    }

    public void select_item_4()
    {
        if (bag_items[3].text == "")
            return;
        item_select_index = 3;
        select();
    }

    public void select_item_5()
    {
        if (bag_items[4].text == "")
            return;
        item_select_index = 4;
        select();
    }

    public void select_item_6()
    {
        if (bag_items[5].text == "")
            return;
        item_select_index = 5;
        select();
    }

    public void select_item_7()
    {
        if (bag_items[6].text == "")
            return;
        item_select_index = 6;
        select();
    }

    public void select_item_8()
    {
        if (bag_items[7].text == "")
            return;
        item_select_index = 7;
        select();
    }

    public void select_item_9()
    {
        if (bag_items[8].text == "")
            return;
        item_select_index = 8;
        select();
    }

    public void select_item_10()
    {
        if (bag_items[9].text == "")
            return;
        item_select_index = 9;
        select();
    }

    public void select_item_11()
    {
        if (bag_items[10].text == "")
            return;
        item_select_index = 10;
        select();
    }

    public void select_item_12()
    {
        if (bag_items[11].text == "")
            return;
        item_select_index = 11;
        select();
    }

    public void select_item_13()
    {
        if (bag_items[12].text == "")
            return;
        item_select_index = 12;
        select();
    }

    public void select_item_14()
    {
        if (bag_items[13].text == "")
            return;
        item_select_index = 13;
        select();
    }

    public void select_item_15()
    {
        if (bag_items[14].text == "")
            return;
        item_select_index = 14;
        select();
    }

    public void select_item_16()
    {
        if (bag_items[15].text == "")
            return;
        item_select_index = 15;
        select();
    }

    public void select()
    {
        for(int i = 0; i < 4; i++)
        {
            give_names[i].text = SaveSystem.GetString("player" + (i + 1) + "_name");
        }
        givedrop.SetActive(true);
    }

    public void drop()
    {
        areyousuretext.text = "Are you sure you want to drop this?";
        areyousure.SetActive(true);
        StartCoroutine(drop_item());
    }

    bool areyousure_yes;
    bool areyousure_no;

    public void areyousureyes()
    {
        areyousure_yes = true;
    }

    public void areyousureno()
    {
        areyousure_no = true;
    }

    IEnumerator drop_item()
    {
        while (!areyousure_yes && !areyousure_no)
        {
            yield return null;
        }

        if (areyousure_yes)
        {
            string name = bag_items[item_select_index].text.Substring(0, bag_items[item_select_index].text.IndexOf(" x"));

            Dictionary<string, int> items = SaveSystem.GetStringIntDict("items");
            if (items[name] == 1)
                items.Remove(name);
            else
                items[name] = items[name] - 1;
            SaveSystem.SetStringIntDict("items", items);
        }

        areyousure.SetActive(false);
        givedrop.SetActive(false);

        areyousure_yes = false;
        areyousure_no = false;

        bag();
    }

    public void give_button()
    {
        give.SetActive(true);
        StartCoroutine(give_to_player());
    }

    int give_index = -1;

    public void give_to_player_1()
    {
        give_index = 0;
    }

    public void give_to_player_2()
    {
        give_index = 1;
    }

    public void give_to_player_3()
    {
        give_index = 2;
    }

    public void give_to_player_4()
    {
        give_index = 3;
    }

    IEnumerator give_to_player()
    {
        givedrop.SetActive(false);

        while(give_index == -1)
        {
            yield return null;
        }

        string item_name = bag_items[item_select_index].text.Substring(0, bag_items[item_select_index].text.IndexOf(" x"));

        string category = new Equips().item_category(item_name);

        switch (category)
        {
            case "weapon":
                List<string> weapons = SaveSystem.GetStringList("player" + (give_index + 1) + "_weapons_inventory");
                weapons.Add(item_name);
                SaveSystem.SetStringList("player" + (give_index + 1) + "_weapons_inventory", weapons);

                Dictionary<string, int> party_items = SaveSystem.GetStringIntDict("items");
                int count = party_items[item_name];
                if (count == 1)
                    party_items.Remove(item_name);
                else
                    party_items[item_name] = party_items[item_name] - 1;

                SaveSystem.SetStringIntDict("items", party_items);

                break;
            case "armor":
                List<string> armor = SaveSystem.GetStringList("player" + (give_index + 1) + "_armor_inventory");
                armor.Add(item_name);
                SaveSystem.SetStringList("player" + (give_index + 1) + "_armor_inventory", armor);

                Dictionary<string, int> party_items1 = SaveSystem.GetStringIntDict("items");
                int count1 = party_items1[item_name];
                if (count1 == 1)
                    party_items1.Remove(item_name);
                else
                    party_items1[item_name] = party_items1[item_name] - 1;

                SaveSystem.SetStringIntDict("items", party_items1);

                break;
        }

        give.SetActive(false);
        givedrop.SetActive(false);

        bag();

        give_index = -1;
    }

    public void status_weapon()
    {
        equipparty.SetActive(false);
        status_bag_category.text = "WEAPONS";

        foreach(Text t in status_bag_items)
        {
            t.text = "";
        }

        List<string> weapons = SaveSystem.GetStringList(status_player_n + "weapons_inventory");

        for(int i = 1; i < weapons.Count; i++)
        {
            string prefix = "";
            if (SaveSystem.GetString(status_player_n + "weapon") == weapons[i])
                prefix = "E- ";
            status_bag_items[i - 1].text = prefix + weapons[i];
        }

        status_bag.SetActive(true);
    }

    public void status_armor()
    {
        equipparty.SetActive(false);
        status_bag_category.text = "ARMOR";

        foreach (Text t in status_bag_items)
        {
            t.text = "";
        }

        List<string> armor = SaveSystem.GetStringList(status_player_n + "armor_inventory");

        Equips eq = new Equips();

        for (int i = 1; i < armor.Count; i++)
        {
            string category = eq.get_armor(armor[i]).category;
            string prefix = "";

            if (SaveSystem.GetString(status_player_n + category) == armor[i])
                prefix = "E- ";

            status_bag_items[i - 1].text = prefix + armor[i];
        }

        status_bag.SetActive(true);
    }

    int item_select_status_index;

    public void select_status_item_1()
    {
        item_select_status_index = 0;
        string name = status_bag_items[0].text;
        if (name == "")
            return;
        equipparty.SetActive(true);
    }

    public void select_status_item_2()
    {
        item_select_status_index = 1;
        string name = status_bag_items[1].text;
        if (name == "")
            return;
        equipparty.SetActive(true);
    }

    public void select_status_item_3()
    {
        item_select_status_index = 2;
        string name = status_bag_items[2].text;
        if (name == "")
            return;
        equipparty.SetActive(true);
    }
    public void select_status_item_4()
    {
        item_select_status_index = 3;
        string name = status_bag_items[3].text;
        if (name == "")
            return;
        equipparty.SetActive(true);
    }

    public void equip()
    {
        Equips eq = new Equips();

        string name = status_bag_items[item_select_status_index].text;
        string category = eq.item_category(name);

        if(name.Contains("E- "))
        {
            category = eq.item_category(name.Substring(3));
            switch (category)
            {
                case "armor":
                    string armor_type = eq.get_armor(name).category;

                    switch (armor_type)
                    {
                        case "armor":
                            SaveSystem.SetString(status_player_n + "armor", "");
                            break;
                        case "shield":
                            SaveSystem.SetString(status_player_n + "shield", "");
                            break;
                        case "helmet":
                            SaveSystem.SetString(status_player_n + "helmet", "");
                            break;
                        case "glove":
                            SaveSystem.SetString(status_player_n + "glove", "");
                            break;
                    }

                    status_bag_items[item_select_status_index].text = name.Substring(3);

                    break;
                case "weapon":
                    SaveSystem.SetString(status_player_n + "weapon", "");
                    status_bag_items[item_select_status_index].text = name.Substring(3);
                    break;
            }
        }
        else
        {
            switch (category)
            {
                case "armor":
                    string armor_type = eq.get_armor(name).category;

                    string player_class = SaveSystem.GetString(status_player_n + "class");

                    if (eq.can_equip_armor(eq.get_armor(name), player_class))
                    {

                        if (SaveSystem.GetString(status_player_n + armor_type) != "")
                        {
                            string already_equipped = SaveSystem.GetString(status_player_n + armor_type);
                            foreach(Text t in status_bag_items)
                            {
                                if (t.text == "E- " + already_equipped)
                                {
                                    t.text = t.text.Substring(3, t.text.Length - 3);
                                    break;
                                }
                            }
                        }

                        switch (armor_type)
                        {
                            case "armor":
                                SaveSystem.SetString(status_player_n + "armor", name);
                                break;
                            case "shield":
                                SaveSystem.SetString(status_player_n + "shield", name);
                                break;
                            case "helmet":
                                SaveSystem.SetString(status_player_n + "helmet", name);
                                break;
                            case "glove":
                                SaveSystem.SetString(status_player_n + "glove", name);
                                break;
                        }

                        status_bag_items[item_select_status_index].text = "E- " + name;
                    }
                    break;
                case "weapon":
                    string player_class1 = SaveSystem.GetString(status_player_n + "class");
                    if(eq.can_equip_weapon(eq.get_weapon(name), player_class1))
                    {
                        if (SaveSystem.GetString(status_player_n + "weapon") != "")
                        {
                            string already_equipped = SaveSystem.GetString(status_player_n + "weapon");
                            foreach (Text t in status_bag_items)
                            {
                                if (t.text == "E- " + already_equipped)
                                {
                                    t.text = t.text.Substring(3, t.text.Length - 3);
                                    break;
                                }
                            }
                        }
                        SaveSystem.SetString(status_player_n + "weapon", status_bag_items[item_select_status_index].text);
                        status_bag_items[item_select_status_index].text = "E- " + status_bag_items[item_select_status_index].text;
                    }
                    break;
            }
        }

        equipparty.SetActive(false);
    }

    public void send_to_party()
    {
        Equips eq = new Equips();

        string name = status_bag_items[item_select_status_index].text;
        if (name == "")
            return;
        if (name.Contains("E- "))
            name = name.Substring(3);
        string category = eq.item_category(name);

        //Unequip
        switch (category)
        {
            case "weapon":
                if (name == SaveSystem.GetString(status_player_n + "weapon"))
                {
                    SaveSystem.SetString(status_player_n + "weapon", "");
                }
                break;
            case "armor":
                if (name == SaveSystem.GetString(status_player_n + "armor"))
                {
                    SaveSystem.SetString(status_player_n + "armor", "");
                }
                if (name == SaveSystem.GetString(status_player_n + "helmet"))
                {
                    SaveSystem.SetString(status_player_n + "helmet", "");
                }
                if (name == SaveSystem.GetString(status_player_n + "shield"))
                {
                    SaveSystem.SetString(status_player_n + "shield", "");
                }
                if (name == SaveSystem.GetString(status_player_n + "glove"))
                {
                    SaveSystem.SetString(status_player_n + "weapon", "");
                }
                break;
        }

        Dictionary<string, int> items = SaveSystem.GetStringIntDict("items");
        if (items.ContainsKey(name))
        {
            items[name] = items[name] + 1;
        }
        else
            items.Add(name, 1);

        SaveSystem.SetStringIntDict("items", items);

        equipparty.SetActive(false);

        switch (category)
        {
            case "armor":
                List<string> armor = SaveSystem.GetStringList(status_player_n + "armor_inventory");
                armor.Remove(name);
                SaveSystem.SetStringList(status_player_n + "armor_inventory", armor);
                status_armor();
                break;
            case "weapon":
                List<string> weapons = SaveSystem.GetStringList(status_player_n + "weapons_inventory");
                weapons.Remove(name);
                SaveSystem.SetStringList(status_player_n + "weapons_inventory", weapons);
                status_weapon();
                break;
        }

        status_bag_items[item_select_status_index].text = "";
    }

    public void status_off()
    {
        if (equipparty.active)
        {
            equipparty.SetActive(false);
        }
        if (status_bag.active)
        {
            status_bag.SetActive(false);
        }
        else
        {
            pausemenu_container.SetActive(true);
            bag_obj.SetActive(false);
            give.SetActive(false);
            givedrop.SetActive(false);
            status_container.SetActive(false);
        }
    }

    public void off()
    {
        if(give.active)
        {
            give.SetActive(false);
        }
        else if(givedrop.active)
        {
            givedrop.SetActive(false);
        }
        else if(bag_obj.active)
        {
            bag_obj.SetActive(false);
        }
        else
        {
            Cursor.visible = false;
            pausemenu_container.SetActive(false);
            status_container.SetActive(false);
            music_container.SetActive(false);
            overworld_scene_container.SetActive(true);
            bag_obj.SetActive(false);
            givedrop.SetActive(false);
        }
    }

    public void on()
    {
        overworld_scene_container.SetActive(false);
        pausemenu_container.SetActive(true);
        music_container.SetActive(true);
        Cursor.visible = true;
    }

    public void items()
    {
        Dictionary<string, int> dict = SaveSystem.GetStringIntDict("items");
        foreach (KeyValuePair<string, int> kvp in dict)
            Debug.Log(kvp.Key + " x" + kvp.Value);
    }

    public void quit()
    {
        areyousuretext.text = "Do you want to quit? Any unsaved progress will be lost.";
        areyousure.SetActive(true);
        StartCoroutine(quit_coroutine());
    }

    IEnumerator quit_coroutine()
    {
        while(!areyousure_yes && !areyousure_no)
        {
            yield return null;
        }

        if (areyousure_yes)
        {
            actually_quit_application();
        }
        else
        {
            areyousure.SetActive(false);
        }

        areyousure_yes = false;
        areyousure_no = false;

        yield return null;
    }

    void actually_quit_application()
    {
#if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
