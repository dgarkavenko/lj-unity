//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18331
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

[System.Serializable]
public class GunData : WeaponData
{
	public enum EMode{
		auto,
		semi
	}

    public enum EAmmo
    {
        mm9,
        mm556,
        mm762,
        shells
    }

	public int ammo_max;
	public int ammo_current;
	public float reload_time;
	public float rate;
	public EMode mode = EMode.semi;
    public EAmmo ammoType = EAmmo.mm9;
	public float dispersion;
	public int fragments;
	public int damage_min;
	public int damage_max;
	public float force;
	public float recoilPerShot;
	public float recoilReduction;
	public float reload_time_left;
    public Vector2[] gunpoints;
    public Sprite[] frames;

}


