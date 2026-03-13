using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

/// <summary>
/// This is an enum of the various weapon types.
/// It also includes a "shield" type to allow a shield PowerUP
/// Items marked [NI] below are NOt Implemented in this book
/// </summary>

public enum eWeaponType
{
    none,       // The default / no weapon
    blaster,    // a simple blaster
    spread,     // two shots simultaneously
    phaser,     // [NI] shots that move in waves
    missile,    // [NI] homing missiles
    laser,      // [NI] damage over time
    shield      // raise shieldLevel
}
/// <summary>
/// The WeaponDefinintion class allows you to specify the properties 
/// of a specific weapon type in the Inspector. The Main class has an 
/// array of WeaponDefinitions that makes this possible
/// </summary>

[System.Serializable]
public class WeaponDefinition
{
    public eWeaponType type = eWeaponType.none;
    [Tooltip("Letter to show on the PowerUp Cube")]
    public string letter; // The letter to display on the PowerUp
    [Tooltip("The color of the PowerUp Cube")]
    public Color color = Color.white;
    [Tooltip("Prefab of the Weapon Model that is attached to the Player Ship")]
    public GameObject weaponModelPrefab;
    [Tooltip("Prefab of the Projectile fired by this Weapon")]
    public GameObject projectilePrefab;
    [Tooltip("Color of the Projectile")]
    public Color projectileColor = Color.white;
    [Tooltip("The damage amount of the projectiles")]
    public float damageOnHit = 0;
    [Tooltip("Damage caused per second by the Laser [NI]")]
    public float damagePerSecond = 0;
    [Tooltip("Delay between shots in seconds")]
    public float delayBetweenShots = 0;
    [Tooltip("Velocity of the Projectile")]
    public float velocity = 50;

}
public class Weapon : MonoBehaviour
{
    
}
