using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
	public enum EnemyType {
		NakedWhiteShorts,  
		NakedBluePants,  
		WhiteTSortBlackPants,  
		BrownTSortBlackPants,
		BlackTShortWhitePants
	};
	
	public GameObject bulletPrototype;
	
	private float step = 1.2f;
	private float stepGoAway = 3.2f;
	private float bulletXposition = 0.0f;
	
    protected virtual int chanceShoot { get; set; }
	protected virtual int maxLife { get; set; }
	protected virtual EnemyType currentEnemyType { get; set; }
	
	private OTSprite sprite;
	private OTAnimatingSprite enemyAnimation;
	
	private enum AnimationState
	{
		idle,
		walk,
		hit,
		shootLeft,
		shootRight,
		shoot,
		walkLeft,
		walkRight,
		goAway,
	};
	
	private AnimationState currentAnimation = AnimationState.idle;
	private Vector2 direction = new Vector2 (0, 0);
	private Vector2 goAwayDirection = new Vector2 (0, 0);

	private Vector2 UP    = Vector2.up;
	private Vector2 LEFT  = -Vector2.right;
	private Vector2 RIGHT = Vector2.right;
	private Vector2 DOWN  = -Vector2.up;
	
	
	// Use this for initialization
	
	void Start ()
	{
		sprite = GetComponent<OTSprite> ();
		enemyAnimation = GetComponent<OTAnimatingSprite> ();

		enemyAnimation.onAnimationFinish = animationFinish;
		
		sprite.onCollision = onCollision;
	}

	// Update is called once per frame
	void Update ()
	{
		
		switch (currentAnimation) {
			case AnimationState.goAway:
				sprite.position += goAwayDirection * stepGoAway * Time.deltaTime;
				break;
			case AnimationState.walk:
				sprite.position += direction * step * Time.deltaTime;
				break;
		}
		
		if (sprite.outOfView) {
			OT.DestroyObject (sprite);			
		}
		
	}
	
	void Decision (EnemiesBrain.Decision decision)
	{
		
		if (currentAnimation == AnimationState.goAway
			|| currentAnimation == AnimationState.hit
			) {
			return;
		}
		
		switch (decision) {
		case EnemiesBrain.Decision.right:
			direction = RIGHT;
			doWalk ();
			break;
			
		case EnemiesBrain.Decision.left:
			direction = LEFT;
			doWalk ();
			break;
			
		case EnemiesBrain.Decision.up:
			direction = UP;
			doWalk ();
			break;
			
		case EnemiesBrain.Decision.down:
			direction = DOWN;
			doWalk ();
			break;
		case EnemiesBrain.Decision.shoot:
			if (fallChance (chanceShoot)) {
				doShoot ();
			} else {
				setAnimation (AnimationState.idle);	
			}		
			break;
			
		}
		
	}
	
	void doShoot ()
	{
		
		if ((Random.Range (1, 100) % 2) == 0) {
			setAnimation (AnimationState.shootLeft);
			bulletXposition = sprite.size.x;
			
		} else {
			setAnimation (AnimationState.shootRight);
			bulletXposition = -sprite.size.x;
		}
		
		Invoke ("LaunchBullet", 0.2f);
	}
	
	void LaunchBullet ()
	{
		OTSprite nBullet = (Instantiate (bulletPrototype.gameObject) as GameObject).GetComponent<OTSprite> ();
		nBullet.position = sprite.position - new Vector2 (bulletXposition / 2, 0);	
	}
	
	void doDie ()
	{
		GameObject.Find ("level").SendMessage ("KillEnemy", currentEnemyType);
	}
	
	void doWalk ()
	{

		if (direction.x == -1 || direction.y == -1) {
			setAnimation (AnimationState.walkLeft);
		} else {
			setAnimation (AnimationState.walkRight);
		}
	}
	
	void onCollision (OTObject owner)
	{
		string tag = owner.collisionObject.tag;
		
		if (currentAnimation == AnimationState.goAway 
			|| currentAnimation == AnimationState.hit) {
			return;
		}
		
		if (tag != "playerBullet") { return; }
				
		GameObject.Find ("level").BroadcastMessage ("BulletInTarget", SendMessageOptions.RequireReceiver);
		OT.Destroy (owner.collisionObject);
		setAnimation (AnimationState.hit);
				
	}
	
	void animationFinish (OTObject owner)
	{

		switch (currentAnimation) {
			
			case AnimationState.hit:
				
				maxLife = maxLife - 1;
				
				if(maxLife!=0){
					return;
				}	
			
				doDie();
				calculateGoAwayDirection();
				setAnimation (AnimationState.goAway);
				break;
			default:
				setAnimation (AnimationState.idle);
				break;
		}

	}
	
	void calculateGoAwayDirection ()
	{
		
		Vector3 screenPositionEnemy = OT.view.camera.WorldToScreenPoint (transform.position);
		
		float left = screenPositionEnemy.x;
		float up = screenPositionEnemy.y;
		float right = Screen.width - screenPositionEnemy.x;
		
		Debug.Log(up);
		/*	
		float[] arr = new float[]{ left, up, right };
		
		float min = Mathf.Min(arr);
		*/
		
		if(left < right ){
			goAwayDirection = LEFT;		
		}else{
			goAwayDirection = RIGHT;	
		}
		
		/*else{
			goAwayDirection = UP;
		}	*/
		
	}
	
	void setAnimation (AnimationState state)
	{
				
		if (currentAnimation == AnimationState.hit && state != AnimationState.goAway) {
			return;
		}
		
		switch (state) {
			
			case AnimationState.idle:
				enemyAnimation.PlayLoop ("idle");
				currentAnimation = AnimationState.idle;
				break;
			case AnimationState.hit:
				enemyAnimation.PlayOnce ("hit");
				currentAnimation = AnimationState.hit;
				break;
			case AnimationState.shootLeft:
				currentAnimation = AnimationState.shoot;
				enemyAnimation.PlayOnce ("shootLeft");
				break;
			case AnimationState.shootRight:
				currentAnimation = AnimationState.shoot;
				enemyAnimation.PlayOnce ("shootRight");
				break;
			case AnimationState.walkLeft:
				enemyAnimation.PlayOnceBackward ("walk");
				currentAnimation = AnimationState.walk;
				break;
			case AnimationState.walkRight:
				enemyAnimation.PlayOnce ("walk");
				currentAnimation = AnimationState.walk;
				break;
			case AnimationState.goAway:
				enemyAnimation.PlayLoop ("escape");
				
				if(goAwayDirection == RIGHT || goAwayDirection == UP){
					enemyAnimation.PlayLoop ("escape");
				}else{
					enemyAnimation.PlayLoopBackward ("escape");
				}
				
				currentAnimation = AnimationState.goAway;
				break;
		}
		
		
	}
	
	private bool fallChance (int chance)
	{
		
		int rand = Random.Range (1, 100); 
		
		if (rand < chance) {
			return true;
		}
		
		return false;	
	}
}