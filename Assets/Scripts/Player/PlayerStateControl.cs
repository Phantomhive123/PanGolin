using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateControl : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.pause)
        {
            anim.SetBool("Pause", true);
            print("pause is true");
        }
        else
        {
            anim.SetBool("Pause", false);
            print("pause is false");
        }
    }
}
