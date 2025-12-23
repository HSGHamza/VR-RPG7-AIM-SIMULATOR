using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using HWRWeaponSystem;

public class GameManager : MonoBehaviour {

	public TankController TankControl;
	public TankHUD HUD;
	void Start () {
		Time.timeScale = 1;
		Application.targetFrameRate = 120;
		TankGame.TankGameManager = this;
		TankControl = (TankController)GameObject.FindObjectOfType(typeof(TankController));
		HUD = (TankHUD)GameObject.FindObjectOfType(typeof(TankHUD));
		NewGame();
	}
	public void AddPlayerScore(){
		
	}

	public void NewGame(){
		TankGame.PlayerScore = 0;
	}

	public void GameOver(){
		SceneManager.LoadScene("GameOver");
	}

	public void Pause(){
		MouseLock.MouseLocked = false;
		Time.timeScale = 0;
		if(HUD)
			HUD.ShowMenu(true);
	}

	void Update () {
		if(TankControl!=null){
			if(TankControl.TargetTank == null){ // if TargetTank is null. mean the tank are destroyed or not exist.
				HUD.ShowMenu(true); // show main menu.
			}
		}
	}
}

public static class TankGame
{
	public static int PlayerScore;
	public static GameManager TankGameManager;

}

