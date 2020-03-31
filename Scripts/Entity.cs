using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class Entity : MonoBehaviour
{
    [HideInInspector] public string state;

    protected abstract string UpdateClient();

    public void Update()
    {
        state = UpdateClient();
    }
}
