using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveGameFabricator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void title_screen()
    {
        SceneManager.LoadSceneAsync("Title Screen");
        SceneManager.UnloadScene("Save Game Fabricator");
    }

    public void save()
    {
        SaveSystem.SaveToDisk();
    }

    public void delete_save_game()
    {
        File.Delete(Application.persistentDataPath + "/party.bin");
    }

    public void P1Class(Text t)
    {
        SaveSystem.SetString("player1_class", t.text);
    }

    public void P2Class(Text t)
    {
        SaveSystem.SetString("player2_class", t.text);
        Debug.Log(t.text);
    }

    public void P3Class(Text t)
    {
        SaveSystem.SetString("player3_class", t.text);
    }

    public void P4Class(Text t)
    {
        SaveSystem.SetString("player4_class", t.text);
    }

    public void P1HP(Text t)
    {
        int val;

        bool success = Int32.TryParse(t.text, out val);

        if (success)
        {
            SaveSystem.SetInt("player1_HP", val);
            Debug.Log("alright");
        }
    }

    public void P2HP(Text t)
    {
        int val;

        bool success = Int32.TryParse(t.text, out val);

        if (success)
        {
            SaveSystem.SetInt("player2_HP", val);
        }
    }

    public void P3HP(Text t)
    {
        int val;

        bool success = Int32.TryParse(t.text, out val);

        if (success)
        {
            SaveSystem.SetInt("player3_HP", val);
        }
    }

    public void P4HP(Text t)
    {
        int val;

        bool success = Int32.TryParse(t.text, out val);

        if (success)
        {
            SaveSystem.SetInt("player4_HP", val);
        }
    }
}
