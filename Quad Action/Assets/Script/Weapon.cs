using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range};
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");     //�ڷ�ƾ ����
            StartCoroutine("Swing");    //�ڷ�ƾ ����

        }
    }

    IEnumerator Swing()
    {
        //1 
        yield return new WaitForSeconds(0.1f); //1.2�� ���
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        //2
        yield return new WaitForSeconds(0.3f); //1�� ���
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f); //1�� ���
        trailEffect.enabled = false;


    }
}


