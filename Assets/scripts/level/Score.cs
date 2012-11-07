using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	
	public int WiteEnemyScore = 25;
	
	private int playerScore = 0;
	
	void KillEnemy(EnemyController.EnemyType type){
		
		switch(type){
			case EnemyController.EnemyType.NakedWhiteShorts:
			    playerScore += WiteEnemyScore; 
				GameObject.Find("gui").SendMessage("SetScore", playerScore);
			break;
		}
		
	}
	

	
	
}
