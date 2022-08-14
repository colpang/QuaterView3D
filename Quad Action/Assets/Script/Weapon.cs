using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range};
    public Type type;
    public int damage;
    public float rate;

    public int maxAmmo, curAmmo;    //전체 탄약, 현재 탄약 변수

    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public Transform bulletPos;     //총알 위치
    public GameObject bullet;

    public Transform bulletCasePos;     //탄피 위치
    public GameObject bulletCase;



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
            StopCoroutine("Swing");     //코루틴 종료
            StartCoroutine("Swing");    //코루틴 실행

        }
        else if(type == Type.Range && curAmmo >0)
        {
            curAmmo--;
            StopCoroutine("Shot");
            StartCoroutine("Shot");
        }

    }

    IEnumerator Swing()
    {
        //1 
        yield return new WaitForSeconds(0.1f); //1.2초 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        //2
        yield return new WaitForSeconds(0.3f); //1초 대기
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f); //1초 대기
        trailEffect.enabled = false;


    }

    IEnumerator Shot()
    {
        //1.총알 발사
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;

        yield return null;

        //2.탄피 배출
        GameObject intantBulletCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = intantBulletCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -1) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

    }
}


