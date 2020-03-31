using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Gold : MonoBehaviour
{
    public TextMeshProUGUI text;
    private Player player;

    public void Init(Player player)
    {
        this.player = player;

        this.player.inventory.OnGoldChange += Inventory_OnGoldChange;
        UpdateVisual(this.player.inventory.GetGold());
    }

    private void Inventory_OnGoldChange(object sender, Inventory.OnGoldChangeEventArgs e)
    {
        UpdateVisual(e.golds);
    }

    private void UpdateVisual(int golds)
    {
        text.SetText(golds.ToString());
    }
}
