using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] MovementController movementController;
    [SerializeField] AnimationController animationController;
    [SerializeField] FireController fireController;


    public MovementController P_MovementController
    {
        get { return movementController; }
        set { movementController = value; }
    }

    public AnimationController P_AnimationController
    {
        get { return animationController; }
        set { animationController = value; }
    }

    public FireController P_FireController
    {
        get { return fireController; }
        set { fireController = value; }
    }
}
