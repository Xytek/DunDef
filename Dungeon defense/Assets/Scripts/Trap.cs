using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapType {
    Wall,
    Floor,
    Both,
    Weapon
}

[CreateAssetMenu(fileName ="NewTrap",menuName ="TrapData")]
public class Trap : ScriptableObject
{
    public GameObject TrapObject;
    public GameObject PreviewObject;
    public TrapType Type;
    public bool Unlocked;
    public int Price;
    public int Damage;
    public string Tooltip;

}
