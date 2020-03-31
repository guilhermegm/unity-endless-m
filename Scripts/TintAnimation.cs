using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TintAnimation : MonoBehaviour
{
    public Color tintColor;
    public float tintFadeSpeed;
    public SpriteRenderer[] gfxs;

    private List<Material> materials;
    private Color currentTintColor;

    private void Awake()
    {
        materials = new List<Material>();
        currentTintColor = tintColor;

        foreach (SpriteRenderer spriteRenderer in gfxs)
        {
            materials.Add(spriteRenderer.material);
        }
    }

    public void Begin()
    {
        currentTintColor = tintColor;
    }

    private void Update()
    {
        if (currentTintColor.a > 0)
        {
            currentTintColor.a = Mathf.Clamp01(currentTintColor.a - tintFadeSpeed * Time.deltaTime);
            UpdateMaterialsColor();
        }
    }

    private void UpdateMaterialsColor()
    {
        foreach (Material material in materials)
        {
            material.SetColor("_Tint", currentTintColor);
        }
    }
}
