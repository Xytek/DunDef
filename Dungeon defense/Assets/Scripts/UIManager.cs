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
    [SerializeField] private Text _mobCount = default;
    [SerializeField] private Text _gameOver = default;
    [SerializeField] private Image _crosshair = default;

    private void Start()
    {
        TurnOffSpawnTooltip();
    }
    private void Update()
    {
        _playerHealth.text = "Health: " + GameManager.Instance.playerHealth;
        _playerResources.text = "Coins: $" + GameManager.Instance.playerResources;
        LookingAtEnemy();
    }
    public void ToggleSpawnTooltip(int cost, BuySellBroke bsb)
    {
        _spawnTooltip.gameObject.SetActive(true);
        switch (bsb)
        {
            case BuySellBroke.Buy:
                _spawnTooltip.text = "Place Trap: $" + cost + "\nPress LMB";
                StopCoroutine(TimeOut());
                break;
            case BuySellBroke.Sell:
                _spawnTooltip.text = "Sell Trap: $" + (int)(cost * 0.75f) + "\nPress LMB";
                StopCoroutine(TimeOut());
                break;
            case BuySellBroke.Broke:
                _spawnTooltip.text = "Not enough resources. This trap costs: " + cost;
                StartCoroutine(TimeOut());
                break;
        }
    }

    private void LookingAtEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.gameObject.tag == "Enemy")
                _crosshair.color = new Color32(255, 0, 0, 100);
            else
                _crosshair.color = new Color32(0, 0, 0, 50);
        }
    }

    public void GameOver(int maxHealth, int curHealth, int stars)
    {
        _gameOver.text = "Good job! You beat the level with " + curHealth + "/" + maxHealth + " health. This earned you a " + stars + " star rating. Press any key to continue";

    }
    public void GameOver()
    {
        _gameOver.text = "Fuck, you suck. Press any key to continue";
    }

    public void TurnOffSpawnTooltip()
    {
        _spawnTooltip.gameObject.SetActive(false);
    }
    public void UpdateMobCount(int waveSize, int killCount)
    {
        _mobCount.text = "Wave: " + (waveSize - killCount) + "/" + waveSize;
    }

    private IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(3f);
        TurnOffSpawnTooltip();
    }
}
