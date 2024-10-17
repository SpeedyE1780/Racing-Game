using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider backLeft;
    public WheelCollider backRight;

    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform backLeftTransform;
    public Transform backRightTransform;

    public float wheelSpeed;
    public float brakingForce;
    public float steerAngle;

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool isBraking = Input.GetKey(KeyCode.Space);

        frontLeft.motorTorque = verticalInput * wheelSpeed;
        frontRight.motorTorque = verticalInput * wheelSpeed;
        backLeft.motorTorque = verticalInput * wheelSpeed;
        backRight.motorTorque = verticalInput * wheelSpeed;

        frontLeft.steerAngle = steerAngle * horizontalInput;
        frontRight.steerAngle = steerAngle * horizontalInput;

        if(isBraking)
        {
            frontLeft.brakeTorque = brakingForce;
            frontRight.brakeTorque = brakingForce;
        }
        else
        {
            frontLeft.brakeTorque = 0;
            frontRight.brakeTorque = 0;
        }

        Vector3 flPos;
        Quaternion flRot;
        frontLeft.GetWorldPose(out flPos, out flRot);
        frontLeftTransform.SetPositionAndRotation(flPos, flRot);

        Vector3 frPos;
        Quaternion frRot;
        frontRight.GetWorldPose(out frPos, out frRot);
        frontRightTransform.SetPositionAndRotation(frPos, frRot);

        Vector3 blPos;
        Quaternion blRot;
        backLeft.GetWorldPose(out blPos, out blRot);
        backLeftTransform.SetPositionAndRotation(blPos, blRot);

        Vector3 brPos;
        Quaternion brRot;
        backRight.GetWorldPose(out brPos, out brRot);
        backRightTransform.SetPositionAndRotation(brPos, brRot);
    }
}
