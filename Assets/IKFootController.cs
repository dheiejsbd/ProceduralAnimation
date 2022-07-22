using System;
using System.Collections.Generic;
using UnityEngine;

public class IKFootController:MonoBehaviour
{
    [SerializeField] IKFootSolver LIKFoot;
    [SerializeField] IKFootSolver RIKFoot;
    [SerializeField] IKBodySolver IKBody;
    bool firstL;
    bool moving;
    public void Start()
    {
        LIKFoot.Initialize();
        RIKFoot.Initialize();
    }

    public void Update()
    {
        LIKFoot.OnUpdate();
        RIKFoot.OnUpdate();
        TryMoveFoot();
    }

    void TryMoveFoot()
    {
        if (RIKFoot.isMoving || LIKFoot.isMoving) return;

        if(moving)
        {
            moving = false;
            IKBody.Set();
        }

        if (!RIKFoot.IsFoward() && RIKFoot.F)
        {
            Debug.Log("L");
            RIKFoot.F = false;
            LIKFoot.Move();
            moving = true;
            return;
        }
        if (!LIKFoot.IsFoward() && LIKFoot.F)
        {
            Debug.Log("R");
            LIKFoot.F = false;
            RIKFoot.Move();
            moving = true;
            return;
        }
    }
}
