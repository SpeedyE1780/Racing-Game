using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Transform target;

    private void OnValidate()
    {
        if (target != null)
        {
            transform.position = target.TransformPoint(offset);
            Vector3 lookVector = target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
        }
    }

    private void Update()
    {
        transform.position = target.TransformPoint(offset);
        Vector3 lookVector = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
    }
}
