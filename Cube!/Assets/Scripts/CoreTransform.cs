using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreTransform : MonoBehaviour {

	//							Public
	public	GameObject[]	objects;
	public	bool			canIMove;

	/*
	 * sides where player is able to move
	 */
	public	bool			forward	=true;
	public	bool			back	=true;
	public	bool			left	=true;
	public	bool			right	=true;
	
	// Update is called once per frame
	void Update () {
		if (canIMove == true) {
			if (Input.GetKeyDown (KeyCode.D)) { // Working in progress- switch wasd to input menager stuff
				if (right != false) {
					transform.position = new Vector3 (transform.position.x + 1, transform.position.y, transform.position.z);
					canIMove = false;
					ArrowOn ();
					SetTrueAgain();
				}
			}
			if (Input.GetKeyDown (KeyCode.A)) {
				if (left != false) {
					transform.position = new Vector3 (transform.position.x - 1, transform.position.y, transform.position.z);
					canIMove = false;
					ArrowOn ();
					SetTrueAgain ();
				}
			}
			if (Input.GetKeyDown (KeyCode.W)) {
				if (forward != false) {
					transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 1);
					canIMove = false;
					ArrowOn ();
					SetTrueAgain();
				}
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				if (back != false) {
					transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 1);
					canIMove = false;
					ArrowOn ();
					SetTrueAgain();
				}
			}
		}
		CanI ();
	}

	void ArrowOn(){
		//	Turning on arrows
		objects = GameObject.FindGameObjectsWithTag ("Arrow");
		foreach(GameObject obj in objects)
		{
			obj.GetComponent < ArrowFly > ().canIFly = true;
			obj.GetComponent < ArrowFly > ().isMoving = true;
		}
	}
	bool CanI(){
		//	Checking is player is able to move again after last movement
		canIMove = false;
		objects = GameObject.FindGameObjectsWithTag ("Arrow");
		foreach (GameObject obj in objects) {
			if (obj.GetComponent< ArrowFly > ().isMoving == true) {
				return false;
			}
		}
		canIMove = true;
		return true;
	}

	void SetTrueAgain(){
		//	After every move to not lock position forever
		forward	=true;
		back	=true;
		left	=true;
		right	=true;
	}
}
