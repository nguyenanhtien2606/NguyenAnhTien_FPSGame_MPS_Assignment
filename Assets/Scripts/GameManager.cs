using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GLOBAL { get; private set; }

    [SerializeField] WeaponController weaponController;

    public WeaponController P_WeaponController
    {
        get { return weaponController; }
        set { weaponController = value; }
    }

    private void Awake()
    {
        GLOBAL = this;
    }
}
