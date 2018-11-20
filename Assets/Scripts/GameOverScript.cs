using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {

	private GameObject canvas;
	private Text scoreText;
	private Text currentScoreText;
	private Text survivalTimeText;
	private Text scoreListText;
	private Text nameListText;
	private List<Scores> highscore;

	private float score = 0f;
	private float debrisDestroyed = 0f;
	private float aliveTime = 0f;

	private const float DEBRIS_SCORE = 100f;
	private float timeScore = 5f;
	private const float TIME_SCORE_INCREASE = 0.25f;

	// Use this for initialization
	void Start () {	
		canvas = GameObject.Find("Canvas");
		currentScoreText = GameObject.Find("CurrentScoreText").GetComponent(typeof(Text)) as Text;
		survivalTimeText = GameObject.Find("SurvivalTimeText").GetComponent(typeof(Text)) as Text;
		highscore = new List<Scores>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && Time.timeScale == 0f) {
			restartGame();
		}
		score += Time.deltaTime * timeScore;
		timeScore += TIME_SCORE_INCREASE * Time.deltaTime;
		aliveTime += Time.deltaTime; 	 	
		updateCurrentScores();
	}

	public void triggerGameOver() {
		Time.timeScale = 0f;
		GameObject goui = Instantiate(globalPrefabs.getPrefab("GameOverUI"));
		goui.transform.SetParent(canvas.transform, false);
		scoreListText = GameObject.Find("ScoreList").GetComponent(typeof(Text)) as Text;
		nameListText = GameObject.Find("NameList").GetComponent(typeof(Text)) as Text;
		scoreText = GameObject.Find("ScoreText").GetComponent(typeof(Text)) as Text;
		if (debrisDestroyed != 1) scoreText.text = "You have managed to destroy "+ Mathf.Round(debrisDestroyed).ToString() + " pieces of debris! \nYou achieved a score of "+ Mathf.Round(score).ToString();
		else scoreText.text = "You have managed to destroy "+ Mathf.Round(debrisDestroyed).ToString() + " piece of debris! \nYou achieved a score of "+ Mathf.Round(score).ToString();

		HighScoreManager._instance.SaveHighScore(MainMenuScript.playerName, (int)Mathf.Round(score));
		highscore = HighScoreManager._instance.GetHighScore();

		scoreListText.text = "";
		nameListText.text = "";
		foreach(Scores _score in highscore)
        {
         	scoreListText.text = scoreListText.text + "\n" + _score.score;
         	nameListText.text = nameListText.text + "\n" + _score.name;
        }
	}
	void restartGame() {
		debrisDestroyed = 0;
		score = 0;
		aliveTime = 0;
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
	}
	public void addDebrisScore() {
		score += DEBRIS_SCORE;
		debrisDestroyed += 1;
	}
	private void updateCurrentScores() {
		survivalTimeText.text = "You have survived for: " + Mathf.Round(aliveTime).ToString() + " seconds";
		currentScoreText.text = "Score: " + Mathf.Round(score).ToString();
	}


}
