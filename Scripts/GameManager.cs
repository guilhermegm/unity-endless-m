using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    [SerializeField] private Shop shop;
    public WaveManager waveManager;
    public Item[] items;

    [SerializeField] private UI_CharacterEquipment uiCharacterEquipment;
    [SerializeField] private UI_HealthBar uiHealthBar;
    [SerializeField] private MyCameraFollow uiPlayerCamera;
    [SerializeField] private UI_Gold uiGold;
    [SerializeField] private UI_Shop uiShop;
    [SerializeField] private GameObject uiStartPanel;
    [SerializeField] private GameObject uiWaveDone;
    [SerializeField] private TextMeshProUGUI uiWaveDoneGoldText;
    [SerializeField] private UI_SelectWave uiSelectWave;
    [SerializeField] private Transform walkableArea;

    [SerializeField] private Joystick movementJoystick;
    [SerializeField] private Joystick aimJoystick;

    public UI_WaveTimer uiWaveTimer;
    [SerializeField] private GameObject uiShopButton;
    public UI_Reloading uiReloading;

    private int lastGold;

    private void Awake()
    {
        instance = this;

        Player playerGO = Instantiate(player, Vector3.zero, Quaternion.identity);
        player = playerGO;

        player.Init(uiHealthBar, movementJoystick, aimJoystick, walkableArea, instance);

        uiCharacterEquipment.Init(player.GetComponent<CharacterEquipment>());
        uiPlayerCamera.Init(player);
        uiGold.Init(player);
        uiShop.Init(shop, player, uiCharacterEquipment);

        shop.Init(player, uiShop);
        waveManager.Init(this);

        uiSelectWave.Init(instance, waveManager);

        StartGame();
    }

    public Item GetItem(string name)
    {
        foreach (Item item in items)
        {
            if (item.gameObject.name == name)
                return item;
        }

        return null;
    }

    public void StartGame()
    {
        movementJoystick.gameObject.SetActive(true);
        aimJoystick.gameObject.SetActive(true);
        uiReloading.gameObject.SetActive(true);
        uiHealthBar.gameObject.SetActive(true);
        uiGold.gameObject.SetActive(true);
        uiCharacterEquipment.gameObject.SetActive(true);
        uiStartPanel.SetActive(true);
        uiWaveTimer.gameObject.SetActive(true);
        uiStartPanel.SetActive(false);
        // uiShopButton.SetActive(false);
        uiWaveDone.SetActive(false);
        uiSelectWave.gameObject.SetActive(false);

        lastGold = player.inventory.GetGold();

        player.Test();
        waveManager.StartWave();
    }
    
    public void WaveDone()
    {
        movementJoystick.Reset();
        aimJoystick.Reset();

        int goldEarned = player.inventory.GetGold() - lastGold;
        lastGold = player.inventory.GetGold();

        uiWaveDoneGoldText.SetText("$" + goldEarned.ToString("0"));

        uiWaveDone.SetActive(true);
        movementJoystick.gameObject.SetActive(false);
        aimJoystick.gameObject.SetActive(false);

        Invoke("HideUiWaveDone", 2f);
        Invoke("ShowUiSelectWave", 2f);
    }

    public void ShowUiSelectWave()
    {
        // uiShopButton.SetActive(true);
        uiSelectWave.gameObject.SetActive(true);
    }

    public void ShowUiShop()
    {
        uiSelectWave.gameObject.SetActive(false);
        // uiShopButton.SetActive(false);
        uiShop.gameObject.SetActive(true);
    }

    public void CloseUiShop()
    {
        uiSelectWave.gameObject.SetActive(true);
        // uiShopButton.SetActive(true);
        uiShop.gameObject.SetActive(false);
    }

    private void HideUiWaveDone()
    {
        uiWaveDone.SetActive(false);
    }

    public static int LayerMaskToLayer(LayerMask layerMask)
    {
        int layerNumber = 0;
        int layer = layerMask.value;
        while (layer > 0)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber - 1;
    }
}
