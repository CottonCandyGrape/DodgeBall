using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    Rigidbody rigid = null;
    float inputH = 0.0f;
    float inputV = 0.0f;
    Vector3 moveDir = Vector3.zero;
    float moveSpeed = 3.0f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    //void Start() { }

    void FixedUpdate()
    {
        Move();
    }

    //void Update() { }

    private void Move()
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        moveDir.x = inputH;
        moveDir.y = 0.0f;
        moveDir.z = inputV;
        moveDir.Normalize();

        Vector3 pos = transform.position + moveDir * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(pos);
        rigid.MoveRotation(Quaternion.LookRotation(moveDir));
    }


}
