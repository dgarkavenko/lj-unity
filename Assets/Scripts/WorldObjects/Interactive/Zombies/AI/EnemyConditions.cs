
using System;

[System.Flags]
public enum EnemyConditions
{
	see_enemy = 1,
	can_stand = 2,
	can_walk = 4,
	can_ranged_attack = 8,
	can_melee_attack = 16
}


