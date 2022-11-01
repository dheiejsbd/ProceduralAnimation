using System;
using System.Collections.Generic;
using UnityEngine;

public class IKBodySolver : MonoBehaviour
{
    [SerializeField] Transform body;
    [SerializeField] Transform target;
    [SerializeField] float legDistance = 2.6f;
    [SerializeField] float offset;
    [SerializeField] Transform[] legEnds;
    [SerializeField] float moveSpeed;
    Transform parent;
    float parentY;
    float oldPos, currentPos, newPos;
    Vector3 oldNormal, currentNormal, newNormal;
    float lerp = 0;
    public void OnInitialize(Transform parent)
    {
        this.parent = parent;
        parentY = parent == null? 0: parent.position.y;
        oldPos = currentPos = newPos = target.position.y;
    }

    public void Update()
    {
        if(lerp < 1)
        {
            currentPos = Mathf.Lerp(oldPos, newPos, lerp);
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * moveSpeed;
        }
        else
        {
            oldPos = newPos;
            oldNormal = newNormal;
            currentPos = newPos;
            currentNormal = newNormal;
        }
        Vector3 offset = parent == null? Vector3.zero : Vector3.up * (body.position.y - parentY);
        target.position = new Vector3(target.position.x, currentPos, target.position.z) + offset;
        var up = currentNormal;

        var forward = parent == null ? normal() : parent.position - target.position;
        var right = -Vector3.Cross(forward, currentNormal);
        Matrix4x4 M = new Matrix4x4();
        M.SetColumn(0, right);
        M.SetColumn(1, up);
        M.SetColumn(2, forward);

        Quaternion Q = QuaternionFromMatrix(M);
        target.rotation = Q;
    }


    Vector3 normal()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down * 2, out hit, LayerMask.NameToLayer("Ground"));
        return transform.forward + hit.normal;
    }

    public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
    {
        return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
    }




    public void Set()
    {
        lerp = 0;
        
        float sum = 0;
        foreach (var item in legEnds)
        {
            sum += Vector3.Dot(body.up, item.position);
        }
        sum /= legEnds.Length;

        oldPos = transform.position.y;
        newPos = sum + offset;
        if(parent != null) parentY = body.position.y;

        float left = 0, right = 0;
        int leftCount = 0, rightCount = 0;
        foreach (var item in legEnds)
        {
            var deltaPos = item.position - body.position;

            float dot = Vector3.Dot(body.right, deltaPos);
            if (dot > 0)
            {
                //오른쪽
                right += item.position.y;
                rightCount++;
            }
            else if (dot < 0)
            {
                //왼쪽
                left += item.position.y;
                leftCount++;
            }
        }
        if (rightCount == 0 || leftCount == 0) return;
        right /= rightCount;
        left /= leftCount;

        Vector3 inclination = Vector3.zero;
        int rightLeft = (right - left) > 0 ? 1 : -1;
        inclination += body.right * legDistance * rightLeft;
        inclination += body.up * Mathf.Abs(right - left);

        oldNormal = target.up;
        var forward = parent == null ? normal() : parent.position - target.position;
        newNormal = Vector3.Cross(forward * rightLeft, inclination);
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(body.position, newNormal * 10, Color.green);
        Debug.DrawRay(body.position, Vector3.up * 10, Color.red);
        Debug.DrawRay(body.position, target.up * 10, Color.blue);


        var forward = parent == null ? normal() : parent.position - target.position;
        Debug.DrawRay(target.position, forward, Color.cyan);

        Debug.DrawRay(body.position, newNormal* 10, Color.green);
        Debug.DrawRay(body.position, -Vector3.Cross(body.forward, newNormal)* 10, Color.red);
        Debug.DrawRay(body.position, body.forward * 10, Color.blue);
    }
}
