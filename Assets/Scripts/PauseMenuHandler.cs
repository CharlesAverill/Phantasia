using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{

    public GameObject overworld_scene_container;
    public GameObject pausemenu_container;

    public SpriteController[] spriteControllers;
    public Text[] names;
    public Text[] levels;
    public Text[] HPs;
    public Text[] MPs;

    public Text gold;

    public GameObject earth;
    public GameObject fire;
    public GameObject water;
    public GameObject wind;

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

    void Start()
    {
        overworld_scene_container.SetActive(true);
        pausemenu_container.SetActive(false);
    }

    // Start is called before the first frame update
    void setup()
    {
        gold.text = "" + SaveSystem.GetInt("gil");

        if (SaveSystem.GetBool("earth_orb"))
            earth.GetComponent<SpriteController>().change_direction("down");
        else
            earth.GetComponent<SpriteController>().change_direction("up");

        if (SaveSystem.GetBool("fire_orb"))
            fire.GetComponent<SpriteController>().change_direction("down");
        else
            fire.GetComponent<SpriteController>().change_direction("up");

        if (SaveSystem.GetBool("water_orb"))
            water.GetComponent<SpriteController>().change_direction("down");
        else
            water.GetComponent<SpriteController>().change_direction("up");

        if (SaveSystem.GetBool("wind_orb"))
            wind.GetComponent<SpriteController>().change_direction("down");
        else
            wind.GetComponent<SpriteController>().change_direction("up");

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (SceneManager.sceneCount == 1)
                setup();
                on();
        }
    }

    public void off()
    {
        Cursor.visible = false;
        pausemenu_container.SetActive(false);
        overworld_scene_container.SetActive(true);
    }

    public void on()
    {
        overworld_scene_container.SetActive(false);
        pausemenu_container.SetActive(true);
        Cursor.visible = true;
    }

    public void quit()
    {
#if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
