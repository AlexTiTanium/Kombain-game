using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour{

  public float speed = 10.0f;
  public int size = 10; 
  private OTSprite sprite;
  private int lifes	= 2;
  private bool ignoreCollisions = false;	
 
  private GameObject cannon;	

	
  private Detonator detonator; 

	// Use this for initialization
	void Start () {
		
		sprite = GetComponent<OTSprite>();
	  	detonator = GetComponent<Detonator>();
		
		sprite.onCollision = onCollision;
		cannon = GameObject.Find("cannon");
		
		GameObject.Find("gui").SendMessage("SetLifes", lifes);
	}
	
	// Update is called once per frame
	void Update () {
		
	    float translation = Input.GetAxis("Horizontal") * speed;
	
		translation *= Time.deltaTime;
		
	    if (sprite.position.x > -size && translation < 0) {
	      sprite.position += new Vector2(translation, 0);
	    }

	    if (sprite.position.x < size && translation > 0) {
	      sprite.position += new Vector2(translation, 0);
	    }
		
		if(translation > 0){
			sprite.rotation = -90;
		}else if(translation == 0){
			sprite.rotation = 0;	
		}else{
			sprite.rotation = 90;		
		}
		
		if(ignoreCollisions){
			float lerp = Mathf.PingPong(Time.time * 2.0f, 1);
			setAlpha(this.gameObject, lerp);
			setAlpha(this.cannon, lerp);
		}

	}
	
	void setAlpha(GameObject gameObject, float alpha){
		if(gameObject == null){ return; }
		Color color = gameObject.renderer.material.color;
		color.a = alpha;
     	gameObject.renderer.material.color = color;	
	}
	
	void onCollision(OTObject owner){
		string tag = owner.collisionObject.tag;
		
		if(ignoreCollisions){ return; }
		if(tag != "enemyBullet"){ return; }
		
		lifes--;
		
		OT.Destroy(owner.collisionObject);
		
		GameObject.Find("gui").SendMessage("SetLifes", lifes);
		
		if(lifes == -1){ gameOver(); }
		
		ignoreCollisions = true;
		
		Invoke("ColisionsOn", 2);
	}
	
	void ColisionsOn(){
		ignoreCollisions = false;
		
		setAlpha(this.gameObject, 1);
		setAlpha(this.cannon, 1);
	}
	
	void gameOver(){
		
		foreach(Transform child in this.transform){
        	GameObject.Destroy(child.gameObject);
		}
		
		OT.Destroy(sprite);
		
		detonator.Explode();
		
		GameObject.Find("gui").SendMessage("GameOver");
	}

}