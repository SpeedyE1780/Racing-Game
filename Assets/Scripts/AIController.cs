using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public List<Transform> checkPoints;
    private int currentCheckpoint;
    public float speed = 1f;
    public float targetSpeed = 10f;
    public float steerAngle = 30f;
    public float brakeTorque;
    public Transform target;

    public List<WheelCollider> frontWheels;
    public List<WheelCollider> backWheels;

    public List<Transform> frontWheelsMesh;
    public List<Transform> backWheelsMesh;
    public float timeBetweenCheckpoints = 15f;
    private float currentTime;
    [SerializeField] private float brakeFactor;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private MeshRenderer tailLights;
    [SerializeField] private Color color;
    [SerializeField] private float intensity;
    [SerializeField] private float magnitude;

    void Start()
    {
        Vector3 lookDirection = checkPoints[1].position - checkPoints[0].position;
        transform.SetPositionAndRotation(checkPoints[0].position, Quaternion.LookRotation(lookDirection, Vector3.up));
        target.position = transform.position + transform.forward * 3f;
        currentCheckpoint = 1;
    }

    void Update()
    {
        magnitude = rb.velocity.magnitude;
        currentTime += Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 10f)
        {
            target.position = Vector3.MoveTowards(target.position, checkPoints[currentCheckpoint].position, targetSpeed * Time.deltaTime);
        }

        if (Vector3.Dot(transform.up, Vector3.up) < 0)
        {
            Vector3 lookDirection = checkPoints[currentCheckpoint].position - checkPoints[currentCheckpoint - 1].position;
            transform.SetPositionAndRotation(checkPoints[currentCheckpoint - 1].position, Quaternion.LookRotation(lookDirection, Vector3.up));
            target.position = transform.position + transform.forward * 3f;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        float currentAngle = Mathf.Clamp(Vector3.SignedAngle(transform.forward, (target.position - transform.position).normalized, Vector3.up), -steerAngle, steerAngle);

        frontWheels.ForEach(wheel =>
        {
            wheel.motorTorque = speed;
            wheel.steerAngle = currentAngle;
            wheel.brakeTorque = rb.velocity.magnitude > 8 ? brakeTorque * brakeFactor : 0;
        });

        for (int i = 0; i < frontWheels.Count; i++)
        {
            WheelCollider wheel = frontWheels[i];
            Transform wheelMesh = frontWheelsMesh[i];

            wheel.GetWorldPose(out Vector3 pos, out Quaternion quat);
            wheelMesh.SetPositionAndRotation(pos, quat);
        }

        for (int i = 0; i < backWheels.Count; i++)
        {
            WheelCollider wheel = backWheels[i];
            Transform wheelMesh = backWheelsMesh[i];

            wheel.GetWorldPose(out Vector3 pos, out Quaternion quat);
            wheelMesh.SetPositionAndRotation(pos, quat);
        }

        if (Vector3.Distance(target.position, checkPoints[currentCheckpoint].position) < 1f)
        {
            currentCheckpoint += 1;
            currentCheckpoint %= checkPoints.Count;
            currentTime = 0;
        }

        if(currentTime >= timeBetweenCheckpoints)
        {
            Debug.Log("Reset");
            Vector3 lookDirection = checkPoints[currentCheckpoint].position - checkPoints[currentCheckpoint - 1].position;
            transform.SetPositionAndRotation(checkPoints[currentCheckpoint - 1].position, Quaternion.LookRotation(lookDirection, Vector3.up));
            target.position = transform.position + transform.forward * 3f;
            currentTime = 0;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            brakeFactor = other.GetComponent<CheckpointController>().BrakeFactor;
            tailLights.material.SetColor("_EmissionColor", color * intensity * brakeFactor);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.position, 0.5f);
        Gizmos.DrawLine(transform.position, target.position);
    }
}
