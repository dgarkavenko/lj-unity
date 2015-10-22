using UnityEngine;
using System.Collections;

public class TestEmitter : MonoBehaviour {

    ParticleSystem Sys;



    // Use this for initialization
	void Start () {

        Sys = GetComponent<ParticleSystem>();
        for (int i = 0; i < _randomFloat.Length; i++)        
            _randomFloat[i] = Random.Range(0f, 1f);
        
	}

    public float InputDirection;
    public float Spread = 0.5f;
    public float Count;
    public Vector2 Size;
    public Vector2 Lifetime;
    public Vector2 Force;

    private float[] _randomFloat = new float[128];
    private int _randomIndex;

    private ParticleSystem.Particle _particle;

  

    Vector2 GetRandomVelocity(float inputDirection)
    {
        var dirPolarWDispersion = inputDirection + UnityEngine.Random.Range(-Spread, Spread);
        return new Vector2(Mathf.Cos(dirPolarWDispersion), Mathf.Sin(dirPolarWDispersion)) * Random.Range(Force.x, Force.y);
    }

    Vector2 GetFastRandomVelocity(float inputDirection)
    {
        var dirPolarWDispersion = inputDirection + FastRandom * Spread;
        return new Vector2(Mathf.Cos(dirPolarWDispersion), Mathf.Sin(dirPolarWDispersion)) * (Force.x + Force.y * FastRandom);
    }


    public float FastRandom
    {
        get{
            _randomIndex = (_randomIndex + 5)%128;
            return _randomFloat[_randomIndex];
        }
      
    }
   

	// Update is called once per frame
	void Update () {
        
       
        if (Input.GetMouseButtonDown(1))
        {            
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos = new Vector3(pos.x, pos.y, 0);
            //p.transform.position = new Vector3(pos.x, pos.y, 0);
            _particle.position = pos;
            _particle.color = Color.red;

            for (int i = 0; i < Count; i++)
            {
                _particle.velocity = GetRandomVelocity(InputDirection);
                _particle.lifetime = _particle.startLifetime = Random.Range(Lifetime.x, Lifetime.y);
                _particle.size = Random.Range(Size.x, Size.y);
                Sys.Emit(_particle);
            }

        }
        else if (Input.GetMouseButtonDown(0))
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos = new Vector3(pos.x, pos.y, 0);
            //p.transform.position = new Vector3(pos.x, pos.y, 0);
            _particle.position = pos;
            _particle.color = Color.red;

            for (int i = 0; i < Count; i++)
            {
                _particle.velocity = GetFastRandomVelocity(InputDirection);
                _particle.lifetime = _particle.startLifetime = Lifetime.x + Lifetime.y * FastRandom;
                _particle.size = Size.x + Size.y * FastRandom;
                Sys.Emit(_particle);
            }
        }

	}
}
