using System;
using System.Collections.Generic;
using UnityEngine;

public class IKFootController:MonoBehaviour
{
    [SerializeField] IKFootSolver LIKFoot;
    [SerializeField] IKFootSolver RIKFoot;
    [SerializeField] IKBodySolver IKBody;
    bool firstL;
    bool footMoving;
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

        if(footMoving)
        {
            //발 이동이 끝난후 몸을 이동시키기 위함
            footMoving = false;
            IKBody.Set();
        }

        if (firstL) MoveLeft(); else MoveRight();
        if (!firstL) MoveRight(); else MoveLeft();

    }

    void MoveLeft()
    {
        if (!RIKFoot.IsFoward() && RIKFoot.F)
        {
            RIKFoot.F = false;
            LIKFoot.Move();
            footMoving = true;
            return;
        }
    }

    void MoveRight()
    {
        if (!LIKFoot.IsFoward() && LIKFoot.F)
        {
            LIKFoot.F = false;
            RIKFoot.Move();
            footMoving = true;
            return;
        }
    }
}
