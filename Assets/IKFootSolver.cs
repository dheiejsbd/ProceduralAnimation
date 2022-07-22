using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] Transform body;
    [SerializeField] Transform target;
    [SerializeField] float stepHeight;
    [SerializeField] float stepSpeed;
    [SerializeField] float maxDistanve;
    [SerializeField] float moveDistanve;
    [SerializeField] float sideDistance;
    [SerializeField] LayerMask terrainLayerMask;
    [SerializeField] float gizmosSize;

    Vector3 oldPos, currentPos, newPos;
    Vector3 oldNormal, currentNormal, newNormal;

    public bool isMoving { get; private set; }
    public bool F;
    public float lerp = 0;
    public void Initialize()
    {
        RaycastHit hit = GetPos(new Ray(body.position + body.right * sideDistance + body.up * 1.5f, -body.up));
        target.position = hit.point;
        target.up = hit.normal;
        oldPos = currentPos = newPos = target.position;
        oldNormal = currentNormal = newNormal = target.up;
        F = true;
    }

    public void Move()
    {
        RaycastHit hit = GetPos();
        newPos = hit.point;
        newNormal = hit.normal;
        lerp = 0;
        isMoving = true;
        F = true;
    }

    public void OnUpdate()
    {
        Solve();
        target.position = currentPos;
        target.up = currentNormal;
    }

    void Solve()
    {
        if (!isMoving)
        {
            if (Vector3.Distance(target.position, GetPos().point) >= maxDistanve * 2)
            {
                Move();
            }
            return;
        }
        if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPos, newPos, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPos = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * stepSpeed;
        }
        else
        {
            oldPos = newPos;
            oldNormal = newNormal;
            currentPos = newPos;
            currentNormal = newNormal;

            isMoving = false;
        }
    }

    public bool IsFoward()
    {
        Vector2 f = new Vector2(body.forward.x, body.forward.z);
        var delta = target.position - body.position;
        Vector2 t = new Vector2(delta.x, delta.z);
        return Vector2.Dot(f, t) > 0;
    }
    RaycastHit GetPos()
    {
        var ray = new Ray(body.position + body.right * sideDistance + body.forward * moveDistanve + body.up * 1.5f, -body.up);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, int.MaxValue, terrainLayerMask);
        if (hit.point == Vector3.zero) hit.point = body.position + body.right * sideDistance + body.forward * moveDistanve + body.up * -1.5f;
        return hit;
    }
    RaycastHit GetPos(Ray ray)
    {
        RaycastHit hit;
        Physics.Raycast(ray, out hit, int.MaxValue, terrainLayerMask);
        return hit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetPos().point, gizmosSize);
        Gizmos.DrawWireSphere(target.position, maxDistanve);
    }
}
