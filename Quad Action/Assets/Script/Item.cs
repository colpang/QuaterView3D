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
        //GetComponent는 가장 첫 컴포넌트를 가져옴 
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }
    void Update()
    {
        //제자리에서 회전하는 효과
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
