using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInputManager : MonoBehaviour
{

    public static CustomInputManager cim;

    public KeyCode up { get; set; }
    public KeyCode down { get; set; }
    public KeyCode left { get; set; }
    public KeyCode right { get; set; }
    public KeyCode back { get; set; }
    public KeyCode select { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        if(cim == null)
        {
            DontDestroyOnLoad(gameObject);
            cim = this;
        }
        else if(cim != this)
        {
            Destroy(gameObject);
        }

        up = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("upkey", "W"));
        down = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("downkey", "S"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftkey", "A"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightkey", "D"));
        back = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("backkey", "O"));
        select = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("selectkey", "P"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
