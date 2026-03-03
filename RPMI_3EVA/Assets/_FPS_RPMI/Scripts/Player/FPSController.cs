using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class FPSController : MonoBehaviour
{
    #region General Variables
    [Header("Movement & Look")]
    [SerializeField] GameObject camHolder;//Ref al obj q tiene como hijo la cam
    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float crouchSpeed = 3f;
    [SerializeField] float maxForce = 1f;
    [SerializeField] float sensitivity = 0.1f;


    [Header("Jump & GrondCheck")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool isGrounded;

    [Header("Player State Bools")]
    [SerializeField] bool isSprinting;
    [SerializeField] bool isCrouching;
    #endregion



    // Variables de referencia privadas 
    Rigidbody rb; //Ref al rb del player
    Animator anim; 

    //Variables para el input
    Vector2 moveInput;
    Vector2 lookInput;
    float lookRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();    
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Lock del cursor raton
        Cursor.lockState = CursorLockMode.Locked; //Mueve el cursor al centro 
        Cursor.visible = false; //Ocultael cursor de la vista
    }

    // Update is called once per frame
    void Update()
    {
        //Groundcheck
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.DrawRay(camHolder.transform.position, camHolder.transform.forward * 100f, Color.red);
    }


    private void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    void CameraLook()
    {
        transform.Rotate(Vector3.up * lookInput.x * sensitivity);
        //Rotacion vertical 
        lookRotation += (-lookInput.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.localEulerAngles = new Vector3(lookRotation, 0f, 0f);
    }

    void Movement()
    {
        Vector3 currentVelocity = rb.linearVelocity; //necesita calcuñar la vel acual del rb constantemente
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y);//vel a alcanzar = direccion que miramos
        targetVelocity *= isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : speed;
        // convertir la direccion local a global
        targetVelocity = transform.TransformDirection(targetVelocity);

        //calcular cambio velocidad (aceleracion)
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0f, velocityChange.z);
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);

        //Aplicar la fuerza de mov/aceleracion
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
    

    void Jump()
    {
        if (isGrounded) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }




    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
       if(context.performed) Jump();
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
       if (context.performed)
        {
            isCrouching = !isCrouching;
            anim.SetBool("isCrouching", isCrouching);
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed && !isCrouching) isSprinting = true;
        if (context.canceled) isSprinting = false;
        
            
        
    }


    #endregion
}

