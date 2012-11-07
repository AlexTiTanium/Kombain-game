using UnityEngine;
using System.Collections;

public class EnemiesBrain : MonoBehaviour {
	
	public enum Decision { none, left, right, up, down, shoot };
	
	public int chanceShoot = 10;
	public int chanceWalkLeftRight = 60;
	
	public int MaxHorizontal = 10;
	public int MaxVertical = 5;
	
	public float speed = 0.2f;
	
	private int currentHorizontal = 0;
	private int currentVertical = 0;
	
	private float nextDecision = 1.0f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(transform.GetChildCount() == 0){
			GameObject.Find("gui").SendMessage("Win");
			return;
		}
		
		if (Time.time > nextDecision) {
			generateDecision();
		}
	}
	
	private void generateDecision(){
		nextDecision = Time.time + speed;
		
		if(fallChance(chanceShoot)){
			sendDecision(Decision.shoot);
		}else{
			if(fallChance(chanceWalkLeftRight)){
				walk(Decision.left, Decision.right);
			}else{
				walk(Decision.up, Decision.down);
			}
		}
		
	}
	
	private void walk(Decision stateOne, Decision stateTwo){
		
		Decision decision = Decision.none;
		
		if(currentHorizontal < 0 && currentHorizontal <= -MaxHorizontal){
			decision = Decision.right; 
		}
		
		if(currentVertical < 0 && currentVertical <= -MaxVertical){
			decision = Decision.up; 
		}
		
		if(currentHorizontal > 0 && currentHorizontal >= MaxHorizontal){
			decision = Decision.left; 
		}
		
		if(currentVertical > 0 && currentVertical >= MaxVertical){
			decision = Decision.down; 
		}
		
		if(decision == Decision.none){
			if((Random.Range(1,100) % 2) == 0){
			 	decision = stateTwo;
			}else{
			 	decision = stateOne; 
			} 
		}
		
		sendDecision(decision);
	}
	
	private void sendDecision(Decision decision){
		
		BroadcastMessage("Decision",  decision);
		
		if(decision == Decision.left){
			currentHorizontal--;
		}
		
		if(decision == Decision.right){
			currentHorizontal++;
		}
		
		if(decision == Decision.up){
			currentVertical++;
		}
		
		if(decision == Decision.down){
			currentVertical--;
		}
	}
	
	private bool fallChance(int chance){
		
		int rand = Random.Range(1,100); 
		
		if(rand < chance){
			return true;
		}
		
		return false;	
	}
	
	
}