using UnityEngine;

public class LoadingCircle : MonoBehaviour
{
    
    public GameObject progress;

    private RectTransform rectComponent;
    public float rotateSpeed = 200f;
    
    private bool turning;

    private void Start()
    {
        turning = false;
        rectComponent = progress.GetComponent<RectTransform>();
        progress.SetActive(false);
    }
    
    public void start_loading_circle(){
        progress.SetActive(true);
        turning = true;
    }

    private void Update()
    {
        if(turning){
            rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
        }
    }
}