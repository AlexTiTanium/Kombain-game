using UnityEngine;
using System.Collections;

public class Window : MainGuiScript {
	
	public bool showWindow = false;
	public string title = "text";
	
	public GameObject[] inWindowElements;
	
	void OnGUI() {
		
		if(!showWindow){ return; }
		
		beginLayout(LayoutType.middle);
		GUI.Window(0, getBox(), InWindow, new GUIContent(title)); 
		
	}
	
	void InWindow(int id){
		foreach(GameObject gameObject in inWindowElements){
			
			foreach(MainGuiScript mainGuiScript in gameObject.GetComponents<MainGuiScript>()){
				Rect rect = getBox();
				mainGuiScript.setLayoutSize(rect.width, rect.height);
			}
			
			gameObject.SendMessage("InWindow", SendMessageOptions.DontRequireReceiver);	
		}
	}
			
}