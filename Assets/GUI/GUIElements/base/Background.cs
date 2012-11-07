using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
	
	//**************************************************************************
	//
	// Public
	//
	//**************************************************************************
	
	public Material material;
	public int distance = 700;
	
	//**************************************************************************
	//
	// MonoBehaviour
	//
	//**************************************************************************
	
	void Start ()
	{
		
		if(!isConfigurationRight()){
			return;
		}
		
		backgroundGameObject = new GameObject();
		
		backgroundGameObject.name = "background";
		
		backgroundGameObject.AddComponent<MeshRenderer>();
		backgroundGameObject.AddComponent<MeshFilter>();
		
		backgroundGameObject.GetComponent<MeshRenderer>().material = material;
		
		backgroundMesh = backgroundGameObject.GetComponent<MeshFilter>().mesh;
		
		updateMesh(backgroundMesh);
		
		backgroundGameObject.transform.localPosition = Vector3.zero;
		backgroundGameObject.transform.parent = transform;
		
		writeCache();
	}
	
	//--------------------------------------------------------------------------
	
	void Update(){
		
		if(!isConfigurationRight()){
			return;
		}
		
		if(!isCacheOutdated()){
			return;
		}
		
		updateMesh(backgroundMesh);
		
	}
	
	//**************************************************************************
	//
	// Private
	//
	//**************************************************************************
	
	private GameObject backgroundGameObject;
	private Mesh backgroundMesh;
	
	//**************************************************************************
	// Cache  
	
	private int prevDistance;	
	private int prevScreenWeight;
	private int prevScreenHeight;
	
	//**************************************************************************
	// Calculate mesh params
	
	private void updateMesh(Mesh mesh)
	{
		mesh.vertices = getVertecsCoords();
		mesh.triangles = new int[]{ 0,3,1 , 3,2,1 };
		
		mesh.uv = getUVCoords(); 
	}
	
	//--------------------------------------------------------------------------
	
	private Vector3[] getVertecsCoords()
	{
		
		return new Vector3[] {
			camera.ScreenToWorldPoint(new Vector3(0,Screen.height, distance)),
			camera.ScreenToWorldPoint(new Vector3(0,0, distance)),
			camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, distance)),
			camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distance))	
		};
		
	}
	
	//--------------------------------------------------------------------------
	
	private Vector2[] getUVCoords()
	{
		
		return new Vector2[]{
			new Vector2(0,1), 
			new Vector2(0,0),
			new Vector2(1,0),
			new Vector2(1,1)
		};	
		
	}
	
	//**************************************************************************
	// Some cache methods
	
	private void writeCache()
	{
		prevDistance = distance;
		prevScreenWeight = Screen.width;
		prevScreenHeight = Screen.height;
	} 
	
	//--------------------------------------------------------------------------
	
	private bool isCacheOutdated()
	{		
		if(prevDistance != distance 
			|| prevScreenWeight != Screen.width
			|| prevScreenHeight != Screen.height){
			
			return true;
		}
		
		return false;
	}
	
	//**************************************************************************
	// Some tests
	
	private bool isConfigurationRight()
	{
		
		bool result = true;
		
		if(camera == null){
			Debug.LogError("This screept must be bind to main camera");
			result = false;
		}
		
		if(distance > camera.farClipPlane){
			Debug.LogWarning("Distance to large");
			result = false;
		}
		
		if(distance < camera.nearClipPlane){
			Debug.LogWarning("Distance to small");
			result = false;
		}
		
		return result;
		
	}
	
	
}