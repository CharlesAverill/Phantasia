using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour
{

    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }
    
    private int counter;

    // Update is called once per frame
    void Update()
    {
        counter += 1;
        
        if(counter == 30){
            text.enabled = !text.enabled;
            counter = 0;
        }
    }
}
