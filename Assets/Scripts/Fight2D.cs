using UnityEngine;
using System.Collections;

public class Fight2D : MonoBehaviour 
{
	public static void Action(Vector2 point, float radius, int layerMask, float damage, bool allTargets)
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(point, radius, 1 << layerMask);

		if(!allTargets)
		{
			GameObject obj = NearTarget(point, colliders);
			if(obj != null && obj.GetComponent<EnemyScript>())
			{
				obj.GetComponent<EnemyScript>().DamageEnemy(damage);
			}
			return;
		}

		foreach(Collider2D hit in colliders) 
		{
			if(hit.GetComponent<EnemyScript>())
			{
				hit.GetComponent<EnemyScript>().DamageEnemy(damage);
			}
		}
	}
	
	static GameObject NearTarget(Vector3 position, Collider2D[] array) 
	{
		Collider2D current = null;
		float dist = Mathf.Infinity;

		foreach(Collider2D coll in array)
		{
			float curDist = Vector3.Distance(position, coll.transform.position);

			if(curDist < dist)
			{
				current = coll;
				dist = curDist;
			}
		}

		return (current != null) ? current.gameObject : null;
	}
}