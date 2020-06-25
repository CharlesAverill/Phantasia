using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ShopWarp : MonoBehaviour
{
    public PlayerController player;

    public string shopmode;
    public int inn_clinic_price;

    [Serializable]
    public class product
    {
        public string name;
        public int price;
    }

    public product[] products;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool shopping;

    public IEnumerator warp()
    {
        shopping = true;

        player.can_move = false;

        GlobalControl.instance.shopmode = shopmode;
        GlobalControl.instance.inn_clinic_price = inn_clinic_price;
        if (GlobalControl.instance.shop_products == null)
        {
            GlobalControl.instance.shop_products = new Dictionary<string, int>();
        }
        foreach (product p in products)
        {
            GlobalControl.instance.shop_products.Add(p.name, p.price);
        }

        int countLoaded = SceneManager.sceneCount;
        if (countLoaded == 1)
        {
            GlobalControl.instance.overworld_scene_container.SetActive(false);

            SceneManager.LoadScene("Shop", LoadSceneMode.Additive);

            player.sc.change_direction("down");

            while (SceneManager.sceneCount > 1)
            {
                yield return null;
            }

            GlobalControl.instance.overworld_scene_container.SetActive(true);
        }

        countLoaded = SceneManager.sceneCount;

        while (countLoaded > 1)
        {
            countLoaded = SceneManager.sceneCount;
            yield return null;
        }

        player.can_move = true;

        shopping = false;

        yield return null;
    }
}
