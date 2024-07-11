using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    Rigidbody rigid = null;
    float inputH = 0.0f;
    float inputV = 0.0f;
    Vector3 moveDir = Vector3.zero;
    float moveSpeed = 5.0f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");
        moveDir.x = inputH;
        moveDir.z = inputV;
        moveDir.Normalize();

        Vector3 pos = transform.position + moveDir * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(pos);
    }

    void Update()
    {
        
    }
}
