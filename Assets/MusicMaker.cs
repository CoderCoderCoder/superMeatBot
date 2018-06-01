using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMaker : MonoBehaviour {

    public AudioClip jump, die;
    public AudioSource source;

    public static MusicMaker instance;

    // Use this for initialization
    void Start () {
        instance = this;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayDie()
    {
        source.PlayOneShot(die);
    }
    public void PlayJump()
    {
        source.PlayOneShot(jump);
    }
}
