using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowOnColision : MonoBehaviour {

	public	GameObject	obj;
	private	Vector3		basicPosition;


	void Start (){
		basicPosition = transform.position;
	}

	public void SetBasic (){
		transform.position = basicPosition;
	}

	//	To detect kind of colision during arrow flight
	private void OnCollisionEnter(Collision detector) {
		if(detector.gameObject.CompareTag("Player")){
			SetBasic();
		}
		if (detector.gameObject.CompareTag ("Killable")){
			SetBasic();
			Destroy (detector.gameObject);
		}
		else {
			SetBasic();
		}
	}

}
