using UnityEngine;
using System.Collections;

// this class its sole purpose is to disconnect the prefab object it is in
// from the prefab as soon as the user drags the object from the project view 
// into the scene's hierarchy
public class OTPB : MonoBehaviour {

	/// <exclude />
	[HideInInspector]
	public bool disconnected = false;		// we only need to break once.
	
}
