using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Ammo, Coin, Grenade, Heart, Weapon};
    public Type type;
    public int value;



    Rigidbody rigid;
    SphereCollider sphereCollider;

    private void Awake()
    {
        //GetComponent�� ���� ù ������Ʈ�� ������ 
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }
    void Update()
    {
        //���ڸ����� ȸ���ϴ� ȿ��
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
}
