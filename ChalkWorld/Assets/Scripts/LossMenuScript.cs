using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;
using UnityEngine.SceneManagement;


public class LossMenuScript : MonoBehaviour {

    public Button exitButton;
    public Button playAgainButton;
    public VRTK_Pointer pointer;

   // Use this for initialization
    void Start () {
        exitButton.onClick.AddListener(exitPressed);
        playAgainButton.onClick.AddListener(playAgainPressed);


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void exitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    void playAgainPressed()
    {
        SceneManager.LoadScene("chalk_world_level1_final", LoadSceneMode.Single);
    }
}
