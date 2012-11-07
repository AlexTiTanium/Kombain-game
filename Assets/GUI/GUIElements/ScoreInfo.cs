using UnityEngine;
using System.Collections;

public class ScoreInfo : MainGuiScript {
	
	public int score = 0;
	
	void OnGUI() {
		
		beginLayout(LayoutType.topMiddle);
		GUI.Label(getBox(), new GUIContent(score.ToString()), guiStyle);
		
	}
	
	void SetScore(int score){
		this.score = score;		
	}
	
}