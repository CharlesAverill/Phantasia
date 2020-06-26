using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Transform[] party;
    public List<GameObject> party_clones;

    public string[] party_names;

    public string shopmode;

    public Dictionary<string, int> products;

    public Text prompt_text;
    public Text gil_display_text;
    public Text shop_type_text;

    public GameObject yes_no;
    public GameObject buy_sell_quit;
    public GameObject only_quit;

    private bool yes;
    private bool no;
    private bool buy;
    private bool sell;

    private int inn_clinic_price;
    private int player_gil;

    private SpriteController sc;

    // Start is called before the first frame update
    void Start()
    {
        party_clones = new List<GameObject>();
        products = new Dictionary<string, int>();
        party_names = new string[4];

        for(int i = 0; i < 4; i++)
        {
            party_names[i] = SaveSystem.GetString("player" + (i + 1) + "_name");
        }
        
        //if (shopmode == null)
        shopmode = GlobalControl.instance.shopmode;

        if(shopmode != "inn" && shopmode != "clinic")
        {
            yes_no.SetActive(false);
            buy_sell_quit.SetActive(true);
            only_quit.SetActive(false);
            for(int i = 0; i < GlobalControl.instance.shop_products.Count; i++)
            {
                string prod_name = GlobalControl.instance.shop_products.ElementAt(i).Key;
                int prod_cost = GlobalControl.instance.shop_products.ElementAt(i).Value;
                products.Add(prod_name, prod_cost);
            }
            prompt_text.text = "What do you need?";
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
                int dead = 0;
                foreach(string name in party_names)
                {
                    if(SaveSystem.GetInt(name + "_HP") <= 0)
                    {
                        dead += 1;
                    }
                }
                if (dead > 0)
                {
                    prompt_text.text = "A party member of yours has fallen. Would you like to revive them for " + inn_clinic_price + "G?";
                }
                else {
                    prompt_text.text = "You do not need my help right now.";
                    yes_no.SetActive(false);
                    only_quit.SetActive(true);
                }
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

    public void buy_true()
    {
        buy = true;
        sell = false;
    }

    public void sell_true()
    {
        buy = false;
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
        if((shopmode == "inn" || shopmode == "clinic") && (yes || no))
        {
            if(yes && player_gil < inn_clinic_price)
            {
                Debug.Log("You don't have enough money!");
            }
            else if (yes)
            {
                SaveSystem.SetInt("gil", player_gil - inn_clinic_price);
                for(int i = 0; i < 4; i++)
                {
                    if(SaveSystem.GetInt("player" + (i + 1) + "_HP") > 0)
                        SaveSystem.SetInt("player" + (i + 1) + "_HP", SaveSystem.GetInt("player" + (i + 1) + "_maxHP"));
                }

                GlobalControl.instance.player.map_handler.save_inn();

                SaveSystem.SaveToDisk();
            }

            exit_shop();
        }
    }
}
