using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : Interactable
{

    SpriteController sc;

    public GameObject boss;

    public Transform cam;

    public string flag;
    public bool flagval;

    public RandomEncounterHandler reh;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (SaveSystem.GetBool(flag) == flagval)
            Destroy(this.gameObject);
        sc = GetComponentInChildren<SpriteController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!sc.walk_animation && !interacting)
        {
            StartCoroutine(sc.walk());
        }
    }

    public bool interacting;

    public IEnumerator interact(PlayerController p)
    {
        interacting = true;

        if (is_player_within_radius(2.5f))
        {
            p.can_move = false;

            Vector3 p_pos = p.gameObject.transform.position;

            Vector3 location = new Vector3(p_pos.x, p_pos.y - 7.5f, p_pos.z);

            display_textbox(location);

            yield return new WaitForSeconds(.6f);
            while (!Input.GetKey(CustomInputManager.cim.select))
            {
                yield return null;
            }

            hide_textbox();

            GlobalControl.instance.boss = this;

            if (!reh.battling)
            {
                yield return StartCoroutine(reh.start_boss_battle(p, boss, flag, flagval, this.gameObject));

                interacting = false;

                p.frames_since_last_interact = 0;
            }
        }

        yield return null;
    }

    bool is_player_within_radius(float radius)
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(transform.position, radius, 1 << LayerMask.NameToLayer("Player"));
        /*
        Collider2D[] NPCs = Physics2D.OverlapCircleAll(transform.position, radius, 1 << LayerMask.NameToLayer("NPC"));
        Collider2D[] warps = Physics2D.OverlapCircleAll(transform.position, radius, 1 << LayerMask.NameToLayer("Warp"));
        return player.Length + (NPCs.Length - 1) + warps.Length > 0;
        */
        return player.Length > 0;
    }
}
