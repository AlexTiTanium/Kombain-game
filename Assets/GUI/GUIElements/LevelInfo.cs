using UnityEngine;
using System.Collections;

public class LevelInfo : MainGuiScript {

	public int level = 1;
	
	void OnGUI() {
		
		beginLayout(LayoutType.rightTop);
		GUI.Label(getBox(), new GUIContent("Level " + level.ToString()), guiStyle);
		
	}
	
	void SetLevel(int level){
		this.level = level;		
	}
	
}
