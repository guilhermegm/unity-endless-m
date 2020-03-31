using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Stats : MonoBehaviour
{
    public TextMeshProUGUI textLeft;
    public TextMeshProUGUI textRight;

    public void Init(string textLeft, string textRight)
    {
        this.textLeft.SetText(textLeft);
        this.textRight.SetText(textRight);
    }
}
