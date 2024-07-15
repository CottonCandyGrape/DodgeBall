using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTest : MonoBehaviour
{
    public Transform Target = null;
    public BallCtrl ball = null;
    Rigidbody ballRigid = null;

    Vector3 dir = Vector3.zero;

    void Start()
    {
        ballRigid = ball.Rigid;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            dir = Target.position - ball.transform.position;
            dir.Normalize();

            ball.transform.parent = null;
            ball.Rigid.useGravity = true;
            ballRigid.AddForce(dir * 1000);
        }
    }
}
