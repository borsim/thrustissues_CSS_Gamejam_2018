using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

	public static string playerName = "";
	private InputField inputField;

	void Start () {
		DontDestroyOnLoad(this);
		inputField = GameObject.Find("InputField").GetComponent(typeof(InputField)) as InputField;
		inputField.onEndEdit.AddListener(delegate {saveInput(inputField); });
	}
	
	// Saves stuff entered into the input field into "name"
    void saveInput(InputField input)
    {
    	MainMenuScript.playerName = input.text.Substring(0,Mathf.Min(15, input.text.Length));
    }
}
