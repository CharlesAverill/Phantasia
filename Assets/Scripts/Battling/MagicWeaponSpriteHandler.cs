using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MagicWeaponSpriteHandler : MonoBehaviour
{

    [Serializable]
    public class Weapon
    {
        public string name;
        public Sprite sprite;
    }

    [Serializable]
    public class Spell
    {
        public string name;
        public Sprite sprite1;
        public Sprite sprite2;
    }

    public Weapon[] weapons;
    public Spell[] spells;

    public Sprite get_weapon(string name)
    {
        foreach(Weapon w in weapons)
        {
            if(w.name == name)
            {
                return w.sprite;
            }
        }
        return null;
    }

    public KeyValuePair<Sprite, Sprite> get_spell(string name)
    {
        foreach(Spell s in spells)
        {
            if(s.name == name)
            {
                return new KeyValuePair<Sprite, Sprite>(s.sprite1, s.sprite2);
            }
        }
        return new KeyValuePair<Sprite, Sprite>(null, null);
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
