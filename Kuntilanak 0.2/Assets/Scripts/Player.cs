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
    [SerializeField] float CHealth;
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

        Cursor.visible = false;
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
            Pause = false;
            Time.timeScale = 1f;
            Cursor.visible = false;
            PS.PauseMenu.SetActive(false);
        }
        else
        {
            Pause = true;
            Time.timeScale = 0f;
            Cursor.visible = true;
            PS.PauseMenu.SetActive(true);
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
                        break;
                    case DoorScrpt.Doortype.OfficeDoor:
                        switch (DS.Condition)
                        {
                            case false:
                                DoorText.text = "[E] Open Door";
                                break;
                        }
                        break;
                }
                DoorUI.SetActive(true);
                break;
            case "Main Door":
                MD = other.GetComponent<MainDoor>();
                if (!MD.Active)
                {
                    DoorText.text = "[E] Open Door";
                }
                DoorUI.SetActive(true);
                break;
            case "Shelf":
                OS = other.GetComponent<OpenShelf>();
                if (!OS.IsOpen)
                {
                    
                }
                break;
            case "Budak":
                Budak = true;
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
            case "Budak":
                Budak = false;
                break;

        }
    }

    void GotHit()
    {
        SpeedUp = true;
        CHealth -= 20;
        Speed = 10;
        HealthSystem();
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
            IH.DestroyThisItem();
            GI.AddItemList(Item);
            PS.AddItem(IH);
            ShowItems();
            //RefreshItem();
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
                            MD.RightDoor.SetBool("Open", MD.Active);
                            MD.LeftDoor.SetBool("Open", MD.Active);
                            DoorUI.SetActive(false);
                        }
                        else
                        {
                            ErrorOpenDoor();
                        }
                    }
                }
                else
                {
                    ErrorOpenDoor();
                }
            }
            //else if ()
            //{

            //}
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
                                    DoorUI.SetActive(false);
                                    DS.Door.SetBool("Open", true);
                                    break;
                                }
                                else
                                {
                                    ErrorOpenDoor();
                                    break;
                                }
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

    public void ShowItems()
    {
        if (GI.Items.Count == 1)
        {
            ShowItem.SetActive(true);
            UIItemsName.text = Item.Name;
            CurrentItemShow.sprite = Item.Icon;
            ItemCount.text = Item.Count.ToString();
        }
    }

    public void RefreshItem()
    {
        if (GI.Items.Count > 0)
        {

        }
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