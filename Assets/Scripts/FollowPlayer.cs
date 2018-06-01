using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	private GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		this.transform.localPosition = new Vector3(player.transform.localPosition.x, -5.5f, 10f); // Holy crap, hard coded numbers are baaaaaaaaad!!!
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.localPosition = new Vector3(player.transform.localPosition.x, -5.5f, 10f);
	}
}
