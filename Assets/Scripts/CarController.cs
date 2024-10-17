using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    [SerializeField]
    private WheelCollider frontLeft;
    [SerializeField]
    private WheelCollider frontRight;
    [SerializeField]
    private WheelCollider backLeft;
    [SerializeField]
    private WheelCollider backRight;

    [Header("Wheel Transforms")]
    [SerializeField]
    private Transform frontLeftTransform;
    [SerializeField]
    private Transform frontRightTransform;
    [SerializeField]
    private Transform backLeftTransform;
    [SerializeField]
    private Transform backRightTransform;

    [Header("Car Configuration")]
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float wheelSpeed;
    [SerializeField]
    private float brakingForce;
    [SerializeField]
    private float steerAngle;

    [Header("Tail Lights Configuration")]
    [SerializeField]
    private MeshRenderer tailLights;
    [SerializeField]
    private Color tailLightsColor;
    [SerializeField]
    private float intensity;

    private readonly List<WheelCollider> frontWheels = new();
    private readonly List<WheelCollider> backWheels = new();

    private static readonly int EmissionID = Shader.PropertyToID("_EmissionColor");

    private void Start()
    {
        frontWheels.AddRange(new[] { frontLeft, frontRight });
        backWheels.AddRange(new[] { backLeft, backRight });
    }

    private void UpdateTorque(float torque)
    {
        frontWheels.ForEach(wheel => wheel.motorTorque = torque);
        backWheels.ForEach(wheel => wheel.motorTorque = torque);
    }

    private void UpdateSteeringAngle(float angle)
    {
        frontWheels.ForEach(wheel => wheel.steerAngle = angle);
    }

    private void UpdateBrakeTorque(float brakingTorque)
    {
        frontWheels.ForEach(wheel => wheel.brakeTorque = brakingTorque);
        float lightsMultiplier = brakingTorque > 0 ? 1 : 0;
        tailLights.material.SetColor(EmissionID, tailLightsColor * intensity * lightsMultiplier);
    }

    private void ResetCar()
    {
        transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        UpdateTorque(0);
        UpdateSteeringAngle(0);
        UpdateBrakeTorque(0);
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
    {
        wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        wheelTransform.SetPositionAndRotation(position, rotation);
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool isBraking = Input.GetKey(KeyCode.Space);

        UpdateTorque(verticalInput * wheelSpeed);
        UpdateSteeringAngle(steerAngle * horizontalInput);
        UpdateBrakeTorque(isBraking ? brakingForce : 0);

        UpdateWheelTransform(frontLeft, frontLeftTransform);
        UpdateWheelTransform(frontRight, frontRightTransform);
        UpdateWheelTransform(backLeft, backLeftTransform);
        UpdateWheelTransform(backRight, backRightTransform);

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCar();
        }
    }
}
