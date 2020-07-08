using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BridgeTrigger : MonoBehaviour
{

    public ScreenTransition st;
    public MusicHandler mh;
    public PlayerController p;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(loadbridge());
    }

    IEnumerator loadbridge()
    {
        p.can_move = false;
        Transform t = p.move_point;
        while (t.position.y > p.transform.position.y)
            yield return null;
        Destroy(p);
        mh.gameObject.SetActive(false);
        st.transition();
        while (st.filling)
            yield return null;
        SceneManager.LoadSceneAsync("BridgeTitle");
    }
}
