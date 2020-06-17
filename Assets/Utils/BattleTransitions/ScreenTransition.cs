using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class ScreenTransition : MonoBehaviour
{
    
    public Material mat;
    
    float fill_value = 0f;
    public bool filling = false;
    
    public bool unfilling = false;
    
    int wait_frames;
    bool wait;
    
    void Awake(){
        mat.SetFloat("_Cutoff", 0f);
        wait_frames = 0;
        wait = false;
    }
    
    void Update(){
    
        wait_frames = wait_frames + 1;
        
        if(wait_frames >= 30){
            wait_frames = 35;
        }
    
        if(filling){
            fill_value = Mathf.Lerp(mat.GetFloat("_Cutoff"), 1f, 8*Time.deltaTime);
            mat.SetFloat("_Cutoff", fill_value);
            if(fill_value >= .98f){
                filling = false;
                mat.SetFloat("_Cutoff", 1f);
            }
        }
        if(unfilling && wait_frames >= 30){
            fill_value = Mathf.Lerp(mat.GetFloat("_Cutoff"), 0f, 4*Time.deltaTime);
            mat.SetFloat("_Cutoff", fill_value);
            if(fill_value <= .02f){
                unfilling = false;
                wait_frames = 0;
                wait = false;
                
                mat.SetFloat("_Cutoff", 0f);
            }
        }
        if(fill_value >= .98f && !wait){
            //unfilling = true;
            filling = false;
            
            wait = true;
            wait_frames = 0;
        }
    }
    
    public void transition(){
        filling = true;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (mat != null){
            Graphics.Blit(src, dst, mat);
        }
    }
}
