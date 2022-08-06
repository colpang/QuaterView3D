using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Ammo, Coin, Grenade, Heart, Weapon};
    public Type type;
    public int value;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //제자리에서 회전하는 효과
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);    
    }
}
