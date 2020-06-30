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

    static List<Armor> armor;
    static List<Weapon> weapons;

    public Equips()
    {
        armor = new List<Armor>();
        weapons = new List<Weapon>();

        setup_armor();
        setup_weapons();
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
        return new KeyValuePair<string, int>(null, 0);
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
