using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    float first_speed;
    float hAxis;        //횡이동
    float vAxis;        //종이동
    bool wDown;         //walk 조작키입력(shift)

    Vector3 moveVec;
    Animator anim;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        first_speed = speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");            //Shift 누르고 있을때만 작동
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;



        /*if (wDown) {
            speed = 1.0f;
        }
        else
        {
            speed = first_speed;
        }
        transform.position += moveVec * speed * Time.deltaTime;*/
        //삼항연산자 써서 한줄로 작성   wDown이 true이면 1.0, false면 first_speed
        transform.position += moveVec * (wDown ? 1.0f : first_speed) * Time.deltaTime;


        anim.SetBool("isRun",moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);


        //LookAt() 지정된 벡터를 향해서 회전시켜주는 함수
        //현재 위치 + 나아가야하는 방향벡터로 회전
        transform.LookAt(transform.position + moveVec);
    }
}
