using UnityEngine;
using System.Collections;

public class NakedWhiteShorts : EnemyController
{
	protected override int chanceShoot { get{ return 20; } }
	protected override int maxLife { get{ return maxLife; } set{ maxLife = value; } }
	protected override EnemyType currentEnemyType { get{ return EnemyType.NakedWhiteShorts; }}
	
	NakedWhiteShorts(){
		maxLife = 1;	
	}	

}

