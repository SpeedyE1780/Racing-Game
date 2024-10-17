using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    public Transform target;

    private void OnValidate()
    {
        if (target != null)
        {
            transform.position = target.TransformPoint(offset);
            Vector3 lookVector = target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
        }
    }

    void Update()
    {
        transform.position = target.TransformPoint(offset);
        Vector3 lookVector = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
    }
}
