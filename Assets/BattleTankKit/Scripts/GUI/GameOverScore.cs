using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverScore : MonoBehaviour {

	public Text ScoreText;
	void Start () {
	
	}
	void Update () {
		if(ScoreText!=null){
			ScoreText.text = TankGame.PlayerScore.ToString();
		}
	}
}
