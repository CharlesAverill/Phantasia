using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WarpTiles : MonoBehaviour
{
    
    public GameObject warp_to;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TilemapRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
