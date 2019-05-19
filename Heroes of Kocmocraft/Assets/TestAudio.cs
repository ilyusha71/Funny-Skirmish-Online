using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudio : MonoBehaviour
{
    AudioSource ass;
    public AudioClip ccc;
    // Start is called before the first frame update
    void Start()
    {
        ass = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            ass.PlayOneShot(ccc);
    }
}
