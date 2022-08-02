using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //따라갈 목표와 위치 오프셋
    //목표 = 플레이어
    //오프셋 = 카메라의 위치(position)
    public Transform Target;
    public Vector3 offset;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Target.position + offset;
    }
}
