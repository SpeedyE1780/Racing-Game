using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("Car Configuration")]
    [SerializeField] private Rigidbody rb;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float steerAngle = 30f;
    [SerializeField]
    private float brakeTorque;

    [Header("AI Configuration")]
    [SerializeField]
    private List<Transform> checkPoints;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float targetSpeed = 10f;
    [SerializeField]
    private float targetDistanceThreshold = 10f;
    [SerializeField]
    private float checkpointDistanceThreshold = 1f;
    [SerializeField]
    private float timeBetweenCheckpoints = 15f;


    [Header("Wheel Colliders")]
    [SerializeField]
    private List<WheelCollider> frontWheels;
    [SerializeField]
    private List<WheelCollider> backWheels;

    [Header("Wheel Transforms")]
    [SerializeField]
    private List<Transform> frontWheelsTransform;
    [SerializeField]
    private List<Transform> backWheelsTransform;

    [Header("Tail Lights Configuration")]
    [SerializeField] private MeshRenderer tailLights;
    [SerializeField] private Color color;
    [SerializeField] private float intensity;

    private int currentCheckpoint;
    private float currentTime;
    private float brakeFactor;

    private static readonly int EmissionID = Shader.PropertyToID("_EmissionColor");

    private bool IsCarFlipped => Vector3.Dot(transform.up, Vector3.up) < 0;
    private bool ReachedCheckpoint => Vector3.Distance(target.position, checkPoints[currentCheckpoint].position) < checkpointDistanceThreshold;
    private bool UnableToReachCheckpoint => currentTime >= timeBetweenCheckpoints;

    private void PlaceOnFirstCheckPoint()
    {
        Vector3 lookDirection = checkPoints[1].position - checkPoints[0].position;
        transform.SetPositionAndRotation(checkPoints[0].position, Quaternion.LookRotation(lookDirection, Vector3.up));
        target.position = transform.position + transform.forward * 3f;
        currentCheckpoint = 1;
    }

    private void ResetCar()
    {
        Vector3 lookDirection = checkPoints[currentCheckpoint].position - checkPoints[currentCheckpoint - 1].position;
        transform.SetPositionAndRotation(checkPoints[currentCheckpoint - 1].position, Quaternion.LookRotation(lookDirection, Vector3.up));
        target.position = transform.position + transform.forward * 3f;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void UpdateWheelForces()
    {
        float currentAngle = Mathf.Clamp(Vector3.SignedAngle(transform.forward, (target.position - transform.position).normalized, Vector3.up), -steerAngle, steerAngle);

        frontWheels.ForEach(wheel =>
        {
            wheel.motorTorque = speed;
            wheel.steerAngle = currentAngle;
            wheel.brakeTorque = rb.velocity.magnitude > 8 ? brakeTorque * brakeFactor : 0;
        });
    }

    private void UpdateWheelTransform(List<WheelCollider> wheelColliders, List<Transform> wheelTransforms)
    {
        for (int i = 0; i < wheelColliders.Count; i++)
        {
            WheelCollider wheel = wheelColliders[i];
            Transform wheelMesh = wheelTransforms[i];

            wheel.GetWorldPose(out Vector3 pos, out Quaternion quat);
            wheelMesh.SetPositionAndRotation(pos, quat);
        }
    }

    private void UpdateCheckpoint()
    {
        currentCheckpoint = (currentCheckpoint + 1) % checkPoints.Count;
        currentTime = 0;
    }

    private void Start()
    {
        PlaceOnFirstCheckPoint();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < targetDistanceThreshold)
        {
            target.position = Vector3.MoveTowards(target.position, checkPoints[currentCheckpoint].position, targetSpeed * Time.deltaTime);
        }

        if (IsCarFlipped)
        {
            ResetCar();
        }

        UpdateWheelForces();
        UpdateWheelTransform(frontWheels, frontWheelsTransform);
        UpdateWheelTransform(backWheels, backWheelsTransform);

        if (ReachedCheckpoint)
        {
            UpdateCheckpoint();
        }

        if (UnableToReachCheckpoint)
        {
            Debug.Log("Reset AI Car");
            ResetCar();
            currentTime = 0;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            brakeFactor = other.GetComponent<CheckpointController>().BrakeFactor;
            tailLights.material.SetColor(EmissionID, color * intensity * brakeFactor);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.position, 0.5f);
        Gizmos.DrawLine(transform.position, target.position);
    }
}
