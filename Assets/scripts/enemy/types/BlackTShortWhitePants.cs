using UnityEngine;
using System.Collections;

public class BlackTShortWhitePants : EnemyController
{
	protected override int chanceShoot { get{ return 60; } }
	protected override int maxLife { get{ return 4; } }
	protected override EnemyType currentEnemyType { get{ return EnemyType.BlackTShortWhitePants; }}
	
}

