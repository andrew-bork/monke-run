using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{   

    public Rigidbody body;

    private void FixedUpdate()
    {
        Vector3 axis = new Vector3(0.0f, 0.0f, 1.0f);

        axis *= (body.transform.localEulerAngles.z - 30);

        body.AddTorque(axis, ForceMode.Force);
    }
}
