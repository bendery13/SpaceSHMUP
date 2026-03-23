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
    static public Transform PROJECTILE_ANCHOR;

    [Header("Dynamic")]
    [SerializeField]
    [Tooltip("Setting this manually while playing does not work properly")]
    private eWeaponType _type = eWeaponType.none;
    public WeaponDefinition def;
    public float nextShotTime;

    private GameObject weaponModel;
    private Transform shotPointTrans;

    void Start()
    {
        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        shotPointTrans = transform.GetChild(0);

        // Call SetType() for the default
        SetType(_type);

        // Find the fireEvent in the parent heirarchy
        Hero hero = GetComponentInParent<Hero>();
        if (hero != null) hero.fireEvent += Fire;
    }

    public eWeaponType type
    {
        get {return(_type);}
        set {SetType(value);}
    }

    public void SetType(eWeaponType wt)
    {
        _type = wt;
        if (type == eWeaponType.none)            {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        // Get the WeaponDefinition for this type from main
        def = Main.GET_WEAPON_DEFINITION(_type);
        // Destroy any old model then attach a model for this weapon
        if (weaponModel != null) Destroy(weaponModel);
        weaponModel = Instantiate<GameObject>(def.weaponModelPrefab, transform);
        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localScale = Vector3.one;

        nextShotTime = 0;
    }

    private void Fire()
    {
        if (!gameObject.activeInHierarchy) return;
        if (Time.time < nextShotTime) return;

        ProjectileHero p;
        Vector3 vel = Vector3.up * def.velocity;

        switch (type)
        {
            case eWeaponType.blaster:
                p = MakeProjectile();
                p.vel = vel;
                break;

            case eWeaponType.spread:
                p = MakeProjectile();
                p.vel = vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.vel = p.transform.rotation*vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.vel = p.transform.rotation*vel;
                break;
        }
    }

    private ProjectileHero MakeProjectile()
    {
        GameObject go;
        go = Instantiate<GameObject>(def.projectilePrefab, PROJECTILE_ANCHOR);
        ProjectileHero p = go.GetComponent<ProjectileHero>();

        Vector3 pos = shotPointTrans.position;
        pos.z = 0;
        p.transform.position = pos;

        p.type = type;
        nextShotTime = Time.time + def.delayBetweenShots;
        return(p);
    }
}
