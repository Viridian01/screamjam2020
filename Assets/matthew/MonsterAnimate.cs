using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimate : MonoBehaviour
{
    public GameObject handsCover;
    public GameObject handsOpen;

    public MonsterAI monster;

    private bool kill = false;
    private float killTimer = 0f;
    private GameObject player;
    public Vector3 playerOffset;
    public AudioClip killSound;

    // Start is called before the first frame update
    void Start()
    {
        SetCover(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (kill)
        {
            if(killTimer >= 2f)
            {
                transform.position = Vector3.down * 30f;
                return;
            }

            transform.position = player.transform.TransformPoint(playerOffset);
            killTimer += Time.deltaTime;
            return;
        }
        else
        {
            if (!monster) return;

            if (monster.IsHunting())
            {
                SetCover(false);
            }
            else
            {
                SetCover(true);
            }
        }
    }

    void SetCover(bool cover)
    {
        handsCover.SetActive(cover);

        handsOpen.SetActive(!cover);
    }

    public void OpenEyesDie(GameObject plyr)
    {
        player = plyr;
        if (killSound) AudioSource.PlayClipAtPoint(killSound, transform.position);
        SetCover(false);
        kill = true;
    }
}
