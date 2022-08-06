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
    bool jDown;         //점프
    bool isjump;
    bool isDodge;


    Vector3 moveVec;
    Vector3 dodgeVec;
    Animator anim;

    Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
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
        getInput();
        move();
        turn();
        jump();
        dodge();


        
    }

    void getInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");            //Shift 누르고 있을때만 작동
        jDown = Input.GetButtonDown("Jump");
    }

    void move()
    {
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
        if (isDodge)
            moveVec = dodgeVec;

        transform.position += moveVec * (wDown ? 1.0f : first_speed) * Time.deltaTime;


        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void turn()
    {
        //LookAt() 지정된 벡터를 향해서 회전시켜주는 함수
        //현재 위치 + 나아가야하는 방향벡터로 회전
        transform.LookAt(transform.position + moveVec);
    }

    void jump()
    {
        if (jDown && moveVec == Vector3.zero && !isjump && !isDodge)
        {
            rigid.AddForce(Vector3.up * 15 ,ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump"); 
            isjump = true;
        }
        
    }

    void dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isjump )
        {

            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;


            //시간차 호출
            Invoke("dodgeOut", 0.5f);
        }

    }

    void dodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "floor")
        {
            isjump = false;
            anim.SetBool("isJump", false);

        }
    }
}
