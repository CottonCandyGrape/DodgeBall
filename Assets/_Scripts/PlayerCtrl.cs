using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { Idle, Walk, Run, Jump, Loose }

public class PlayerCtrl : MonoBehaviour
{
    [HideInInspector] public PlayerState CurState = PlayerState.Idle;

    Animator anim = null;

    Rigidbody rigid = null;
    float inputH = 0.0f;
    float inputV = 0.0f;
    Vector3 moveDir = Vector3.zero;
    float moveSpeed = 3.0f;
    float rotSpeed = 10.0f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        SetCurState();
    }

    void SetCurState()
    {
        if (CurState == PlayerState.Idle)
        {
            if (inputH != 0.0f || inputV != 0.0f)
            {
                CurState = PlayerState.Walk;

                anim.SetBool("Idle", false); //자기상태 먼저 끄고
                anim.SetBool("Walk", true); //다음상태 켜기
            }
        }
        else if (CurState == PlayerState.Walk)
        {
            if ((-0.01f <= inputH && inputH <= 0.01f) &&
               (-0.01f <= inputV && inputV <= 0.01f))
            {
                inputH = 0.0f; inputV = 0.0f;
                CurState = PlayerState.Idle;

                anim.SetBool("Walk", false);
                anim.SetBool("Idle", true);
            }
        }
        //else if (CurState == PlayerState.Run)
        //{ }
        //else if (CurState == PlayerState.Jump)
        //{ }
        //else if (CurState == PlayerState.Loose)
        //{ }
    }

    void Move()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");

        moveDir.x = inputH;
        moveDir.y = 0.0f;
        moveDir.z = inputV;
        moveDir.Normalize();

        Vector3 pos = transform.position + moveDir * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(pos);

        if (moveDir != Vector3.zero)
        {
            Quaternion target = Quaternion.Slerp(rigid.rotation, Quaternion.LookRotation(moveDir), rotSpeed * Time.fixedDeltaTime);
            rigid.MoveRotation(target);
        }
    }
}
