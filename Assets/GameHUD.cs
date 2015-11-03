using UnityEngine;
using System.Collections;

public class GameHUD : MonoBehaviour {

    private GunData _currentGun;
    private Lumberjack _lj;

    public AmmoCounter AmmoCounter;
    public HealthBar HealthBar;

    void Start()
    {
        AmmoCounter = GetComponentInChildren<AmmoCounter>();
        HealthBar = GetComponentInChildren<HealthBar>();
    }

    public void Init(Lumberjack lj)
    {
        _lj = lj;       

        lj.OnWeaponSwithced += (w) => {
            if ((w.RelatedTypes & DeadlyThings.ANY_GUN) != 0)
            {
                _currentGun = w.GetData<GunData>();
                AmmoCounter.Set(_currentGun.ammo_current, _lj.ReloadAndCheck(_currentGun.ammoType, 999, false));

            }
            else
            {
                _currentGun = null;
            }
        };

        var gun = lj.AllEquipContainers[0] as Gun;        

        gun.OnShot += () =>
        {            
            AmmoCounter.Set(_currentGun.ammo_current);
        };

        gun.OnReload += (x) =>
        {
            AmmoCounter.Set(_currentGun.ammo_current, _lj.ReloadAndCheck(_currentGun.ammoType, 999, false));
            AmmoCounter.Reload(x);
        };
    }

    public void Update() {
        HealthBar.SetHP(_lj.Hp.Normal);
    }


}
