using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFly : MonoBehaviour {

	//							Public
	public	float	movement;
	public	bool	canIFly;
	public	bool	isMoving;

	//							Private
	/*
	 * Basic position of arrow-> begining one
	 * To be sure at that an arrow will not switch position in specific situation
	 * 
	 */
	private	Vector3	basicPosition;
	private float	x;
	private float	y;
	private float	z;

	//	Use this for initialization
	void Start () {
		basicPosition = transform.position;
		x = transform.eulerAngles.x;
		y = transform.eulerAngles.y;
		z = transform.eulerAngles.z;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (canIFly == true) {
			transform.Translate (Vector3.up * Time.deltaTime * movement);
		}
	}

	public void SetBasic (){
		canIFly = false;
		transform.position = basicPosition;
		transform.eulerAngles = new Vector3 (x, y, z);
	}

	private void OnCollisionEnter(Collision detector) {
		/*
		 * script choosing a target and action on colision
		 */
		if(detector.gameObject.CompareTag("Player")){
			GameObject.Find ("Game_UI").GetComponent<Game_UI> ().LooseTheGame();
		}
		if (detector.gameObject.CompareTag ("Killable")){
			isMoving = false;
			SetBasic();
			Destroy (detector.gameObject);
		}
		if (detector.gameObject.CompareTag ("Unkillable")) {
			isMoving = false;
			SetBasic();
		}
		if (detector.gameObject.CompareTag ("Package")) {
			isMoving = false;
			SetBasic();
		}
	}
}
