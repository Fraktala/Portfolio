using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMoreShooting : MonoBehaviour {

	public	GameObject	Arrow;

	/*
	 * Script is used to lock an archer
	 */

	private void OnCollisionEnter(Collision detector) {
		if (detector.gameObject.CompareTag ("Package")) {
			Destroy (Arrow);
			Destroy (gameObject);
		}
	}
}
