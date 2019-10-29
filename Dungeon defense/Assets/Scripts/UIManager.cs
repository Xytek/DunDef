using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum BuySellBroke { Buy, Sell, Broke };

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _spawnTooltip = default;
    [SerializeField] private Text _playerHealth = default;
    [SerializeField] private Text _playerResources = default;

    private void Start()
    {
        TurnOffSpawnTooltip();
    }
    private void Update()
    {
        _playerHealth.text = "Health: " + GameManager.Instance.playerHealth;
        _playerResources.text = "Coins: $" + GameManager.Instance.playerResources;
    }
    public void ToggleSpawnTooltip(int cost, BuySellBroke bsb)
    {
        _spawnTooltip.gameObject.SetActive(true);
        switch (bsb)
        {
            case BuySellBroke.Buy:
                _spawnTooltip.text = "Place Trap: " + cost;
                StopCoroutine(TimeOut());
                break;
            case BuySellBroke.Sell:
                _spawnTooltip.text = "Sell Trap: " + (int)(cost * 0.75f);
                StopCoroutine(TimeOut());
                break;
            case BuySellBroke.Broke:
                _spawnTooltip.text = "Not enough resources. This trap costs: " + cost;
                StartCoroutine(TimeOut());
                break;
        }
    }

    public void TurnOffSpawnTooltip()
    {
        _spawnTooltip.gameObject.SetActive(false);
    }

    private IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(3f);
        TurnOffSpawnTooltip();
    }
}
