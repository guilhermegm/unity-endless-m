using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraFollow : MonoBehaviour
{
    public float smoothSpeed;
    public Vector3 offset;
    public Player Target { get; set; }
    public Transform walkableArea;

    public Vector2 maxPosition;
    public Vector2 minPosition;

    public void Init(Player target)
    {
        this.Target = target;
    }

    private void FixedUpdate()
    {
        if (!Target)
            return;

        Vector3 desiredPosition = Target.transform.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        smoothPosition.x = Mathf.Clamp(smoothPosition.x, minPosition.x, maxPosition.x);
        smoothPosition.y = Mathf.Clamp(smoothPosition.y, minPosition.y, maxPosition.y);

        transform.position = smoothPosition;
    }
}
