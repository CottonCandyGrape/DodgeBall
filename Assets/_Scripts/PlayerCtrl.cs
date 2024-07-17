using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { Idle, Walk, Run, Jump, Loose }

public class PlayerCtrl : MonoBehaviour
{
    [HideInInspector] public PlayerState CurState = PlayerState.Idle;

    CharacterController charCtrl = null;
    Animator anim = null;

    //이동관련
    float inputH = 0.0f;
    float inputV = 0.0f;
    Vector3 moveDir = Vector3.zero;
    float moveSpeed = 0.0f;
    float walkSpeed = 1.5f;
    float runSpeed = 4.0f;
    float rotSpeed = 10.0f;
    //이동관련

    public BallCtrl Ball = null;
    public Transform LeftHandPos = null;
    public Transform RightHandPos = null;

    void Awake()
    {
        charCtrl = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {

    }

    //void FixedUpdate() { //Move(); }

    void Update()
    {
        Move();

        SetCurState();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ball.transform.parent = RightHandPos;
            Ball.transform.localPosition = Vector3.zero;
            Ball.transform.localRotation = Quaternion.identity;

            Ball.Rigid.useGravity = false;
            Ball.Rigid.velocity = Vector3.zero;
            Ball.Rigid.angularVelocity = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Ball.transform.parent = null;
            Rigidbody rb = Ball.GetComponentInChildren<Rigidbody>();
            rb.AddForce(transform.forward * 500);

            Ball.Rigid.useGravity = true;
        }
    }

    //void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    Debug.Log(hit.gameObject.name); 
    //}

    void SetCurState()
    {
        if (CurState == PlayerState.Idle)
        {
            if (inputH != 0.0f || inputV != 0.0f)
            {
                CurState = PlayerState.Walk;

                moveSpeed = walkSpeed;

                ChangeAnimState(PlayerState.Walk);
            }
        }
        else if (CurState == PlayerState.Walk)
        {
            if ((-0.01f <= inputH && inputH <= 0.01f) &&
               (-0.01f <= inputV && inputV <= 0.01f))
            {
                inputH = 0.0f; inputV = 0.0f;
                CurState = PlayerState.Idle;

                moveSpeed = 0.0f;

                ChangeAnimState(PlayerState.Idle);
            }
        }
        else if (CurState == PlayerState.Run)
        {

        }
        //else if (CurState == PlayerState.Jump)
        //{ }
        //else if (CurState == PlayerState.Loose)
        //{ }
    }

    void ChangeAnimState(PlayerState pState)
    {
        for (int i = 0; i < anim.parameterCount; i++)
        {
            if (pState.ToString() == anim.parameters[i].name)
                anim.SetBool(anim.parameters[i].name, true);
            else
                anim.SetBool(anim.parameters[i].name, false);
        }
    }

    void Move()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");

        moveDir.x = inputH;
        moveDir.y = 0.0f;
        moveDir.z = inputV;
        moveDir.Normalize();

        charCtrl.SimpleMove(moveDir * moveSpeed);

        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(moveDir), rotSpeed * Time.deltaTime);
        }
    }
}
