using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public class VFX : MonoBehaviour {


    public ParticleSystem Blood;
	public ParticleSystem[] Gunfire;
	public Light Light;
	public AnimationCurve LightIntencityCurve;

	private static VFX _instance;
	public static VFX Instance
	{
		get
		{
			return _instance;
		}
	}

	private ParticleSystem.Particle _gunParticle = new ParticleSystem.Particle();

	private Stack<LiveGunfire> _stack = new Stack<LiveGunfire>();
	private List<LiveGunfire> _live = new List<LiveGunfire>();

	public struct LiveGunfire
	{
		public ParticleSystem Sys;
		public PositionDelegate Position;
		public float StartTime;
	}

	public delegate Vector2 PositionDelegate();

	public void Awake()
	{
		if (_instance == null)
			_instance = this;
		else
		{
			Debug.Log("There are another instance of Effects");
			Object.Destroy(this);
		}

		for (int i = 0; i < 3; i++)
		{
			_stack.Push(new LiveGunfire()
			{
				Sys = Object.Instantiate(Gunfire[0])
			});
		}
	}

    public void GunfireAt(Vector3 position, float rotation, Vector3 dir, Vector3 hit, PositionDelegate positionDelegate)
    {
	    var effect = _stack.Pop();
	    
	    effect.Position = positionDelegate;

	    effect.Sys.transform.position = positionDelegate();
	    effect.Sys.startRotation = rotation;
		effect.Sys.Emit(1);
	    effect.StartTime = Time.time;

		_live.Add(effect);

    }

	public void Update()
	{
		for (int index = _live.Count - 1; index > -1 ; index--)
		{
			var liveGunfire = _live[index];
			if (liveGunfire.Sys.IsAlive())
			{

				var pos = liveGunfire.Position();

				liveGunfire.Sys.transform.position = pos;

				var lightZ = Light.transform.position.z;

				Light.transform.position = new Vector3(pos.x, pos.y, lightZ);
				Light.intensity = LightIntencityCurve.Evaluate(Time.time - liveGunfire.StartTime);
			}
			else
			{
				Light.intensity = 0;
				_live.RemoveAt(index);
				_stack.Push(liveGunfire);
			}
		}
	}
}
