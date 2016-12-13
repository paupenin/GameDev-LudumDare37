using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

	[SerializeField]
	Text score;

	void Start(){
		int lastGameScore = PlayerPrefs.GetInt("Player Score");

		if (score != null && lastGameScore > 0) {
			score.text = string.Format ("SCORE\n{0}", lastGameScore);
		}
	}

	public void menu(){
		Cursor.visible = true;
		Screen.lockCursor = false;

		UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
	}

	public void retry(){
		Cursor.visible = false;
		Screen.lockCursor = true;

		UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
	}

	public void credits(){
		Cursor.visible = true;
		Screen.lockCursor = false;

		UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
	}

	public void exit(){
		
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif

		Application.Quit();
	}

}
