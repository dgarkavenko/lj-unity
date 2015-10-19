using UnityEngine;
using System.Collections;

public class AmmoCounter : MonoBehaviour {

    public UnityEngine.UI.Text Current;
    public UnityEngine.UI.Text All;
    public UnityEngine.UI.Text Slash;
    public float AnimationSpeed = 0.33f;

    private bool _reloading = false;
    private int index = 0;
    private string[] loading = new string[]{"/", "-", @"\", "l"};

    private float nextIndexTime = 0;

    
    public void Reload(bool on)
    {

        _reloading = on;
        index = 0;

        Slash.text = loading[index];
        
    }

    public void Set(params int[] count)
    {   
        var current = count[0].ToString();
        if(current.Length < 2) current = "0" + current;
        Current.text = current;

        if (count.Length > 1)
        {
            var all = count[1].ToString();
            while (all.Length < 3)            
                all = "0" + all;
            
            All.text = all;
        }
    }


    public void Update()
    {
        if (!_reloading) return;

        if (nextIndexTime < Time.time)
        {
            nextIndexTime = Time.time + AnimationSpeed;
            index = (index + 1) % loading.Length;
            Slash.text = loading[index];

        }
        
    }
    
}
