using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreMovement : MonoBehaviour {
	//							Public
	public  Transform  	target;		//add 2th part of player to add his position
	public	float		speed;
	public	GameObject	obj;		//add 2th part of player to add his object

	//							Private
	private	float		setZero;

	// Use this for initialization
	void Start () {
		setZero = 0;
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
		//	Set to zero to prevent skewing
		transform.eulerAngles = new Vector3 (setZero, setZero, setZero);
	}

	void Move()
	{
		//	function making move
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, target.position, step);
	}
	private void OnCollisionEnter(Collision detector) {
		//	colision detecting and making events on it
		if (detector.gameObject.CompareTag ("Finish")) {
			GameObject.Find ("Game_UI").GetComponent<Game_UI> ().WinTheGame();
		}

		if (detector.gameObject.CompareTag ("Package")) {
			movePackage (detector.gameObject.transform.parent.transform.GetChild (0).position, detector.gameObject.transform.parent.transform.GetChild (0).gameObject);
		}

		else {
			lockPosition (detector.transform.position);
		}
	}

	void movePackage(Vector3 whatMove, GameObject detected){
		//	To move package object after colision with player
		Vector3	difference = (whatMove - transform.position);

		if (difference.x >= 0.9) {
			detected.GetComponent<PackageCoreTransform>().moveRight();
		}
		if (difference.x <= -0.9) {
			detected.GetComponent<PackageCoreTransform>().moveLeft();
		}
		if (difference.z >= 0.9) {
			detected.GetComponent<PackageCoreTransform>().moveForward();
		}
		if (difference.z <= -0.9) {
			detected.GetComponent<PackageCoreTransform>().moveBack();
		}
	}

	void lockPosition (Vector3 whereToLock){
		//	Unable player of making move out of range
		Vector3	difference = (whereToLock - transform.position);
		if (difference.x >= 0.9) {
			obj.GetComponent<CoreTransform>().right=false;
			obj.transform.position = new Vector3 (Round(transform.position.x), (float)Round(transform.position.y), (float)Round(transform.position.z));
		}
		if (difference.x <= -0.9) {
			obj.GetComponent<CoreTransform>().left=false;
			obj.transform.position = new Vector3 (Round(transform.position.x), (float)Round(transform.position.y), (float)Round(transform.position.z));
		}
		if (difference.z >= 0.9) {
			obj.GetComponent<CoreTransform>().forward=false;
			obj.transform.position = new Vector3 (Round(transform.position.x), (float)Round(transform.position.y), (float)Round(transform.position.z));
		}
		if (difference.z <= -0.9) {
			obj.GetComponent<CoreTransform>().back=false;
			obj.transform.position = new Vector3 (Round(transform.position.x), (float)Round(transform.position.y), (float)Round(transform.position.z));
		}
	}

	public	int	Round( float value ) {
		//	To round floats
		int ret = 0;

		float dec = value - (int)value;

		if (value > 0){
			if (dec <= 0.5000000) { ret = (int)value; }
			else { ret = (int)value+1; }
		} else if (value < 0) {
			if (dec > -1.0  && dec < -0.5000000) { ret = (int)value-1; }
			else { ret = (int)value; }
		}

		return ret;
	}
}
