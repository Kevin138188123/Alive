using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
角色控制器
1.第三人称摄像机边缘进入地面
2.斜坡滑落
3.Char_Translate()爬墙
4.真实跳跃
*/
public class Character_Control : MonoBehaviour
{
    
    AudioSource audioSource;
    CharacterController controller;
    Rigidbody rigidbody;
    Animator animator;
    public Camera firstView;
    public Camera thirdView;
    //UGUI面板
    public Transform configPanel;
    public Transform settingPanel;
    public Transform gameInfoPanel;
    public Transform characterPanel;
    public Transform bagPanel;
    //GameObject water;
    float horizontal;
    float vertical;

    public float rotateSpeed, moveSpeed;//人物旋转、移动速度
    public float mouseSpeed, rollSpeed;//鼠标移动速度

    float moveOffSet, rotateOffSet;//旋转增量、移动增量

    Vector3 offset;//摄像机偏离值
    Vector3 direction;//移动方向
    Vector3 mousePos;//鼠标移动位置
    Vector3 localRotation;//第一视角鼠标旋转欧拉角
    Vector3 curPosition;
    Quaternion curRotation;

    float mouseX, mouseY, mouseSrcoll;

    bool isFirstView;
    bool isCombat;

    //角色控制器
    bool groundedPlayer;//是否触地
    Vector3 playerVelocity;//玩家速度（判断跳跃后落地重置）
    float jumpHeight = -0.3f;//跳跃高度
    float gravityValue = -9.81f;//重力值（物理重力-9.81）

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Confined;
        animator = GetComponent<Animator>();
        //默认进入游戏第三人称、左右平移
        firstView.gameObject.SetActive(false);

        offset = thirdView.transform.position - transform.position;
        //rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (settingPanel.gameObject.activeInHierarchy)
            {
                settingPanel.gameObject.SetActive(false);
                return;
            }
            else if (gameInfoPanel.gameObject.activeInHierarchy)
            {
                gameInfoPanel.gameObject.SetActive(false);
                return;
            }
            else if (characterPanel.transform.localScale == Vector3.one)
            {
                characterPanel.transform.localScale = Vector3.zero;
                return;
            }
            else if (bagPanel.transform.localScale == Vector3.one)
            {
                bagPanel.transform.localScale = Vector3.zero;
                return;
            }
            else
            {
                if (configPanel.gameObject.activeInHierarchy)
                {
                    configPanel.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    configPanel.gameObject.SetActive(true);
                    configPanel.SetAsLastSibling();
                    return;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (characterPanel.transform.localScale == Vector3.one)
            {
                characterPanel.transform.localScale = Vector3.zero;
                //characterPanel.GetComponent<CanvasGroup>().alpha = 0;
                //characterPanel.GetComponent<CanvasGroup>().interactable = false;
                //characterPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                var itemInfoPanel = GameObject.FindGameObjectWithTag("ItemInfoPanel");
                if (itemInfoPanel)
                {
                    Destroy(itemInfoPanel.gameObject);
                }
                return;
            }
            else
            {
                characterPanel.transform.localScale = Vector3.one;
                //characterPanel.GetComponent<CanvasGroup>().alpha = 1;
                //characterPanel.GetComponent<CanvasGroup>().interactable = true;
                //characterPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (bagPanel.transform.localScale == Vector3.one)
            {
                bagPanel.transform.localScale = Vector3.zero;
                var itemInfoPanel = GameObject.FindGameObjectWithTag("ItemInfoPanel");
                if (itemInfoPanel)
                {
                    Destroy(itemInfoPanel.gameObject);
                }
                return;
            }
            else
            {
                bagPanel.transform.localScale = Vector3.one;
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.F1)) //isFirstView = true ? false : true;
        {
            if (isFirstView)//第一人称
            {
                isFirstView = false;
            }
            else//第三人称
            {
                isFirstView = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Bag_Model.Instance.AddItem(1001, 10);
            //Bag_Model.Instance.AddItem(2001, 5);
            //Bag_Model.Instance.AddItem(3001, 1);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Character_Model.Instancce.Hurt();
            MessageEventSystem.Instance.SendEventMessage(EventMessage.hpUpdateMes);
        }

    }

    private void FixedUpdate()
    {
        if (!animator.GetBool("IsDeath"))
        {
            //获取键盘
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            //设置状态机
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
            if (horizontal != 0 || vertical != 0)
            {
                //跑步状态下加速
                if (animator.GetBool("IsRun"))
                {
                    if (vertical > 0)//前
                    {
                        moveOffSet = moveSpeed * 2.5f;
                    }
                    else
                    {
                        moveOffSet = moveSpeed * 1.8f;//左\右\后方向
                        rotateOffSet = rotateSpeed * 1.8f;//旋转
                    }
                }
                else//步行
                {
                    moveOffSet = moveSpeed;
                    rotateOffSet = rotateSpeed;
                }
                Char_Translate(horizontal, vertical);

                //如果声音源不在播放，保证不是每帧都重复调用播放
                if (!audioSource.isPlaying)
                {
                    //播放音频剪辑
                    audioSource.Play();
                }
            }
            else
            {
                audioSource.Stop();
            }
        }
    }
    private void LateUpdate()
    {
        //(摄像机作为子对象时旋转不同步)未与对象设为父子关系时要修改和读取偏离值
        //进来读取新偏离值更新位置
        thirdView.transform.position = transform.position + offset;
        if (isFirstView)//第一人称鼠标视角
        {
            thirdView.gameObject.SetActive(false);
            firstView.gameObject.SetActive(true);
            if (Input.GetMouseButton(1))
            {
                Cursor.visible = false;
                FirstView_R_MouseRotater();
            }
            else
            {
                Cursor.visible = true;
            }
        }
        else//第三人称视角
        {
            thirdView.gameObject.SetActive(true);
            firstView.gameObject.SetActive(false);
            //鼠标右键
            if (Input.GetMouseButton(1))
            {
                Cursor.visible = false;
                ThirdView_R_MouseMove();
            }
            else
            {
                Cursor.visible = true;
            }
            Char_MouseScrollWheel();
        }
        StayView();
    }

    /// <summary>
    /// 自定义移动：无斜坡设置
    /// </summary>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    public void Char_Translate(float horizontal, float vertical)
    {
        direction.Set(horizontal, 0, vertical);//移动向量
        direction.Normalize();//归一化，表示方向，速度控制距离

        transform.Translate(direction * moveOffSet * Time.fixedDeltaTime);
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotateOffSet * Time.fixedDeltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotateOffSet * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// 角色控制器Move()（无法触发重力触地效果）
    /// </summary>
    public void CharControl_Move(float horizontal, float vertical)
    {
        groundedPlayer = controller.isGrounded;//通过组件属性获取是否触地
        if (groundedPlayer && playerVelocity.y < 0)//落地判断（触地且玩家的方向向下）
        {
            playerVelocity.y = 0f;//确定落地
        }

        Vector3 move = new Vector3(horizontal, 0, vertical);
        controller.Move(move * Time.fixedDeltaTime * moveSpeed);

        //if (move != Vector3.zero)//判定是否有移动
        //{
        //    gameObject.transform.forward = move;//改变朝向
        //}

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)//起跳判断
        {
            //计算跳跃高度（根号公式：）
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            playerVelocity.y += jumpHeight * gravityValue;//向上的一个速度（跳跃）
        }
        //向下重力效果
        playerVelocity.y += gravityValue * Time.fixedDeltaTime;
        controller.Move(playerVelocity * Time.fixedDeltaTime);
    }


    public void CharControl_SimpleMove(float horizontal, float vertical)
    {
        //原型
        //Rotate around y - axis
        transform.Rotate(0, horizontal * rotateSpeed, 0);
        //Move forward / backward
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = moveSpeed * vertical;
        controller.SimpleMove(forward * curSpeed);

    }

    /// <summary>
    /// 角色控制器：避免爬坡滑落（无法平移）
    /// </summary>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    public void Char_RotaterMove(float _horizontal, float _vertical)
    {
        //Horizontal做旋转
        transform.Rotate(0, _horizontal * rotateOffSet * Time.fixedDeltaTime, 0);
        //Vertical做移动
        direction = transform.TransformDirection(Vector3.forward);
        controller.SimpleMove(direction * moveOffSet * _vertical * Time.fixedDeltaTime);
        //controller.Move(new Vector3(horizontal, 0, vertical) * moveOffSet);

    }

    /// <summary>
    /// 第一人称鼠标右键控制摄像机原地转动
    /// </summary>
    public void FirstView_R_MouseRotater()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime, 0);
        //**垂直旋转 * *
        mouseX -= Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;
        mouseX = Mathf.Clamp(mouseX, -90f, +90f);
        mouseY = firstView.transform.localEulerAngles.y;
        firstView.transform.localEulerAngles = new Vector3(mouseX, mouseY, 0);

        //transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime, 0);
        //mouseX += Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        //mouseY += Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;
        ////mouseX = Mathf.Clamp(mouseX, -90, 90);//屏幕X轴坐标
        //mouseY = Mathf.Clamp(mouseY, -70, 80);//屏幕Y轴坐标
        //localRotation = new Vector3(-mouseY, mouseX,
        //    firstView.transform.localEulerAngles.z);
        //firstView.transform.localEulerAngles = localRotation;//Y轴移动对应X轴

    }

    /// <summary>
    /// 第三人称鼠标右键控制摄像机围绕角色转动(含角色同时转动)，鼠标中间变焦，限制摄像机进入非法区域
    /// </summary>
    /// <param name="offset"></param>
    public void ThirdView_R_MouseMove()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;
        //水平移动
        thirdView.transform.RotateAround(transform.position, transform.up, mouseX);
        transform.RotateAround(transform.position, transform.up, mouseX);
        //垂直移动，先存后动再返回
        curPosition = thirdView.transform.position;
        curRotation = thirdView.transform.rotation;
        thirdView.transform.RotateAround(transform.position, thirdView.transform.right, -mouseY);
        //限制
        if (thirdView.transform.localEulerAngles.x < 0 || thirdView.transform.localEulerAngles.x > 70)
        {
            thirdView.transform.rotation = curRotation;
            thirdView.transform.position = curPosition;
        }
        //变后修改偏移值
        offset = thirdView.transform.position - transform.position;
    }

    /// <summary>
    /// 第三人称鼠标左键控制摄像机转动(角色不转动)，鼠标中间变焦，限制摄像机进入非法区域
    /// </summary>
    /// <param name="offset"></param>
    public void Third_L_MouseMove()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;
        //水平移动
        thirdView.transform.RotateAround(transform.position, transform.up, mouseX);
        //垂直移动，先存后动再返回
        curPosition = thirdView.transform.position;
        curRotation = thirdView.transform.rotation;
        thirdView.transform.RotateAround(transform.position, thirdView.transform.right, -mouseY);
        //限制
        if (thirdView.transform.localEulerAngles.x < 0 || thirdView.transform.localEulerAngles.x > 70)
        {
            thirdView.transform.rotation = curRotation;
            thirdView.transform.position = curPosition;
        }
        //变后修改偏移值
        offset = thirdView.transform.position - transform.position;
    }

    /// <summary>
    /// 鼠标滚轮
    /// </summary>
    public void Char_MouseScrollWheel()
    {
        mouseSrcoll = Input.GetAxis("Mouse ScrollWheel") * rollSpeed * Time.deltaTime;
        thirdView.fieldOfView -= mouseSrcoll;
        thirdView.fieldOfView = Mathf.Clamp(thirdView.fieldOfView, 3.5f, 100f);
    }

    /// <summary>
    /// 禁止摄像机进入非法区域
    /// </summary>
    public void StayView()
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up, thirdView.transform.position, out hit))
        {
            string name = hit.collider.gameObject.tag;
            if (name != "ThirdView")
            {
                thirdView.transform.position = hit.point + new Vector3(1f, 1f, 0);
            }
        }
        //var dis = thirdView.transform.position - transform.position;
        //if (Physics.SphereCast(transform.position + Vector3.up,1, dis, out hit))
        //{
        //    string name = hit.collider.gameObject.tag;
        //    if (name != "ThirdView")
        //    {
        //        thirdView.transform.position = hit.point + new Vector3(1f, 1f, 0);
        //    }
        //}
    }
}



