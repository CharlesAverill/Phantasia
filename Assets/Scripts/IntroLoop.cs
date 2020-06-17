using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLoop : MonoBehaviour
{
    
    public float loop_start_seconds;
    public float loop_end_seconds;
    
    AudioSource source;
    
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(source.time >= loop_end_seconds){
            source.time = loop_start_seconds;
        }
    }
}
