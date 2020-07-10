using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    [Serializable]
    public class product_text
    {
        public Text name;
        public Text cost;
    }

    public Transform[] party;
    public List<GameObject> party_clones;

    public string[] party_names;

    public string shopmode;

    public Text prompt_text;
    public Text gil_display_text;
    public Text shop_type_text;

    public GameObject buy_container;
    public GameObject buy_cursor;
    public product_text[] product_Texts;

    public GameObject yes_no;
    public GameObject buy_sell_quit;
    public GameObject only_quit;

    public FadeOut fadeOut;
    public MusicHandler shop_music;
    public MusicHandler inn_music;

    private bool yes;
    private bool no;
    private bool sell;

    private int inn_clinic_price;
    private int player_gil;

    private SpriteController sc;

    int buy_index;

    List<int> dead_players;

    void setup()
    {
        foreach (product_text t in product_Texts)
        {
            t.name.transform.parent.gameObject.SetActive(false);
        }

        yes_no.SetActive(false);
        buy_sell_quit.SetActive(true);
        only_quit.SetActive(false);
        buy_container.SetActive(false);

        for (int i = 0; i < GlobalControl.instance.shop_products.Count; i++)
        {
            string prod_name = GlobalControl.instance.shop_products.ElementAt(i).Key;
            int prod_cost = GlobalControl.instance.shop_products.ElementAt(i).Value;

            product_Texts[i].name.text = prod_name;
            product_Texts[i].cost.text = "" + prod_cost;

            if(prod_name.Length > 1)
                product_Texts[i].name.transform.parent.gameObject.SetActive(true);
        }
        prompt_text.text = "What do you need?";
    }

    // Start is called before the first frame update
    void Start()
    {
        party_clones = new List<GameObject>();
        party_names = new string[4];

        buy_container.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            party_names[i] = SaveSystem.GetString("player" + (i + 1) + "_name");
        }
        
        //if (shopmode == null)
        shopmode = GlobalControl.instance.shopmode;

        if(shopmode != "inn" && shopmode != "clinic")
        {
            setup();
        }
        else
        {
            yes_no.SetActive(true);
            buy_sell_quit.SetActive(false);
            only_quit.SetActive(false);
            inn_clinic_price = GlobalControl.instance.inn_clinic_price;

            if(shopmode == "inn")
            {
                prompt_text.text = "Stay to heal and save your data? A room will be " + inn_clinic_price + "G per night.";
            }
            if(shopmode == "clinic")
            {
                clinic_setup();
            }
        }

        sc = GetComponentInChildren<SpriteController>();
        switch (shopmode)
        {
            case "armor":
                sc.set_character(0);
                shop_type_text.text = "ARMOR";
                break;
            case "b_magic":
                sc.set_character(1);
                shop_type_text.text = "BLACK MAGIC";
                break;
            case "clinic":
                sc.set_character(2);
                shop_type_text.text = "CLINIC";
                break;
            case "inn":
                sc.set_character(3);
                shop_type_text.text = "INN";
                break;
            case "item":
                sc.set_character(4);
                shop_type_text.text = "ITEM";
                break;
            case "oasis":
                sc.set_character(5);
                shop_type_text.text = "OASIS";
                break;
            case "w_magic":
                sc.set_character(6);
                shop_type_text.text = "WHITE MAGIC";
                break;
            case "weapon":
                sc.set_character(7);
                shop_type_text.text = "WEAPON";
                break;
        }
        sc.change_direction("up");

        for (int i = 0; i < 4; i++)
        {
            string player_n = "player" + (i + 1) + "_";
            string job = SaveSystem.GetString("player" + (i + 1) + "_class");
            switch (job)
            {
                case "fighter":
                    GameObject fighter = Instantiate(Resources.Load<GameObject>("party/fighter"), party[i].position, Quaternion.identity);
                    party_clones.Add(fighter);
                    fighter.transform.localScale = party[i].localScale;
                    Destroy(fighter.GetComponent<PartyMember>());
                    break;
                case "black_belt":
                    GameObject bb = Instantiate(Resources.Load<GameObject>("party/black_belt"), party[i].position, Quaternion.identity);
                    party_clones.Add(bb);
                    bb.transform.localScale = party[i].localScale;
                    Destroy(bb.GetComponent<PartyMember>());
                    break;
                case "red_mage":
                    GameObject red_mage = Instantiate(Resources.Load<GameObject>("party/red_mage"), party[i].position, Quaternion.identity);
                    party_clones.Add(red_mage);
                    red_mage.transform.localScale = party[i].localScale;
                    Destroy(red_mage.GetComponent<PartyMember>());
                    break;
                case "thief":
                    GameObject thief = Instantiate(Resources.Load<GameObject>("party/thief"), party[i].position, Quaternion.identity);
                    party_clones.Add(thief);
                    thief.transform.localScale = party[i].localScale;
                    Destroy(thief.GetComponent<PartyMember>());
                    break;
                case "white_mage":
                    GameObject white_mage = Instantiate(Resources.Load<GameObject>("party/white_mage"), party[i].position, Quaternion.identity);
                    party_clones.Add(white_mage);
                    white_mage.transform.localScale = party[i].localScale;
                    Destroy(white_mage.GetComponent<PartyMember>());
                    break;
                case "black_mage":
                    GameObject black_mage = Instantiate(Resources.Load<GameObject>("party/black_mage"), party[i].position, Quaternion.identity);
                    party_clones.Add(black_mage);
                    black_mage.transform.localScale = party[i].localScale;
                    Destroy(black_mage.GetComponent<PartyMember>());
                    break;
            }
        }

        player_gil = SaveSystem.GetInt("gil");

        gil_display_text.text = "G: " + player_gil;
    }

    void clinic_setup()
    {
        dead_players = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            if (SaveSystem.GetInt("player" + (i + 1) + "_HP") <= 0)
                dead_players.Add(i);
        }

        if (dead_players.Count > 0)
        {
            prompt_text.text = "It seems " + SaveSystem.GetString("player" + (dead_players[0] + 1) + "_name") + " has fallen. Would you like me to revive them for " + inn_clinic_price + "G?";
            yes_no.SetActive(true);
        }
        else
        {
            prompt_text.text = "You do not need my help right now.";
            yes_no.SetActive(false);
            only_quit.SetActive(true);
        }
    }

    public void yes_true()
    {
        yes = true;
        no = false;
    }

    public void no_true()
    {
        yes = false;
        no = true;
    }

    public void buy()
    {
        buy_container.SetActive(true);
        buy_cursor.SetActive(true);
        buy_sell_quit.SetActive(false);
        prompt_text.text = "What would you like?";
    }

    public void sell_true()
    {
        sell = true;
    }

    public void exit_shop()
    {
        foreach (GameObject t in party_clones)
        {
            Destroy(t);
        }

        GlobalControl.instance.overworld_scene_container.SetActive(true);
        SceneManager.UnloadScene("Shop");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(CustomInputManager.cim.back))
        {
            if (buy_sell_quit.active || (shopmode == "clinic" || shopmode == "inn"))
            {
                exit_shop();
            }
            else if(buy_container.active)
            {
                setup();
            }
        }

        if(player_gil != SaveSystem.GetInt("gil"))
        {
            player_gil = SaveSystem.GetInt("gil");

            gil_display_text.text = "G: " + player_gil;
        }

        if((shopmode == "inn" || shopmode == "clinic") && (yes || no))
        {
            if(yes && player_gil < inn_clinic_price)
            {
                Debug.Log("You don't have enough money!");
            }
            else if (yes)
            {
                SaveSystem.SetInt("gil", player_gil - inn_clinic_price);
                if(shopmode == "inn")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (SaveSystem.GetInt("player" + (i + 1) + "_HP") > 0)
                            SaveSystem.SetInt("player" + (i + 1) + "_HP", SaveSystem.GetInt("player" + (i + 1) + "_maxHP"));
                    }

                    GlobalControl.instance.player.map_handler.save_inn();

                    SaveSystem.SaveToDisk();

                    prompt_text.text = "Thank you for staying the night! Sleep well!";

                    yes_no.SetActive(false);

                    StartCoroutine(inn_sleep());

                    yes = false;
                    no = false;
                }
                else
                {
                    SaveSystem.SetInt("player" + (dead_players[0] + 1) + "_HP", 1);
                    yes = false;
                    no = false;
                    clinic_setup();
                }
            }
        }
    }

    IEnumerator inn_sleep()
    {

        shop_music.get_active().Stop();
        inn_music.get_active().Play();

        fadeOut.start_fade(true);

        yield return new WaitForSeconds(.5f);

        while (fadeOut.is_fading())
            yield return null;

        fadeOut.start_fade(false);

        yield return new WaitForSeconds(.5f);

        while (inn_music.get_active().isPlaying)
            yield return null;

        exit_shop();
    }

    public void buy_1()
    {
        StartCoroutine(buy_select(0));
    }

    public void buy_2()
    {
        StartCoroutine(buy_select(1));
    }

    public void buy_3()
    {
        StartCoroutine(buy_select(2));
    }

    public void buy_4()
    {
        StartCoroutine(buy_select(3));
    }

    public void buy_5()
    {
        StartCoroutine(buy_select(4));
    }

    IEnumerator buy_select(int index)
    {
        buy_cursor.SetActive(false);

        yes = false;
        no = false;

        string p_name = product_Texts[index].name.text;
        int p_cost = Int32.Parse(product_Texts[index].cost.text);

        prompt_text.text = "Is " + p_cost + " G for a " + p_name + " okay?";
        yes_no.SetActive(true);

        while (!yes && !no)
            yield return null;

        if (yes)
        {
            if(player_gil >= p_cost)
            {
                switch (shopmode)
                {
                    case "item":
                        Dictionary<string, int> items = SaveSystem.GetStringIntDict("items");
                        if (items.ContainsKey(p_name))
                            items[p_name] = items[p_name] + 1;
                        else
                            items.Add(p_name, 1);
                        SaveSystem.SetStringIntDict("items", items);
                        SaveSystem.SetInt("gil", player_gil - p_cost);
                        break;
                    case "weapon":
                        Dictionary<string, int> items1 = SaveSystem.GetStringIntDict("items");
                        if (items1.ContainsKey(p_name))
                            items1[p_name] = items1[p_name] + 1;
                        else
                            items1.Add(p_name, 1);
                        SaveSystem.SetStringIntDict("items", items1);
                        SaveSystem.SetInt("gil", player_gil - p_cost);
                        break;
                    case "armor":
                        Dictionary<string, int> items2 = SaveSystem.GetStringIntDict("items");
                        if (items2.ContainsKey(p_name))
                            items2[p_name] = items2[p_name] + 1;
                        else
                            items2.Add(p_name, 1);
                        SaveSystem.SetStringIntDict("items", items2);
                        SaveSystem.SetInt("gil", player_gil - p_cost);
                        break;
                }
            }
            else
            {
                prompt_text.text = "You don't have enough money";
                yes_no.SetActive(false);
                yield return new WaitForSeconds(1.5f);
            }
        }

        setup();
    }
}
