using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BridgeTitleHandler : MonoBehaviour
{

    public Sprite[] sprites;
    public string[] strings;

    public SpriteRenderer sr;

    public Text display_text;

    // Start is called before the first frame update
    void Start()
    {
        display_text.text = "";
        StartCoroutine(process());
    }

    IEnumerator process()
    {
        for(int i = 0; i < 13; i++)
        {
            sr.sprite = sprites[i];
            yield return new WaitForSeconds(.25f);
        }

        for(int i = 0; i < strings.Length; i++)
        {
            if (i > 3)
                display_text.alignment = TextAnchor.UpperCenter;
            if (i == 8)
                display_text.alignment = TextAnchor.UpperLeft;

            string s = strings[i];

            display_text.text = s.Replace("<br>", "\n"); ;
            yield return new WaitForSeconds(8f);
        }

        while (!Input.GetKeyDown(CustomInputManager.cim.select))
            yield return null;

        display_text.text = "";

        for (int i = 11; i > -1; i--)
        {
            sr.sprite = sprites[i];
            yield return new WaitForSeconds(.25f);
        }
        sr.enabled = false;

        SceneManager.LoadSceneAsync("Title Screen");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
