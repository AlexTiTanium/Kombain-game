using UnityEngine;
using System.Collections;


public class LifePanel : MainGuiScript {
	
	public Texture lifeTexture;
	public float padding = 0.2f;
	public int lifesCount = 3;
	
	void OnGUI() {

    	beginLayout(LayoutType.leftTop);
		
		for(int i = 0; i < lifesCount; i++){
			Rect rect = getBox();
			rect.x += padding * i;
			GUI.Label(rect, new GUIContent(lifeTexture));
		}
	}
	
	void SetLifes(int lifes){
		lifesCount = lifes;	
	}
}
