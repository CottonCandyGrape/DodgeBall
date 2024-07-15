using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCtrl : MonoBehaviour
{
    SphereCollider spColl = null;
    public SphereCollider SpColl { get { return spColl; } }

    Rigidbody rigid = null;
    public Rigidbody Rigid { get { return rigid; } }

    void Awake()
    {
        spColl = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();
    }

    //void Start() { }

    //void Update() { }
}
