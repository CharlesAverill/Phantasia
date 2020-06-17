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
    
    public void exit(){
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
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
