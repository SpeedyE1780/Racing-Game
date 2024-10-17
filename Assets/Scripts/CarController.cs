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

    void Update()
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

        if (isBraking)
        {
            frontLeft.brakeTorque = brakingForce;
            frontRight.brakeTorque = brakingForce;
        }
        else
        {
            frontLeft.brakeTorque = 0;
            frontRight.brakeTorque = 0;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
            frontLeft.motorTorque = verticalInput * wheelSpeed;
            frontRight.motorTorque = verticalInput * wheelSpeed;
            backLeft.motorTorque = verticalInput * wheelSpeed;
            backRight.motorTorque = verticalInput * wheelSpeed;

            frontLeft.steerAngle = steerAngle * horizontalInput;
            frontRight.steerAngle = steerAngle * horizontalInput;

            frontLeft.brakeTorque = 0;
            frontRight.brakeTorque = 0;
        }

        frontLeft.GetWorldPose(out Vector3 flPos, out Quaternion flRot);
        frontLeftTransform.SetPositionAndRotation(flPos, flRot);

        frontRight.GetWorldPose(out Vector3 frPos, out Quaternion frRot);
        frontRightTransform.SetPositionAndRotation(frPos, frRot);

        backLeft.GetWorldPose(out Vector3 blPos, out Quaternion blRot);
        backLeftTransform.SetPositionAndRotation(blPos, blRot);

        backRight.GetWorldPose(out Vector3 brPos, out Quaternion brRot);
        backRightTransform.SetPositionAndRotation(brPos, brRot);
    }
}
