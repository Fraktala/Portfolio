using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FirstStep_UI : MonoBehaviour {

	//							Private
	public		Color			button_active;
	public		Color			button_inactive;

	//							Private
	private		Button[]		buttons_Menu;
	private		Button[]		buttons_Levels;
	private		GameObject		MainMenu;
	private		GameObject		Levels;

	void Start () {
		/*
		 * 					Main Menu
		 */
		MainMenu = transform.GetChild(0).gameObject;
		buttons_Menu = MainMenu.GetComponentsInChildren<Button>();
		buttons_Menu[0].onClick.AddListener( OnContinueClick );
		buttons_Menu[1].onClick.AddListener( OnLevelsClick );
		buttons_Menu[2].onClick.AddListener( OnExitClick );
		/*
		 * 					Levels
		 */
		Levels = transform.GetChild(1).gameObject;
		buttons_Levels = Levels.GetComponentsInChildren<Button>();

		// 					Levels buttons
		buttons_Levels[0].onClick.AddListener( delegate() { SelectLevel(1); } );
		buttons_Levels[1].onClick.AddListener( delegate() { SelectLevel(2); } );
		buttons_Levels[2].onClick.AddListener( delegate() { SelectLevel(3); } );
		buttons_Levels[3].onClick.AddListener( delegate() { SelectLevel(4); } );
		buttons_Levels[4].onClick.AddListener( delegate() { SelectLevel(5); } );
		buttons_Levels[5].onClick.AddListener( delegate() { SelectLevel(6); } );

		//					Back button
		buttons_Levels[6].onClick.AddListener( OnBackToMenuClick );
	}

	/*
	* 						Buttons action
	*/
	public void OnContinueClick() {
		if (PlayerPrefs.HasKey("level")){
			int WitchLevel = PlayerPrefs.GetInt ("level");
			SceneManager.LoadScene( "L"+WitchLevel.ToString() );
		}
		else{SceneManager.LoadScene( "L1");}
	}

	public void OnLevelsClick() {
		MainMenu.SetActive (false);
		Levels.SetActive (true);
		if (PlayerPrefs.HasKey("level")){
			int WitchLevel = PlayerPrefs.GetInt ("level");
			SetButtonColors( WitchLevel-1 );
		}
		else{SetButtonColors( 0 );}
	}

	public void OnExitClick() {
		Application.Quit ();
	}

	public void OnBackToMenuClick() {
		MainMenu.SetActive (true);
		Levels.SetActive (false);
	}

	public void SelectLevel(int i){
		SceneManager.LoadScene ("L" + i.ToString() );
	}

	/*
	* 						Switching colours depend on registered one
	*/
	public void SetButtonColors( int current_level ) {
		for ( int i = 0; i < buttons_Levels.Length-1; i++ ) {
			ColorBlock colors = buttons_Levels[i].colors;

			if (i <= current_level) {
				colors.normalColor = button_active;
				colors.disabledColor = button_inactive;
				buttons_Levels[i].enabled = true;
			} else {
				colors.normalColor = button_inactive;
				colors.disabledColor = button_inactive;
				buttons_Levels[i].enabled = false;
			}

			buttons_Levels[i].colors = colors;
		}
	}

}
