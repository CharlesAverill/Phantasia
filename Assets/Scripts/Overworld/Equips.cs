using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equips
{
    public class Armor
    {
        public string name;
        public string category;
        public int absorb;
        public string spell;
        public float evade_cost;
        public List<string> resistances;
        public string[] equip_by;
        public int cost;

        public Armor(string n, string cat, int d, string s, float e, List<string> r, string[] eq, int c)
        {
            name = n;
            category = cat;
            absorb = d;
            spell = s;
            evade_cost = e;
            resistances = r;
            equip_by = eq;
            cost = c;
        }
    }

    public class Weapon
    {
        public string name;
        public int damage;
        public float hit;
        public float crit;
        public string spell;
        public string element;
        public string[] equip_by;
        public int cost;

        public Weapon(string n, int d, float h, float cr, string s, string el, string[] eq, int c)
        {
            name = n;
            damage = d;
            hit = h;
            crit = c;
            spell = s;
            element = el;
            equip_by = eq;
            cost = c;
        }
    }

    public class Item
    {
        public string name;
        public int cost;

        public Item(string n, int c)
        {
            name = n;
            cost = c;
        }
    }

    public class Spell
    {
        public string name;
        public string type;
        public int level;
        public int power;
        public float hit;
        public bool multi_target;
        public bool hit_allies;
        public bool self;
        public string status;
        public string element;
        public int cost;
        public string[] learn_by;

        public Spell(string n, string t, int l, int p, float h, bool m, bool ha, bool sf, string s, string e, int cst, string[] lb)
        {
            name = n;
            type = t;
            level = l;
            power = p;
            hit = h;
            multi_target = m;
            hit_allies = ha;
            self = sf;
            status = s;
            element = e;
            cost = cst;
            learn_by = lb;
        }
    }

    static List<Armor> armor;
    static List<Weapon> weapons;
    static List<Item> items;
    static List<Spell> spells;

    public Equips()
    {
        armor = new List<Armor>();
        weapons = new List<Weapon>();
        items = new List<Item>();
        spells = new List<Spell>();

        setup_armor();
        setup_weapons();
        setup_items();
        setup_spells();
    }

    void setup_armor()
    {
        armor.Add(new Armor("Cloth", "armor", 1, "", .2f, new List<string>(), new string[] { "fighter", "knight", "thief", "ninja", "black_belt", "master", "black_mage", "black_wizard", "red_mage", "red_wizard", "white_mage", "white_wizard" }, 10));
        armor.Add(new Armor("Wooden Armor", "armor", 4, "", .8f, new List<string>(), new string[] { "fighter", "knight", "thief", "ninja", "black_belt", "master", "red_mage", "red_wizard" }, 50));
        armor.Add(new Armor("Chain Armor", "armor", 15, "", .15f, new List<string>(), new string[] { "fighter", "knight", "ninja", "red_mage", "red_wizard" }, 80));
        armor.Add(new Armor("Iron Armor", "armor", 24, "", .23f, new List<string>(), new string[] { "fighter", "knight", "ninja" }, 800));
    }

    void setup_weapons()
    {
        string[] equip_nunchuck = new string[] { "black_belt", "ninja", "master" };
        string[] equip_dagger = new string[] { "fighter", "knight", "thief", "ninja", "black_mage", "black_wizard", "red_mage", "red_wizard" };
        string[] equip_staff = new string[] { "fighter", "knight", "ninja", "black_belt", "master", "black_mage", "black_wizard", "red_mage", "red_wizard", "white_mage", "white_wizard" };
        string[] equip_special_sword = new string[] { "fighter", "knight", "thief", "ninja", "red_mage", "red_wizard" };
        string[] equip_hammer = new string[] { "fighter", "knight", "ninja", "white_mage", "white_wizard" };

        weapons.Add(new Weapon("Wooden Nunchuck", 12, 0f, .01f, "", "", equip_nunchuck, 10));
        weapons.Add(new Weapon("Small Dagger", 5, .1f, .015f, "", "", equip_dagger, 5));
        weapons.Add(new Weapon("Wooden Staff", 6, 0f, .02f, "", "", equip_staff, 5));
        weapons.Add(new Weapon("Rapier", 9, .05f, .052f, "", "", equip_special_sword, 10));
        weapons.Add(new Weapon("Iron Hammer", 9, 0f, .03f, "", "", equip_hammer, 10));
    }

    void setup_items()
    {
        items.Add(new Item("HEAL Potion", 60));
        items.Add(new Item("PURE Potion", 75));
        items.Add(new Item("Tent", 75));
    }

    void setup_spells()
    {
        string[] black_broad = new string[] { "black_mage", "black_wizard", "ninja", "red_mage", "red_wizard" };
        string[] white_broad_small = new string[] { "white_mage", "white_wizard", "red_wizard", "knight" };
        string[] white_broad_large = new string[] { "white_mage", "white_wizard", "red_mage", "red_wizard", "knight" };
        string[] white_only = new string[] { "white_mage", "white_wizard" };

        spells.Add(new Spell("LIT", "black", 1, 20, .96f, false, false, false, "", "lightning", 100, black_broad));
        spells.Add(new Spell("FIRE", "black", 1, 20, .96f, false, false, false, "", "fire", 100, black_broad));
        spells.Add(new Spell("LOCK", "black", 1, 0, 1.15f, false, false, false, "", "", 100, black_broad));
        spells.Add(new Spell("SLEP", "black", 1, 0, .96f, true, false, false, "Sleep", "status", 100, black_broad));

        spells.Add(new Spell("RUSE", "white", 1, 0, -1f, false, false, true, "", "", 100, white_broad_small));
        spells.Add(new Spell("HEAL", "white", 1, 0, -1f, false, true, false, "", "", 100, white_broad_large));
        spells.Add(new Spell("HARM", "white", 1, 40, .96f, true, false, false, "", "", 100, white_only));
        spells.Add(new Spell("FOG", "white", 1, 0, -1f, false, false, true, "", "", 100, white_broad_large));
    }

    public void communal_to_personal(string type, string equip_name, int p_index)
    {
        List<string> type_inventory = SaveSystem.GetStringList(type);
        type_inventory.Remove(equip_name);
        SaveSystem.SetStringList(type, type_inventory);

        List<string> p_inventory = SaveSystem.GetStringList("player" + (p_index + 1) + "_" + type + "_inventory");
        p_inventory.Add(equip_name);
        SaveSystem.SetStringList("player" + (p_index + 1) + "_" + type + "_inventory", p_inventory);
    }

    public Armor get_armor(string name)
    {
        foreach(Armor a in armor)
        {
            if (a.name == name)
                return a;
        }
        return null;
    }

    public Weapon get_weapon(string name)
    {
        foreach (Weapon w in weapons)
        {
            if (w.name == name)
                return w;
        }
        return null;
    }

    public KeyValuePair<string, int> name_price(string name)
    {
        foreach (Weapon w in weapons)
        {
            if (w.name == name)
                return new KeyValuePair<string, int>(w.name, w.cost);
        }
        foreach (Armor a in armor)
        {
            if (a.name == name)
                return new KeyValuePair<string, int>(a.name, a.cost);
        }
        foreach(Item i in items)
        {
            if (i.name == name)
                return new KeyValuePair<string, int>(i.name, i.cost);
        }
        foreach(Spell s in spells)
        {
            if (s.name == name)
                return new KeyValuePair<string, int>(s.name, s.cost);
        }
        return new KeyValuePair<string, int>(null, 0);
    }

    public string item_category(string name)
    {
        foreach (Weapon w in weapons)
        {
            if (w.name == name)
                return "weapon";
        }
        foreach (Armor a in armor)
        {
            if (a.name == name)
                return "armor";
        }
        foreach (Item i in items)
        {
            if (i.name == name)
                return "item";
        }
        foreach (Spell s in spells)
        {
            if (s.name == name)
                return "spell";
        }
        return "idk";
    }

    public int sum_armor(int index)
    {
        string player_n = "player" + (index + 1) + "_";

        Armor shield = get_armor(SaveSystem.GetString(player_n + "shield"));
        Armor helmet = get_armor(SaveSystem.GetString(player_n + "helmet"));
        Armor arm = get_armor(SaveSystem.GetString(player_n + "armor"));
        Armor glove = get_armor(SaveSystem.GetString(player_n + "glove"));

        int total = 0;

        if (shield != null)
            total += shield.absorb;
        if (helmet != null)
            total += helmet.absorb;
        if (arm != null)
            total += arm.absorb;
        if (glove != null)
            total += glove.absorb;

        return total;
    }
}
