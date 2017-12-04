using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_UI : MonoBehaviour {
	
	//							Private
	private		bool			is_pause	=	false;

	private		Button[]		buttons_Escape;
	private		Button[]		buttons_Win;
	private		Button[]		buttons_Loose;

	private		GameObject		Win;
	private		GameObject		Loose;
	private		GameObject		Esc;

	// Use this for initialization
	void Start () {

		/*
		 * 					Escape
		 */

		Esc = transform.GetChild (0).gameObject;
		buttons_Escape = Esc.GetComponentsInChildren<Button> ();
		buttons_Escape [0].onClick.AddListener (Restart);
		buttons_Escape [1].onClick.AddListener (BackToMenu);
		buttons_Escape [2].onClick.AddListener (Continue);
		buttons_Escape [3].onClick.AddListener (ExitGame);

		/*
		* 					Win
		*/

		Win = transform.GetChild(1).GetChild(0).gameObject;
		buttons_Win = Win.GetComponentsInChildren<Button> ();
		buttons_Win [0].onClick.AddListener (Restart);
		buttons_Win [1].onClick.AddListener (BackToMenu);
		buttons_Win [2].onClick.AddListener (NextLevel);

		/*
		* 					Loose
		*/

		Loose = transform.GetChild(2).GetChild(0).gameObject;
		buttons_Loose = Loose.GetComponentsInChildren<Button> ();
		buttons_Loose [0].onClick.AddListener (Restart);
		buttons_Loose [1].onClick.AddListener (BackToMenu);
		
		
	}

	/*
	* 						Buttons action
	*/
	void BackToMenu (){
		SceneManager.LoadScene ("FirstStep");
	}

	void Restart (){
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
	void Continue (){
		is_pause = false;
		Esc.SetActive(false);
		Time.timeScale = 1;
	}
	void NextLevel (){
		string scene_name	= SceneManager.GetActiveScene ().name;
		string scene_number = scene_name.Substring (1, scene_name.Length - 1);
		int scene_ID 		= int.Parse (scene_number);
		int current_Save	= PlayerPrefs.GetInt ("level");
		if (current_Save < scene_ID + 1) {
			PlayerPrefs.SetInt ("level", (scene_ID + 1));
		}
		scene_ID++;
		SceneManager.LoadScene ("L" + scene_ID );
	}

	void ExitGame (){
		Application.Quit();
	}

	/*
	* 						Listening to start action at escape press
	*/
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			is_pause = !is_pause;
			if (is_pause) {
				Esc.SetActive(true);
				Time.timeScale = 0;
			} else  {
				Esc.SetActive(false);
				Time.timeScale = 1;
			}
		}
	}

	/*
	* 						Using out of this script- from player and arrow one
	*/
	public void WinTheGame(){
		Time.timeScale = 0;
		transform.GetChild(1).gameObject.SetActive (true);
	}
	public void LooseTheGame(){
		Time.timeScale = 0;
		transform.GetChild(2).gameObject.SetActive (true);
	}
}
