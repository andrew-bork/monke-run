using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class SmoothLook : MonoBehaviour
{

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationX = 0F;
    float rotationY = 0F;
    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;
    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0F;
    public float frameCounter = 20;
    Quaternion originalRotation;
    public Transform player;
    public Rigidbody playerRB;

    public PlayerInput playerInput;
    Vector3 move;
    void Update()
    {

        playerRB.AddForce(move * 10, ForceMode.Acceleration);
        //var mouse = Mouse.current;
        //transform.localEulerAngles = new Vector3(ClampAngle(transform.localEulerAngles.x - mouse.delta.y.value * sensitivityY, minimumY, maximumY), transform.localEulerAngles.y, transform.localEulerAngles.z);

        //player.Rotate(player.up, mouse.delta.x.value * sensitivityY);
        //mouse.delta;
    }
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
            rb.freezeRotation = true;
        originalRotation = transform.localRotation;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if (angle < -180F)
        {
            angle += 360F;
        }
        if (angle > 180F)
        {
            angle -= 360F;
        }
        return Mathf.Clamp(angle, min, max);
    }

    void OnGUI()
    {
        //Press this button to lock the Cursor
        if (GUI.Button(new Rect(0, 0, 100, 50), "Lock Cursor"))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //Press this button to confine the Cursor within the screen
        if (GUI.Button(new Rect(125, 0, 100, 50), "Confine Cursor"))
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();
        //var mouse = Mouse.current;
        transform.localEulerAngles = new Vector3(ClampAngle(transform.localEulerAngles.x - delta.y * sensitivityY, minimumY, maximumY), transform.localEulerAngles.y, transform.localEulerAngles.z);

        player.Rotate(player.up, delta.x * sensitivityY);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        move = value.x * player.transform.right + value.y * player.transform.forward;
        //playerRB.AddForce(value.x * player.transform.forward + value.y * player.transform.right, ForceMode.Acceleration);
    }



    public void OnJump(InputAction.CallbackContext context)
    {
        playerRB.AddForce(transform.up * 10, ForceMode.Acceleration);
    }
}