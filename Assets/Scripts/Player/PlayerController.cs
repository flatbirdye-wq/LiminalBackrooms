using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Look")]
    public Transform cameraPivot;
    public float mouseSensitivity = 1.8f;
    public float maxVerticalAngle = 85f;

    [Header("Movement")]
    public float walkSpeed = 3.5f;
    public float sprintSpeed = 6.5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.0f;

    [Header("Systems")]
    public StaminaSystem stamina;
    public SanitySystem sanity;
    public Inventory inventory;

    private CharacterController cc;
    private float verticalSpeed = 0f;
    private Vector2 rotation = Vector2.zero;
    private bool isSprinting = false;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
        HandleSystems();
    }

    private void HandleLook()
    {
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = -Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotation.x += mx;
        rotation.y += my;
        rotation.y = Mathf.Clamp(rotation.y, -maxVerticalAngle, maxVerticalAngle);

        transform.localRotation = Quaternion.Euler(0, rotation.x, 0);
        if (cameraPivot)
            cameraPivot.localRotation = Quaternion.Euler(rotation.y, 0, 0);
    }

    private void HandleMovement()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 move = transform.TransformDirection(input.normalized);

        isSprinting = Input.GetKey(KeyCode.LeftShift) && input.magnitude > 0.1f && stamina.Current > 0;
        float speed = isSprinting ? sprintSpeed : walkSpeed;
        if (isSprinting) stamina.Drain(Time.deltaTime * 10f);
        else stamina.Recover(Time.deltaTime * 6f);

        if (cc.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        verticalSpeed += gravity * Time.deltaTime;
        move.y = verticalSpeed;
        cc.Move(move * speed * Time.deltaTime);
    }

    private void HandleSystems()
    {
        float ambientLight = RenderSettings.ambientIntensity;
        if (ambientLight < 0.2f)
        {
            sanity.ChangeSanity(-Time.deltaTime * 0.6f, "Darkness");
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, 8f);
        foreach (var c in hits)
        {
            EntityBase e = c.GetComponentInParent<EntityBase>();
            if (e != null && e.IsHostile)
            {
                float d = Vector3.Distance(transform.position, e.transform.position);
                float drain = Mathf.Clamp01(1f - (d / 8f));
                sanity.ChangeSanity(-Time.deltaTime * (0.8f * drain), "EntityProximity");
            }
        }
    }
}
