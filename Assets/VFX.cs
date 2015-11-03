using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public class VFX : MonoBehaviour {

    [System.Serializable]
    public class LJEffect {        

        public ParticleSystem Burst;

        [MinMaxVec2(0,30)]
        public Vector2 Count;
        [MinMaxVec2(0, 10)]
        public Vector2 Lifetime;
        [MinMaxVec2(0, 10)]
        public Vector2 Size;

        [MinMaxVec2(-Mathf.PI, Mathf.PI)]
        public Vector2 Spread;

        [MinMaxVec2(0, 15)]
        public Vector2 Speed;

        public ParticleSystem Follow;
        public Light Light;
        public AnimationCurve LightIntensity;

        private PositionDelegate _getTargetPosition;
        public float StartTime;

        public void Update() {

            if (_getTargetPosition == null) return;
            var pos = _getTargetPosition();
            Follow.transform.position = pos;

            if (Light == null) return;
            var lightZ = Light.transform.position.z;
            Light.transform.position = new Vector3(pos.x, pos.y, lightZ);
            Light.intensity = LightIntensity.Evaluate(Time.time - StartTime);
        }

        public bool IsAlive() {
            var f = Follow != null && Follow.IsAlive();
            var l = Light != null && (Time.time - StartTime) < LightIntensity.keys[LightIntensity.keys.Length - 1].time;
            return f || l;
        }


        public void Off() {
        }

        public void Play(Vector3 position, float rotation, PositionDelegate positionDelegate = null, float count = -1)
        {

            if (Light != null || Follow != null)
            {
                VFX.Instance._live.Add(this);
                StartTime = Time.time;
            }

            _getTargetPosition = positionDelegate;

            if (Follow != null)
            {
                Follow.transform.position = position;
                Follow.startRotation = rotation;
                Follow.Play(true);
            }

            if(Burst != null)
            {
                ParticleSystem.Particle _particle = new ParticleSystem.Particle();
                _particle.position = position;
                _particle.color = Color.white;

                var ln = count > 0 ? count : Count.RandomInt();

                var m = (rotation < Mathf.PI / 2 && rotation > -Mathf.PI / 2) ? -1 : 1;
                for (int i = 0; i < (ln); i++)
                {
                    _particle.velocity = GetRandomVelocity(-rotation, m);
                    _particle.lifetime = _particle.startLifetime = Lifetime.RandomFloat();
                    _particle.size = Size.RandomFloat();
                    Burst.Emit(_particle);
                }
            }

           
        }

        Vector2 GetRandomVelocity(float inputDirection, int m = 1)
        {
            var dirPolarWDispersion = inputDirection + Spread.RandomFloat() * m;
            return new Vector2(Mathf.Cos(dirPolarWDispersion), Mathf.Sin(dirPolarWDispersion)) * Speed.RandomFloat();
        }
    }

    public LJEffect[] Effects;
    public Trace Trace;

	private static VFX _instance;
	public static VFX Instance
	{
		get
		{
			return _instance;
		}
	}

	private ParticleSystem.Particle _gunParticle = new ParticleSystem.Particle();

	private List<LJEffect> _live = new List<LJEffect>();
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

	
	} 

	public void Update()
	{
		for (int index = _live.Count - 1; index > -1 ; index--)
		{
			var liveEffect = _live[index];
			if (liveEffect.IsAlive())
			{
                liveEffect.Update();                
			}
			else
			{
                liveEffect.Off();
				_live.RemoveAt(index);
			}
		}
	}
}

public static class Vector2Extension
{
    public static float RandomFloat(this Vector2 v)
    {
        return UnityEngine.Random.Range(v.x, v.y);
    }
    public static int RandomInt(this Vector2 v)
    {
        return (int)UnityEngine.Random.Range(v.x, v.y);
    }
}
