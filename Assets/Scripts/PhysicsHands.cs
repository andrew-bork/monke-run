using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHands : MonoBehaviour
{
    [Header("PID")]
    [SerializeField] Rigidbody playerRigidBody;
    [SerializeField] public float frequency = 50f;
    [SerializeField] public float damping = 1f;
    [SerializeField] public Transform target;

    [SerializeField] public float rotFrequency = 100f;
    [SerializeField] public float rotDamping = 0.9f;

    [Header("Climb")]
    [SerializeField] public float climbForce = 1000f;
    [SerializeField] public float climbDrag = 500f;

    Rigidbody _rigidbody;
    Vector3 _previousPosition;
    bool _isColliding = false;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.maxAngularVelocity = float.PositiveInfinity;
        _previousPosition = transform.position;
        transform.position = target.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PIDMovement();
        PIDRotation();
        if (_isColliding) HookesLaw();
    }

    void PIDMovement()
    {
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Vector3 force = (target.position - transform.position) * ksg + (playerRigidBody.velocity - _rigidbody.velocity) * kdg;
        _rigidbody.AddForce(force, ForceMode.Acceleration);
    }

    void PIDRotation()
    {
        float kp = (6f * rotFrequency) * (6f * rotFrequency) * 0.25f;
        float kd = 4.5f * rotFrequency * rotDamping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Quaternion q = target.rotation * Quaternion.Inverse(transform.rotation);
        if(q.w < 0)
        {
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            q.w = -q.w;
        }
        q.ToAngleAxis(out float angle, out Vector3 axis);
        axis.Normalize();
        axis *= Mathf.Deg2Rad;
        Vector3 torque = ksg * axis * angle + -_rigidbody.angularVelocity * kdg;
        _rigidbody.AddTorque(torque, ForceMode.Acceleration);

    }

    void HookesLaw()
    {
        Vector3 displacement = transform.position - target.position;
        Vector3 force = displacement * climbForce;
        float drag = GetDrag();

        playerRigidBody.AddForce(force, ForceMode.Acceleration);
        playerRigidBody.AddForce(drag * -playerRigidBody.velocity * climbDrag, ForceMode.Acceleration);
    }



    float GetDrag()
    {
        Vector3 handVelocity = (target.localPosition - _previousPosition) / Time.fixedDeltaTime;
        float drag = 1 / (handVelocity.magnitude + 0.01f);
        return (drag < 0.03f ? 0.03f : (1.0f < drag ? 1.0f : drag));

        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != 9)
        {
            _isColliding = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        _isColliding = false;
    }
}
