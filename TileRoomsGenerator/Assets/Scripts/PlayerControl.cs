using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float backwalkSpeed = 0.5f;
    public float diagonalSpeed = 0.725f;

    public float rotateSpeed = 10.0f;
    public float rotateYMin = -60f;
    public float rotateYMax = 60f;

    private Transform cameraTransform;
    private Transform playerTransform;
    private Rigidbody playerRigidbody;
    private Vector3 movementVector;
    private Vector3 forwardVelocity;
    private Vector3 horizontalVelocity;
    private Vector3 outgoingVelocity;
    
    private float outgoingSpeed = 0f;
    private float deltaRotateX = 0f;
    private float deltaRotateY = 0f;

    private void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().GetComponent<Transform>();
        playerTransform = GetComponent<Transform>();
        playerRigidbody = GetComponent<Rigidbody>();
        outgoingSpeed = moveSpeed;
    }

    private void DiagonalWalk()
    {
        if (Input.GetAxisRaw("Vertical") != 0f && Input.GetAxisRaw("Horizontal") != 0f)
            outgoingVelocity *= diagonalSpeed;
    }

    private void BackWalk()
    {
        if (Input.GetAxisRaw("Vertical") < 0f)
            outgoingVelocity *= backwalkSpeed;
    }

    private void Move()
    {
        forwardVelocity = transform.forward * Input.GetAxisRaw("Vertical");
        horizontalVelocity = transform.right * Input.GetAxisRaw("Horizontal");

        outgoingVelocity = forwardVelocity + horizontalVelocity;
        outgoingSpeed = moveSpeed;

        DiagonalWalk();
        BackWalk();

        playerRigidbody.velocity = outgoingVelocity * outgoingSpeed;
    }

    private void Rotate()
    {
        deltaRotateX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        deltaRotateY -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

        if (deltaRotateY < rotateYMin)
            deltaRotateY = rotateYMin;
        else if (deltaRotateY > rotateYMax)
            deltaRotateY = rotateYMax;

        playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(new Vector3(0f, deltaRotateX, 0f)));
        cameraTransform.eulerAngles = new Vector3(deltaRotateY, playerTransform.localEulerAngles.y, playerTransform.localEulerAngles.z);
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        Rotate();
    }
}
