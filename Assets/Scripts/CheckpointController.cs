using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private float brakeFactor;
    public float BrakeFactor => brakeFactor;
    public Vector3 Position => transform.position;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Position + Vector3.up * 0.25f, 0.25f);

        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 20f))
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(hit.point + Vector3.up * 0.25f, 0.25f);
            Vector3 groundedPosition = transform.position;
            groundedPosition.y = hit.point.y;
            transform.position = groundedPosition;
        }
    }
}
