using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public float moveYSpeed;
    public float disappearTimer;

    private void Update()
    {
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;

        if (disappearTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
