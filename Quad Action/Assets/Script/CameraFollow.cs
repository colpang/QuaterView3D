using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //���� ��ǥ�� ��ġ ������
    //��ǥ = �÷��̾�
    //������ = ī�޶��� ��ġ(position)
    public Transform Target;
    public Vector3 offset;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Target.position + offset;
    }
}
