using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ekran : MonoBehaviour
{
    private Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        Anim.speed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AnimUp()
    {
        Anim.Play("okey", -1, 0f);
        Anim.speed = 1f;
    }

    public void AnimDown()
    {
        Anim.Play("no", -1, 0f);
        Anim.speed = 2f;
    }
}
