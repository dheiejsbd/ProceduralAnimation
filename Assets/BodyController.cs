using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] GameObject body;
    [SerializeField] int bodyCount;
    [SerializeField] public float bodySize = 1f;

    [SerializeField] int gap = 10;
    [SerializeField] float minimumDist = 1f;
    [SerializeField] float lerpSpeed;

    Body bodyTail = null;
    Body bodyhead;
    Vector3 lastHeadPos;

    private void Start()
    {
        for (int i = 0; i < bodyCount; i++)
        {
            GameObject obj = Instantiate(body, transform);
            if (i != 0) obj.transform.position = bodyhead.tr.position - bodyhead.tr.forward * bodySize * i;
            obj.GetComponentInChildren<IKBodySolver>().OnInitialize(i == 0 ? null : bodyTail.tr.Find("Root"));

            bodyTail = new Body(bodyTail, obj.transform, gap, lerpSpeed);

            if (i == 0)
            {
                lastHeadPos = obj.transform.position;
                bodyhead = bodyTail;
            }
        }
        bodyhead.tr.gameObject.AddComponent<move>();
    }

    private void Update()
    {
        Body body = bodyTail;

        while (body.parent != null)
        {
            body.Update();
            body = body.parent;
        }
    }
    void FixedUpdate()
    {
        if (Vector3.Distance(bodyhead.tr.position, lastHeadPos) < minimumDist) return;
        lastHeadPos = bodyhead.tr.position;
        Body body = bodyTail;

        while (body.parent != null)
        {
            body.SetTarget(body.positionsHistory[0], body.rotateHistory[0]);
            body.positionsHistory.RemoveAt(0);
            body.positionsHistory.Add(body.parent.tr.position);
            body.rotateHistory.RemoveAt(0);
            body.rotateHistory.Add(body.parent.tr.rotation);
            body = body.parent;
        }
    }
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Body body = bodyTail;

        while (body.parent != null)
        {
            Gizmos.color = Color.black;
            for (int i = 1; i < body.positionsHistory.Count; i++)
            {
                Gizmos.DrawLine(body.positionsHistory[i - 1], body.positionsHistory[i]);
            }
            body = body.parent;
        }
    }
}
