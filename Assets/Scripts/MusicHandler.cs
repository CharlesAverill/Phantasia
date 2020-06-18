using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{

    public AudioSource classic;
    public AudioSource remaster;

    // Start is called before the first frame update
    void Start()
    {
        if(SaveSystem.GetBool("classic_music")){
            classic.volume = 1f;
            remaster.gameObject.SetActive(false);
        }
        else{
            remaster.volume = 1f;
            classic.gameObject.SetActive(false);
        }
    }
    
    public AudioSource get_active(){
        if(classic.gameObject.active){
            return classic;
        }
        return remaster;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
