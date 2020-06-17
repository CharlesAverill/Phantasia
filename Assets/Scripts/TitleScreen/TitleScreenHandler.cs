using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenHandler : MonoBehaviour
{
    
    public LoadingCircle loading_circle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void start_game(){
        StartCoroutine(load());
    }
    
    IEnumerator load(){
        loading_circle.start_loading_circle();
        yield return new WaitForSeconds(.75f);
        SceneManager.LoadSceneAsync("Overworld");
    }
}
