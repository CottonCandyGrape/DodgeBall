using System;
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

    bool readyRun = false;
    bool isRun = false;
    float runTimer = 0.5f;
    float runTime = 0.5f;
    KeyCode dirKey = KeyCode.None;

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

    void Update()
    {
        SetCurState();

        Move();

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
                moveSpeed = walkSpeed;

                CurState = PlayerState.Walk;
                ChangeAnimParam(PlayerState.Walk);
                return;
            }

            ReadyRun();
        }
        else if (CurState == PlayerState.Walk)
        {
            if ((-0.01f <= inputH && inputH <= 0.01f) &&
               (-0.01f <= inputV && inputV <= 0.01f))
            {
                inputH = 0.0f; inputV = 0.0f;

                moveSpeed = 0.0f;

                CurState = PlayerState.Idle;
                ChangeAnimParam(PlayerState.Idle);
            }
        }
        else if (CurState == PlayerState.Run)
        {
            Debug.Log("run이다");
            if(Input.GetKeyUp(dirKey))
            {
                isRun = false;

                runTimer = runTime;
                moveSpeed = 0.0f;
                dirKey = KeyCode.None;

                CurState = PlayerState.Idle;
                ChangeAnimParam(PlayerState.Idle);
            }
        }
        //else if (CurState == PlayerState.Jump)
        //{ }
        //else if (CurState == PlayerState.Loose)
        //{ }
    }

    void ChangeAnimParam(PlayerState pState)
    {
        for (int i = 0; i < anim.parameterCount; i++)
        {
            if (pState.ToString() == anim.parameters[i].name)
                anim.SetBool(anim.parameters[i].name, true);
            else
                anim.SetBool(anim.parameters[i].name, false);
        }
    }

    void ReadyRun()
    {
        if (dirKey == KeyCode.None) return;

        runTimer -= Time.deltaTime;
        if (runTimer < 0.0f)
        {
            runTimer = runTime;
            return;
        }

        KeyCode curKey = GetCurKeyDown();
        Debug.Log("down curkey : " + curKey.ToString());

        if (curKey == KeyCode.None) return;

        if (curKey == dirKey)
        {
            Debug.Log("같다");
            isRun = true;
            moveSpeed = runSpeed;

            CurState = PlayerState.Run;
            ChangeAnimParam(PlayerState.Run);
        }
        else
        {
            dirKey = curKey;
        }
    }

    void Move()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");

        //if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        //    && !readyRun)
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            dirKey = 0.0f < inputH ? KeyCode.D : KeyCode.A;
            Debug.Log("dirkey : " + dirKey.ToString());
            //readyRun = true;
        }

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

    KeyCode GetCurKeyDown() //현재 무슨 키가 눌렸는지 curKey에 할당
    {
        KeyCode[] keys = (KeyCode[])Enum.GetValues(typeof(KeyCode));
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]))
                return keys[i];
        }
        return KeyCode.None;
    }
}
