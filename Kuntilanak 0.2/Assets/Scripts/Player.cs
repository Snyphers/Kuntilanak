using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    MainDoor MD;
    DoorScrpt DS;
    OpenShelf OS;
    PlayerInputAction PlayerControl;
    [SerializeField] PauseSetting PS;
    [SerializeField] GameInspector GI;
    [SerializeField] HantuCordination HC;

    [SerializeField] CollectableScript Item;

    [SerializeField] ItemHolder IH;
    [SerializeField] TextMeshProUGUI ItemName;
    [SerializeField] Image ItemIcon;
    [SerializeField] GameObject ColectableUI;

    [SerializeField] CharacterController Controller;
    [SerializeField] Rigidbody RigidBody;
    [SerializeField] Collider PlayerCol;

    [SerializeField] float Jump;
    [SerializeField] float Speed;
    [SerializeField] public float CHealth;
    //[SerializeField] Vector3 Velocity;
    float Gravity = -8.81f;

    [SerializeField] Image touchCurrentState;
    [SerializeField] Transform HealthBarA;
    [SerializeField] Slider HealthBar;
    [SerializeField] Sprite TouchOff;
    [SerializeField] Sprite TouchOn;

    [SerializeField] GameObject ShowItem;
    [SerializeField] Image CurrentItemShow;
    [SerializeField] TextMeshProUGUI ItemCount;
    [SerializeField] TextMeshProUGUI UIItemsName;
    [SerializeField] GameObject DeadthUI;

    float Scrolly;

    [SerializeField] GameObject DoorUI;
    [SerializeField] TextMeshProUGUI DoorText;

    [SerializeField] GameObject Message;
    [SerializeField] TextMeshProUGUI MessageText;

    [SerializeField] GameObject OpenShelf;
    [SerializeField] TextMeshProUGUI ShelfText;

    [SerializeField] Light TouchLight;

    [SerializeField] GameObject GetHIt;
    [SerializeField] Animator GetHitAni;

    public float MouseSX;
    public float MouseSY;
    float YRotation = 0f;
    [SerializeField] Transform Camera;

    bool Budak;
    bool SpeedUp;
    bool IsGrounded;
    bool AttackedOnCD;
    public bool Pause;
    bool TouchLightCon;

    bool CannotOpen;

    private void Awake()
    {
        MouseSX = GI.MouseX;
        MouseSY = GI.MouseY;
        HealthBar.value = CHealth;

        PlayerControl = new PlayerInputAction();

        PlayerControl.Player.Pause.performed += IDK => OnPause();
        PlayerControl.Player.TurnOnFlash.performed += UGH => Flash();
        PlayerControl.Player.CollectItem.performed += PickUp => PressE();

        PlayerControl.Player.ScrollItem.performed += ChangeItem => Scrolly = ChangeItem.ReadValue<float>();
        PlayerControl.Player.ScrollItem.performed += ScrollY => ChangeItem();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        //IsGrounded = Controller.isGrounded;
        //if (IsGrounded && Velocity.y < 0)
        //{
        //    Velocity.y = 0f;
        //}

        float MouseX = GetMouseDelta().x * MouseSX * Time.deltaTime;
        float MouseY = GetMouseDelta().y * MouseSY * Time.deltaTime;

        Vector3 move = transform.right * GetPlayerMovement().x + transform.forward * GetPlayerMovement().y;
        Vector3 NewMovePos = new Vector3(move.x * Speed , RigidBody.velocity.y, move.z * Speed);
        RigidBody.velocity = NewMovePos;
        //Controller.Move(move * Speed * Time.deltaTime);

        YRotation -= MouseY;
        YRotation = Mathf.Clamp(YRotation, -90f, 90f);

        Camera.localRotation = Quaternion.Euler(YRotation, 0f, 0f);
        transform.Rotate(Vector3.up * MouseX);

        //RigidBody.velocity.y = Velocity.y;
        //RigidBody.velocity = move * Speed * Gravity;
        //Controller.Move(Velocity * Time.deltaTime);

        if (SpeedUp)
        {
            Speed -= 1 * Time.deltaTime;
            if (Speed <= 5)
            {
                SpeedUp = false;
                Speed = 5;
            }
        }

        if (Budak && TouchLightCon)
        {
            CHealth -= 0.5f;
            HealthSystem();
        }
    }

    private void FixedUpdate()
    {
        if (HealthBar.value > CHealth)
        {
            HealthBar.value -= 0.5f;
            HealthBarA.localScale = new Vector3(1.2f ,1.2f ,1);
        }
        else if (HealthBar.value < CHealth)
        {
            HealthBar.value += 0.5f;
            HealthBarA.localScale = new Vector3(1.2f ,1.2f , 1);
        }
        else
        {
            HealthBarA.localScale = new Vector3(1 ,1 , 1);
        }
    }

    private void OnPause()
    {
        if (Pause)
        {
            GameObject ItemDetail = GameObject.FindWithTag("ItemDetail");
            if (ItemDetail != null)
            {
                Destroy(ItemDetail);
            }

            Pause = false;
            Time.timeScale = 1f;
            PS.PauseMenu.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Pause = true;
            Time.timeScale = 0f;
            Cursor.visible = true;
            PS.PauseMenu.SetActive(true);

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Collectable":
                ColectableUI.SetActive(true);
                IH = other.GetComponent<ItemHolder>();
                Item = IH.CS;
                ItemName.text = "[E] Collect " + Item.Item.ToString();
                ItemIcon.sprite = Item.Icon;
                break;
            case "Door":
                DS = other.GetComponent<DoorScrpt>();
                switch (DS.DoorType)
                {
                    case DoorScrpt.Doortype.NormalDoor:
                        switch (DS.Condition)
                        {
                            case true:
                                 DoorText.text = "[E] Close Door";
                                break;
                            case false:
                                DoorText.text = "[E] Open Door";
                                break;
                        }
                        DoorUI.SetActive(true);
                        break;
                    case DoorScrpt.Doortype.OfficeDoor:
                        switch (DS.Condition)
                        {
                            case false:
                                DoorText.text = "[E] Open Door";
                                DoorUI.SetActive(true);
                                break;
                        }
                        break;
                }
                break;
            case "Main Door":
                MD = other.GetComponent<MainDoor>();
                if (!MD.Active)
                {
                    DoorText.text = "[E] Open Door";
                    DoorUI.SetActive(true);
                }
                break;
            case "Shelf":
                OS = other.GetComponent<OpenShelf>();
                if (!OS.IsOpen)
                {
                    ShelfText.text = "[E] Open";
                    OpenShelf.SetActive(true);
                }
                break;
            case "Budak":
                Budak = true;
                GetHIt.SetActive(true);
                break;
            case "Tangga":
                HC.OnStairs = true;
                HC.DestroyHantu();
                break;
            case "1":

                break;
            case "2":

                break;
            case "3":

                break;
            case "End Game":
                PS.CreditScene();
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.tag)
        {
            case "Kuntilanak":
                if (!SpeedUp)
                {
                    GotHit();
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Collectable":
                ColectableUI.SetActive(false);
                break;
            case "Door":
                DoorUI.SetActive(false);
                break;
            case "Main Door":
                DoorUI.SetActive(false);
                MD = null;
                break;
            case "Shelf":
                OpenShelf.SetActive(false);
                break;
            case "Budak":
                Budak = false;
                GetHitAni.SetBool("Doit", true);
                StartCoroutine(MinusHealthAnimation());
                break;
            case "Tangga":
                HC.OnStairs = false;
                break;
            case "1":
                if (HC.OnStairs == false && HC.OfficeDoor == false)
                {
                    HC.InstaBasement();
                }
                break;
            case "2":
                if (HC.OnStairs == false && HC.OfficeDoor == false)
                {
                    HC.InstaFloor1();
                }
                break;
            case "3":
                if (HC.OnStairs == false && HC.OfficeDoor == false)
                {
                    HC.InstaFloor2();
                }
                break;
        }
    }

    void GotHit()
    {
        SpeedUp = true;
        CHealth -= 20;
        Speed = 10;
        HealthSystem();
        if (CHealth > 0)
        {
            GetHIt.SetActive(true);
            GetHitAni.SetBool("Doit", true);
            StartCoroutine(MinusHealthAnimation());
        }
    }

    IEnumerator MinusHealthAnimation()
    {
        yield return new WaitForSeconds(1f);
        GetHitAni.SetBool("Doit", false);
        GetHIt.SetActive(false);
    }

    public void Heal()
    {
      
    }

    void HealthSystem()
    {
        if (CHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        DeadthUI.SetActive(true);
    }

    public void PressE()
    {
        if (ColectableUI.activeSelf)
        {
            ColectableUI.SetActive(false);
            GI.AddItemList(Item);
            PS.AddItem(Item);

            IH.DestroyThisItem();
        }
        else if (DoorUI.activeSelf)
        {
            if (MD != null)
            {
                if (GI.Items.Count >= 1)
                {
                    foreach (var Item in GI.Items)
                    {
                        if (Item.Item == CollectableScript.Items.MainKey)
                        {
                            MD.Active = true;
                            StartCoroutine(HC.RespawnHantu());
                            MD.RightDoor.SetBool("Open", MD.Active);
                            MD.LeftDoor.SetBool("Open", MD.Active);
                            DoorUI.SetActive(false);
                            HC.DesrtroyThekid();
                            break;
                        }
                        else
                        {
                            MD.Active = false;
                        }
                    }
                }
                if (!MD.Active)
                {
                    ErrorOpenDoor();
                }
            }
            else
            {
                switch (DS.DoorType)
                {
                    case DoorScrpt.Doortype.NormalDoor:
                        switch (DS.Condition)
                        {
                            case true:
                                DS.Condition = false;
                                DS.Door.SetBool("Open", false);
                                DoorText.text = "[E] Open Door";
                                break;
                            case false:
                                DS.Condition = true;
                                DS.Door.SetBool("Open", true);
                                DoorText.text = "[E] Close Door";
                                break;
                        }
                        break;
                    case DoorScrpt.Doortype.OfficeDoor:
                        if (GI.Items.Count >= 1)
                        {
                            foreach (var Item in GI.Items)
                            {
                                if (Item.Item == CollectableScript.Items.OfficeKey)
                                {
                                    DS.Condition = true;
                                    HC.OfficeDoor = true;
                                    DoorUI.SetActive(false);
                                    DS.Door.SetBool("Open", true);
                                    HC.DestroyHantu();
                                    break;
                                }
                                else
                                {
                                    CannotOpen = true;
                                }
                            }
                            if (CannotOpen)
                            {
                                ErrorOpenDoor();
                                CannotOpen = false;
                            }
                        }
                        else
                        {
                            ErrorOpenDoor();
                        }
                        break;
                }
            }
        }
        else if (OS != null && !OS.IsOpen)
        {
            OS.Shelf.SetBool("Open", true);
            OpenShelf.SetActive(false);
            OS.IsOpen = true;
        }
    }

    void ErrorOpenDoor()
    {
        DoorUI.SetActive(false);
        Message.SetActive(true);
        MessageText.text = "I need to find the key";
        StartCoroutine(HoldTheMessage());
    }

    IEnumerator HoldTheMessage()
    {
        yield return new WaitForSeconds(2);
        Message.SetActive(false);
    }

    void ChangeItem()
    {
        if (Scrolly < 0)
        {
            print("Down");
        }
        else if (Scrolly > 0)
        {
            print("UP");
        }
    }

    void Flash()
    {
        if (Time.timeScale == 1)
        {
            if (TouchLightCon)
            {
                touchCurrentState.sprite = TouchOff;
                TouchLight.intensity = 0;
                TouchLightCon = false;
            }
            else
            {
                touchCurrentState.sprite = TouchOn;
                TouchLight.intensity = 300000f;
                TouchLightCon = true;
            }
        }
    }

    #region - Control -

    private void OnEnable()
    {
        PlayerControl.Enable();
    }

    private void OnDisable()
    {
        PlayerControl.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return PlayerControl.Player.Walk.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return PlayerControl.Player.Camera.ReadValue<Vector2>();
    }

    public bool PlayerJumpedThisFrame()
    {
        return PlayerControl.Player.Jump.triggered;
    }

    #endregion
}