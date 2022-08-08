using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    //공전체 회전을 위한 스크립트
    public Transform target;
    public float orbitSpeed;
    Vector3 offset;


    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
        //타겟 주위를 회전하는 함수 
        //목표가 움직이면 일그러지는 단점
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
        offset = transform.position - target.position;
    }
}
