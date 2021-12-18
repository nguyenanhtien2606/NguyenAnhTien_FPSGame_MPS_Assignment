using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController: MonoBehaviour
{
    [SerializeField] List<Gun> gunLst;
    [SerializeField] List<Grenade> grenadeLst;

    public List<Gun> GunLst
    {
        get { return gunLst; }
        set { gunLst = value; }
    }

    public List<Grenade> GrenadeLst
    {
        get { return grenadeLst; }
        set { grenadeLst = value; }
    }
}

[Serializable]
public enum WeaponType
{
    Rifle,
    HandGun
}

[Serializable]
public enum WeaponFireType
{
    Once,
    Continuous
}

[Serializable]
public class Gun
{
    [SerializeField] string gunName;
    [SerializeField] Sprite gunAvatar;
    [SerializeField] float maxAmmo;
    [SerializeField] int damage;
    [SerializeField] WeaponType weaponType;
    [SerializeField] WeaponFireType weaponFireType;


    public string GunName
    {
        get { return gunName; }
        private set { gunName = value; }
    }

    public Sprite GunAvatar
    {
        get { return gunAvatar; }
        private set { gunAvatar = value; }
    }

    public float MaxAmmo
    {
        get { return maxAmmo; }
        private set { maxAmmo = value; }
    }

    public int Damage
    {
        get { return damage; }
        private set { damage = value; }
    }

    public WeaponType P_WeaponType
    {
        get { return weaponType; }
        private set { weaponType = value; }
    }

    public WeaponFireType P_WeaponFireType
    {
        get { return weaponFireType; }
        set { weaponFireType = value; }
    }
}

[Serializable]
public class Grenade
{
    [SerializeField] string grenadeName;
    [SerializeField] Sprite grenadeAvatar;
    [SerializeField] GameObject grenadeModel;
    [SerializeField] float effectRange;
    [SerializeField] int damage;
    

    public string GrenadeName
    {
        get { return grenadeName; }
        private set { grenadeName = value; }
    }

    public Sprite GrenadeAvatar
    {
        get { return grenadeAvatar; }
        private set { grenadeAvatar = value; }
    }

    public GameObject GrenadeModel
    {
        get { return grenadeModel; }
        private set { grenadeModel = value; }
    }

    public float EffectRange
    {
        get { return effectRange; }
        private set { effectRange = value; }
    }

    public int Damage
    {
        get { return damage; }
        private set { damage = value; }
    }
}
