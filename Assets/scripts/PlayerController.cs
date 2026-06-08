using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PhotonView))]
public class PlayerController : MonoBehaviourPun
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float mouseSensitivity = 800f;
    [SerializeField] private Camera playerCamera;

    [Header("Camera Settings")]
    [SerializeField] private bool isThirdPerson = false;
    [SerializeField] private float distanceFromPlayer = 5.0f;
    [SerializeField] private float heightOffset = 1.7f;
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 70f;

    private CharacterController _controller;
    private Vector3 _velocity;
    private float _xRotation = 0f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

        if (photonView.IsMine)
        {
            if (playerCamera != null)
            {
                playerCamera.enabled = true;
                playerCamera.tag = "MainCamera";
            }

            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            if (playerCamera != null)
            {
                playerCamera.enabled = false;
            }
        }
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            float savedValue = PlayerPrefs.GetFloat("GameSensitivity", 800f);
            mouseSensitivity = savedValue;
        }
    }

    private void Update()
    {
        if (!PhotonNetwork.InRoom || !photonView.IsMine) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        _controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
            _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (isThirdPerson)
        {
            transform.Rotate(Vector3.up * mouseX);
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, minVerticalAngle, maxVerticalAngle);
        }
        else
        {
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    public void SetMouseSensitivity(float value)
    {
        if (!photonView.IsMine) return;
        mouseSensitivity = value;
    }
}