using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    float first_speed;
    float hAxis;        //Ⱦ�̵�
    float vAxis;        //���̵�
    bool wDown;         //walk ����Ű�Է�(shift)

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
        wDown = Input.GetButton("Walk");            //Shift ������ �������� �۵�
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;



        /*if (wDown) {
            speed = 1.0f;
        }
        else
        {
            speed = first_speed;
        }
        transform.position += moveVec * speed * Time.deltaTime;*/
        //���׿����� �Ἥ ���ٷ� �ۼ�   wDown�� true�̸� 1.0, false�� first_speed
        transform.position += moveVec * (wDown ? 1.0f : first_speed) * Time.deltaTime;


        anim.SetBool("isRun",moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);


        //LookAt() ������ ���͸� ���ؼ� ȸ�������ִ� �Լ�
        //���� ��ġ + ���ư����ϴ� ���⺤�ͷ� ȸ��
        transform.LookAt(transform.position + moveVec);
    }
}
