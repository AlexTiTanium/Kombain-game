using UnityEngine;
using System.Collections;

public class NakedBluePants : EnemyController
{
	
	protected override int chanceShoot { get{ return 30; } }
	protected override int maxLife { get{ return 2; } set{ this.maxLife = value; } }
	protected override EnemyType currentEnemyType { get{ return EnemyType.NakedBluePants; }}
	
}

