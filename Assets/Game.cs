using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    public CameraController Cam;
    public World World;
    public Lumberjack Lj;
    public GameHUD HUD;

    void Start()
    {
        Cam = Camera.main.GetComponent<CameraController>();
        OnLocationChanged();

        Lj = Object.Instantiate<Lumberjack>(Lj);
        Cam.Target = Lj.transform;
        HUD.Init(Lj);

    }

    void OnLocationChanged()
    {
        var location = GameObject.FindObjectOfType<lj.LocationInfo>();
        Cam.Bounds = location.Size;
        Cam.Parallax = GameObject.FindObjectsOfType<ParallaxLayer>();

        World.LightIntensityByTime = location.LightIntensity;
        World.LightColorByTime = location.LightColor;
        World.SkyColors = location.SkyColors;

        Lj.transform.position = location.Entrances[0].position;
    }   

    public void Update()
    {
        World.ManualUpdate(Time.deltaTime);     
    }

}


[System.Serializable]
public class World
{
    private float _currentTime;
    public float TimeScale = 360;
    public string Time;
    public Light Sun;

    public AnimationCurve LightIntensityByTime;
    public Gradient LightColorByTime;

    public Gradient[] SkyColors;
    public Material SkyMaterial;


    public void ManualUpdate(float deltaTime)
    {
        _currentTime += deltaTime * TimeScale;

        float days = _currentTime / (3600 * 24);
        float hours = _currentTime % (3600 * 24) / 3600;
        float minutes = (_currentTime % 3600) / 60;

        Time = string.Format("Day {0}. {1}h:{2}m", (int)days + 1, (int)hours, (int)minutes);
        
        Sun.intensity = LightIntensityByTime.Evaluate(hours);

        SkyMaterial.SetFloat("_Overbrightness", 0.5f / Sun.intensity);

        var normalTime = hours / 24f;

        Sun.color = LightColorByTime.Evaluate(normalTime);
        SkyMaterial.SetColor("_Top", SkyColors[0].Evaluate(normalTime));
        SkyMaterial.SetColor("_Mid", SkyColors[1].Evaluate(normalTime));
        SkyMaterial.SetColor("_Bot", SkyColors[2].Evaluate(normalTime));

    }
}

