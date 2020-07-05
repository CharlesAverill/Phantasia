using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{

    public AudioSource classic;
    public AudioSource remaster;
    public AudioSource gba;

    // Start is called before the first frame update
    void Start()
    {
        if(SaveSystem.GetBool("classic_music")){
            classic.volume = 1f;
            remaster.gameObject.SetActive(false);
            gba.gameObject.SetActive(false);
        }
        else if(SaveSystem.GetBool("remaster_music")){
            remaster.volume = 1f;
            classic.gameObject.SetActive(false);
            gba.gameObject.SetActive(false);
        }
        else
        {
            gba.volume = 1f;
            classic.gameObject.SetActive(false);
            remaster.gameObject.SetActive(false);
        }
    }
    
    public AudioSource get_active(){
        if(classic.gameObject.active){
            return classic;
        }
        if (gba.gameObject.active)
        {
            return gba;
        }
        return remaster;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
