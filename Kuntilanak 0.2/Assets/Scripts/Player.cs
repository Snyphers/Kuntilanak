using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    PlayerInputAction PlayerControl;
    [SerializeField] PauseSetting PS;
    [SerializeField] GameInspector GI;

    [SerializeField] CollectableScript Item;

    [SerializeField] ItemHolder IH;
    [SerializeField] TextMeshProUGUI ItemName;
    [SerializeField] Image ItemIcon;
    [SerializeField] GameObject ColectableUI;

    [SerializeField] CharacterController Controller;
    [SerializeField] Collider PlayerCol;

    [SerializeField] float Jump;
    [SerializeField] float Speed;
    [SerializeField] int AHealth;
    [SerializeField] int CHealth;
    [SerializeField] Vector3 Velocity;
    float Gravity = -8.81f;

    [SerializeField] Image touchCurrentState;
    [SerializeField] Slider HealthBar;
    [SerializeField] Sprite TouchOff;
    [SerializeField] Sprite TouchOn;

    [SerializeField] GameObject ShowItem;
    [SerializeField] Image CurrentItemShow;
    [SerializeField] TextMeshProUGUI ItemCount;
    [SerializeField] TextMeshProUGUI UIItemsName;

    float Scrolly;

    [SerializeField] Light TouchLight;

    public float MouseSX;
    public float MouseSY;
    float YRotation = 0f;
    [SerializeField] Transform Camera;

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
        PlayerControl.Player.CollectItem.performed += PickUp => CollectItem();

        PlayerControl.Player.ScrollItem.performed += ChangeItem => Scrolly = ChangeItem.ReadValue<float>();
        PlayerControl.Player.ScrollItem.performed += ScrollY => ChangeItem();

        Cursor.visible = false;
    }

    void Update()
    {
        IsGrounded = Controller.isGrounded;
        if (IsGrounded && Velocity.y < 0)
        {
            Velocity.y = 0f;
        }

        float MouseX = GetMouseDelta().x * MouseSX * Time.deltaTime;
        float MouseY = GetMouseDelta().y * MouseSY * Time.deltaTime;

        Vector3 move = transform.right * GetPlayerMovement().x + transform.forward * GetPlayerMovement().y;
        Controller.Move(move * Speed * Time.deltaTime);

        YRotation -= MouseY;
        YRotation = Mathf.Clamp(YRotation, -90f, 90f);

        Camera.localRotation = Quaternion.Euler(YRotation, 0f, 0f);
        transform.Rotate(Vector3.up * MouseX);

        Velocity.y += Gravity * Time.deltaTime;
        Controller.Move(Velocity * Time.deltaTime);
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Collectable":
                ColectableUI.SetActive(false);
                break;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Kuntilanak")
        {
            CHealth -= 10;
            HealthSystem();
        }
        else if (hit.gameObject.tag == "Budak")
        {

        }
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

    }

    public void CollectItem()
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
                TouchLight.intensity = 250000f;
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