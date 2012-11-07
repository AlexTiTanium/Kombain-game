using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {
	
	public float fireRate = 0.5F;
	public GameObject bulletPrototype;
	public GameObject bangGameObject;
	
	public bool mayShoot = true;
	
	private OTSprite sprite;
	private OTAnimatingSprite gunAnimation;
	private bool animationIsPlaying = false;
	private ParticleSystem bang; 
	
	// Use this for initialization
	void Start () {
		sprite = GetComponent<OTSprite>();
		gunAnimation = GetComponent<OTAnimatingSprite>();
		bang = bangGameObject.GetComponent<ParticleSystem>();
		
		gunAnimation.onAnimationFinish = shoot;
	}
	
	// Update is called once per frame
	void Update () {
		
		sprite.rotation = 0;
		
		if(animationIsPlaying){ return; }
		
		if (Input.GetButton("Fire1") && mayShoot) {
		    gunAnimation.PlayOnce("shoot");
		    
			OTSprite nBullet = (Instantiate(bulletPrototype.gameObject) as GameObject).GetComponent<OTSprite>();
		    nBullet.position = sprite.position + ((Vector2)sprite.transform.up * (sprite.size.y / 2));
			
			bang.Play();
			mayShoot = false;
		}
	}
	
	public void PlayerBulletOutOfView(){
		mayShoot = true;	
	}
	
	public void BulletInTarget(){
		mayShoot = true;	
	}
	
	void shoot(OTObject owner){
		if(owner != gunAnimation){ return; }

		gunAnimation.PlayLoop("idle");
	}
}