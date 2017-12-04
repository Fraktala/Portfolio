using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepItOnPosition : MonoBehaviour {

	/*
	* It wanted to be "Keep It On Eye!" but something went wrong... :D
	* Script making walls unpushable. That mean arrow or other force cant push it away
	*/

	//Set begining position
	private	Vector3	position;
	private float	x;
	private float	y;
	private float	z;

	// Use this for initialization
	void Start () {
		//set values
		position = transform.position;
		x = transform.eulerAngles.x;
		y = transform.eulerAngles.y;
		z = transform.eulerAngles.z;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = position;
		transform.eulerAngles = new Vector3 (x, y, z);
	}
}
