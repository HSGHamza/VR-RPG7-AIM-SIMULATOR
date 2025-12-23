using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using HWRWeaponSystem;

public class GameMainmenu : MonoBehaviour {


	void Start () {
		Time.timeScale = 1;
		MouseLock.MouseLocked = false;
	}
	
	public void BackToMainmenu(){
		SceneManager.LoadScene("Menu");
	}

	public void Play(){
		SceneManager.LoadScene("Battlefield");
	}

	public void Continue(){
		if(TankGame.TankGameManager.TankControl.TargetTank == null){
			SceneManager.LoadScene("Battlefield");
		}else{
			TankGame.TankGameManager.HUD.ShowMenu(false);
		}
		Time.timeScale = 1;
	}

	public void GameOver(){
		Time.timeScale = 1;
		SceneManager.LoadScene("GameOver");
	}

	public void ExitGame(){
		Application.Quit();
	}

	public void GetProject(){
		
	}
}
