using System.Collections.Generic;
using UnityEngine;


public class Body
{
    public Body(Body parent, Transform tr, int gap, float lerpSpeed)
    {
        this.parent = parent;
        this.tr = tr;
        this.lerpSpeed = lerpSpeed;

        if(parent != null)
        {
            for (int i = 1; i < gap; i++)
            {
                positionsHistory.Add(Vector3.Lerp(tr.position, parent.tr.position, i / (float)gap));
                rotateHistory.Add(Quaternion.Lerp(tr.rotation, parent.tr.rotation, i / (float)gap));
            }
            Debug.DrawLine(tr.position, parent.tr.position, Color.red, 5);
        }
    }


    public Body parent;
    public Transform tr;
    public List<Vector3> positionsHistory = new List<Vector3>();
    public List<Quaternion> rotateHistory = new List<Quaternion>();

    float lerpSpeed;

    float lerp;
    Vector3 oldPos, currentPos, newPos;
    Quaternion oldRot, currentRot, newRot;

    public void SetTarget(Vector3 TargetPos, Quaternion TargetQuternion)
    {
        lerp = 0;

        oldPos = tr.position;
        oldRot = tr.rotation;

        newPos = TargetPos;
        newRot = TargetQuternion;
    }
    public void Update()
    {
        if (oldPos == newPos) return;
        lerp += lerpSpeed * Time.deltaTime;
        lerp = Mathf.Clamp(lerp, 0.01f, 0.99f);
        currentPos = Vector3.Lerp(oldPos, newPos, lerp);
        currentRot = Quaternion.Lerp(oldRot, newRot, lerp);

        tr.position = currentPos;
        tr.rotation = currentRot;
    }
}