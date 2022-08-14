using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    float first_speed;
    float hAxis;        //횡이동
    float vAxis;        //종이동


    //탄약, 코인, 하트, 수류탄 소지 변수
    //인스펙터에서 설정
    public int ammo, coin, health, hasGrenades; 
    public int max_ammo, max_coin, max_health, max_hasGrenades; 

    bool wDown;         //walk 조작키입력(shift)
    bool jDown;         //점프
    bool iDown;         //아이템 입수 입력
    bool fDown;         //공격키


    bool isjump;
    bool isDodge;
    bool isSwap;
    bool isFireReady = true;

    bool sDown1, sDown2, sDown3;
    

    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;


    Vector3 moveVec;
    Vector3 dodgeVec;
    Animator anim;

    Rigidbody rigid;


    GameObject nearObject;
    Weapon equipWeapon;
    float fireDelay;

    int equipWeaponIndex = -1;


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
        interation();
        swap();
        Attack();

        
    }

    void getInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");            //Shift 누르고 있을때만 작동
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interation");
        fDown = Input.GetButtonDown("Fire1");

        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");


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


        if (isSwap || !isFireReady)
            moveVec = Vector3.zero;

        transform.position += moveVec * (wDown ? 1.0f : first_speed) * Time.deltaTime;


        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Attack(){
        if(equipWeapon == null)
        {
            return;
        }

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isDodge && isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger("doSwing");
            fireDelay = 0;
        }
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
        if (jDown && moveVec != Vector3.zero && !isjump && !isSwap)
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





    void swap()
    {
        int weaponIndex = -1;


        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;

  

        

        if((sDown1 || sDown2 || sDown3) && !isjump && !isDodge)
        {
            if (sDown1) weaponIndex = 0;
            else if (sDown2) weaponIndex = 1;
            else if (sDown3) weaponIndex = 2;


            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);
            anim.SetTrigger("doSwap");

            isSwap = true;
            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {
        isDodge = false;
    }


    void interation()
    {
        if(iDown && nearObject != null && !isjump && !isDodge)
        {
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;

                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);

            }
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            isjump = false;
            anim.SetBool("isJump", false);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = other.gameObject;
            Debug.Log(nearObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            Debug.Log("실행");
            Item item = other.GetComponent<Item>();
            Debug.Log(item.gameObject.name);
            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > max_ammo)
                    {
                        ammo = max_ammo;
                    }
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > max_coin)
                    {
                        coin = max_coin;
                    }
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if(health >max_health)
                    {
                        health = max_health;
                    }
                    break;
                case Item.Type.Grenade:
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if(hasGrenades > max_hasGrenades)
                    {
                        hasGrenades = max_hasGrenades;
                    }


                    break;

            }
            Destroy(other.gameObject);
        }


    }
}
