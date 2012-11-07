using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	public float speed = 10.0f;
	public bool upDirection = true;
	
	private OTSprite sprite;
	
	// Use this for initialization
	void Start () {
		sprite = GetComponent<OTSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 vector = sprite.transform.up;	
		
		if(!upDirection){
			vector = -sprite.transform.up;			
		}
		
		sprite.position += (Vector2)vector * speed * Time.deltaTime;
		
		if(sprite.outOfView){
			
			if(this.tag == "playerBullet"){
        		GameObject.Find("level").BroadcastMessage("PlayerBulletOutOfView", SendMessageOptions.RequireReceiver);
			}
			
			OT.DestroyObject(sprite);			
		}
	}
	
}
