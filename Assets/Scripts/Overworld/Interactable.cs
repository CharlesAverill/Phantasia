using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{

    public string dialogue;
    public Sprite box;
    
    public GameObject text_box;

    // Start is called before the first frame update
    void Start()
    {
        text_box.SetActive(false);
        
        text_box.GetComponentInChildren<Text>().text = dialogue;
    }
    
    public void display_textbox(Vector3 pos){
        text_box.transform.position = pos;
        text_box.SetActive(true);
    }
    
    public void hide_textbox(){
        text_box.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
