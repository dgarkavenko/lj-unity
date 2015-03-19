using UnityEngine;
using System.Collections;

namespace CT
{
	public class Weapon : MonoBehaviour
	{
		//public float Recoil;
		public float cooldown = 0.33f;
		public Transform Origin;
		public float RayCastDistance;
		public Trace Trace;

		private double _lastShotTime;

		private RaycastHit2D hit;
		internal void Trigger(Vector2 dir)
		{
			var currentTime = Time.time;
			if (currentTime >= _lastShotTime + cooldown)
			{

				_lastShotTime = currentTime;
				//angle += (gd.dispersion + recoil) * UnityEngine.Random.Range(-1f, 1f);
				//var dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

				hit = Physics2D.Raycast(Origin.position, dir, RayCastDistance, LayerMask.GetMask("Zombies"));

				if (hit.collider != null)
				{
					var IR = hit.collider.gameObject.GetComponent<Interactive>();
					Trace.Show(Origin.position, hit.point);

				}
				else
					Trace.Show(Origin.position, (Vector2)Origin.position + dir * RayCastDistance);

			}
		}
	}

}



