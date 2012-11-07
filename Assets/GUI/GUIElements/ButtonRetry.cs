using UnityEngine;
using System.Collections;

public class ButtonRetry : Button {
	
    override public void OnClick(){
		Application.LoadLevel(Application.loadedLevel);
	}
	
}