using UnityEngine;
using System.Collections;

namespace CT
{
	public class Weapon : MonoBehaviour
	{
        public int ID = 0;
	


		public float RayCastDistance;


		protected RaycastHit2D hit;
		public virtual bool Trigger(Vector2 dir)
		{
            return false;
		}
	}

}



