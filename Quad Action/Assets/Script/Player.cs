using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    float first_speed;
    float hAxis;        //횡이동
    float vAxis;        //종이동

    public Camera followCamera;

    //탄약, 코인, 하트, 수류탄 소지 변수
    //인스펙터에서 설정
    public int ammo, coin, health, hasGrenades; 
    public int max_ammo, max_coin, max_health, max_hasGrenades; 

    bool wDown;         //walk 조작키입력(shift)
    bool jDown;         //점프
    bool iDown;         //아이템 입수 입력
    bool fDown;         //공격
    bool rDown;         //재장전
    bool gDown;



    bool isjump;
    bool isDodge;
    bool isSwap;
    bool isFireReady = true;
    bool isReload;
    bool isBoarder;

    bool sDown1, sDown2, sDown3;
    

    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public GameObject grenadeObj;
    


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
        Reload();
        Turn();
        Grenade();
    }

    void getInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");            //Shift 누르고 있을때만 작동
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interation");
        fDown = Input.GetButton("Fire1");
        gDown = Input.GetButtonDown("Fire2");
        rDown = Input.GetButtonDown("Reload");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");



    }

    void move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if (isDodge)
            moveVec = dodgeVec;


        if (isSwap || !isFireReady || isReload)
            moveVec = Vector3.zero;

        if(!isBoarder)
            transform.position += moveVec * (wDown ? 1.0f : first_speed) * Time.deltaTime;


        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {

        //#1 키보드에 의한 회전
        transform.LookAt(transform.position, moveVec);

        //#2 마우스에 의한 회전

        if (fDown)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                Vector3 nextVec = hitInfo.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
        
    }

    void Attack(){
        if(equipWeapon == null)
        {
            return;
        }

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isDodge && !isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
    }

    void Grenade()
    {
        if (hasGrenades == 0)
            return;

        if(gDown && !isReload && !isSwap)
        {

            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                Vector3 nextVec = hitInfo.point - transform.position;
                nextVec.y = 7;
                GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation);
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
                rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
                rigidGrenade.AddTorque(Vector3.back*10, ForceMode.Impulse);

                hasGrenades--;
                grenades[hasGrenades].SetActive(false);
            }
        }
    }
    void Reload()
    {
        if(equipWeapon == null)
        {
            return;
        }
        if(equipWeapon.type == Weapon.Type.Melee)
        {
            return;
        }

        if (ammo == 0)
            return;

        if(rDown && !isDodge && !isjump && !isSwap && isFireReady)
        {
            anim.SetTrigger("doReload");
            isReload = true;

            Invoke("ReloadOut", 2f);
        }
    }

    void ReloadOut()
    {
        
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
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
        isSwap = false;
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

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }
    void StopToWall()
    {
        //Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBoarder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }
    private void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
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
