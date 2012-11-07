using UnityEngine;
using System.Collections;

public abstract class Button : MainGuiScript {
	
	public string text = "";
	
	void InWindow() {
		
		beginLayout(LayoutType.middle);
		if(GUI.Button(getBox(), new GUIContent(text))){
			OnClick();	
		}
		
	}
	
	virtual public void OnClick(){
		
	}
}
