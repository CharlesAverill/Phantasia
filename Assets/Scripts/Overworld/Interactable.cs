using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{

    public string dialogue;
    
    public GameObject text_box;

    GameObject tb_instance;

    // Start is called before the first frame update
    void Start()
    {

    }
    
    public void display_textbox(Vector3 pos){
        tb_instance = Instantiate(text_box, pos, Quaternion.identity);
        if (dialogue.Length >= 91)
            tb_instance.GetComponentInChildren<Text>().fontSize = 26;
        tb_instance.GetComponentInChildren<Text>().text = dialogue;
        tb_instance.SetActive(true);
    }
    
    public void hide_textbox(){
        Destroy(tb_instance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
