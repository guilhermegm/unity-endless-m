using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRendererSorter : MonoBehaviour
{
    public new Renderer renderer;
    [SerializeField] private int sortingOrderBase = 5000; // This number should be higher than what any of your sprites will be on the position.y
    [SerializeField] private int offset = 0;

    private float timer;
    private float timerMax = .1f;

    private void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = timerMax;
            renderer.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
        }
    }

    public void SetOffset(int offset)
    {
        this.offset = offset;
    }
}
