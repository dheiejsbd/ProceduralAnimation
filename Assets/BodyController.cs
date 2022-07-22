using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] GameObject body;
    [SerializeField] int bodyCount;
    [SerializeField] float bodySize;
    [SerializeField] float rotateSpeed = 1;
    [SerializeField] bool showBone;

    Transform[] bodys;
    
    private void Start()
    {
        bodys = new Transform[bodyCount];
        for (int i = 0; i < bodyCount; i++)
        {
            bodys[i] = Instantiate(body, transform).transform;
            if(i != 0) bodys[i].transform.position = bodys[i - 1].transform.position - bodys[i - 1].transform.forward * bodySize;
            bodys[i].GetComponentInChildren<IKBodySolver>().OnInitialize(i ==0? null: bodys[i-1].Find("Root"));
        }
        bodys[0].gameObject.AddComponent<move>();
    }

    public void Update()
    {
        for (int i = 1; i < bodyCount; i++)
        {
            Transform parent = bodys[i - 1];
            Transform target = bodys[i];

            Vector3 parentPos = parent.position - parent.forward * bodySize / 2;
            Vector3 targetPos = target.position + target.forward * bodySize / 2;
            Vector3 deltaPos = parentPos - targetPos;
            target.position += deltaPos;

            deltaPos.y = 0;
            Quaternion targetAngle = Quaternion.Lerp(target.rotation, parent.rotation, Time.deltaTime * rotateSpeed);
            target.rotation = targetAngle;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.blue;
        for (int i = 1; i < bodyCount; i++)
        {
            Transform parent = bodys[i - 1];
            Transform target = bodys[i];
            Gizmos.DrawLine(parent.position, target.position);
        }
    }
}
