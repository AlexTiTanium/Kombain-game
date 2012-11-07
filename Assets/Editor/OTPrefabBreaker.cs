#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(OTPB))]
public class OTPrefabBreaker : Editor
{		
    void OnEnable()
    {		
		// if we are presistent we reside on disk and therefor are a real prefab
		if (EditorUtility.IsPersistent(target)) return;
		
		// disconnect the prefab
		OTPB pb = (target as OTPB);				
		if (!pb.disconnected)
		{
			PrefabUtility.DisconnectPrefabInstance(target);	
			// save state so we know we already disconnected once
			pb.disconnected = true;		
		}
    }	

}
#endif