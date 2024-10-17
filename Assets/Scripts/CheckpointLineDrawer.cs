using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointLineDrawer : MonoBehaviour
{
    [SerializeField] Gradient brakeGradient;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            CheckpointController checkpoint = transform.GetChild(i).GetComponent<CheckpointController>();
            CheckpointController nextCheckpoint = transform.GetChild((i + 1) % transform.childCount).GetComponent<CheckpointController>();
            Color lineColor = brakeGradient.Evaluate((checkpoint.BrakeFactor + nextCheckpoint.BrakeFactor) * 0.5f);
            Gizmos.color = lineColor;
            Gizmos.DrawLine(checkpoint.Position, nextCheckpoint.Position);
        }
    }
}
