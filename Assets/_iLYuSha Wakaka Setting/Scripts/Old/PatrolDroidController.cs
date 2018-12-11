using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolDroidController : MonoBehaviour
{
    private Transform myTransform;
    public bool run;
    Animator anim;

    void Awake ()
    {
        myTransform = transform;
        anim = GetComponent<Animator>();
        anim.SetBool("Run", run);
    }
	
	void Update ()
    {
        if (!run) return;
        myTransform.position += myTransform.forward * Time.deltaTime * 15;
        if (myTransform.position.x < -1730)
            myTransform.position += new Vector3(1170, 0, 0);
        else if (myTransform.position.x > -560)
            myTransform.position -= new Vector3(1170, 0, 0);

    }
}
