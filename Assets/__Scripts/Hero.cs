using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S {get; private set;} // Singleton property

    [Header("Inscribed")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weapons;

    [Header("Dynamic")] [Range(0,4)] [SerializeField]
    private float _shieldLevel = 1;
    [Tooltip("This field holds a reference to the last triggering GameObject")]
    private GameObject lastTriggerGo = null;

    // Declare a new delegate type
    public delegate void WeaponFireDelegate();

    // Create a weaponFireDelegate event named fireEvent
    public event WeaponFireDelegate fireEvent;

    void Awake()
    {
        if (S == null)
        {
            S = this; // Set the singleton
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
        //fireEvent += TempFire;

        // Reset the weapons to start _Hero with 1 blaster
        ClearWeapons();
        weapons[0].SetType(eWeaponType.blaster);
    }

    // Update is called once per frame
    void Update()
    {
        // Pull in information from the input class
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        // Change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;
        pos.y += vAxis * speed * Time.deltaTime;
        transform.position = pos;

        // Rotate the ship to make it feel more dynamic
        transform.rotation = Quaternion.Euler(vAxis * pitchMult, hAxis * rollMult, 0);

        // Allow the ship to fire
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TempFire();
        //}

        // Use the fireEvent to fire weapons when spacebar is pressed
        if (Input.GetAxis("Jump") == 1 && fireEvent != null)
        {
            fireEvent();
        }
    }

    //void TempFire()
    //{
    //    GameObject projGo = Instantiate<GameObject>(projectilePrefab);
    //    projGo.transform.position = transform.position;
    //    Rigidbody rigidB = projGo.GetComponent<Rigidbody>();
        // rigidB.velocity = Vector3.up * projectileSpeed;

    //    ProjectileHero proj = projGo.GetComponent<ProjectileHero>();
    //    proj.type = eWeaponType.blaster;
    //    float tSpeed = Main.GET_WEAPON_DEFINITION(proj.type).velocity;
    //    rigidB.velocity = Vector3.up * tSpeed;
    //}

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        // Debug.Log("Shield trigger hit by: " + go.gameObject.name);

        // Make sure it's not the same triggering go as last time
        if (go == lastTriggerGo) return;
        lastTriggerGo = go;

        Enemy enemy = go.GetComponent<Enemy>();
        PowerUp pUp = go.GetComponent<PowerUp>();
        if (enemy != null)
        {
            shieldLevel--;
            Destroy(go);
        }
        else if (pUp != null)
        {
            AbsorbPowerUp(pUp);
        }
        else
        {
            // If the shield was triggered by something other than an Enemy
            Debug.LogWarning("Shield trigger hit by non-Enemy: " + go.name);
        }
    }

    public void AbsorbPowerUp(PowerUp pUp)
    {
        Debug.Log("Absorbed PowerUp: " + pUp.type);
        switch (pUp.type)
        {
            case eWeaponType.shield:
                shieldLevel++;
                break;

            default:
                if (pUp.type == weapons[0].type)
                {
                    Weapon weap = GetEmptyWeaponSlot();
                    if (weap != null)
                    {
                        weap.SetType(pUp.type);
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pUp.type);
                }
                break;
        }
        pUp.AbsorbedBy(this.gameObject);
    }

    public float shieldLevel
    {
        get { return _shieldLevel; }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            // If the shield is going to be set to less than zero
            if (value < 0)
            {
                Destroy(this.gameObject); // Destroy the Hero
                Main.HERO_DIED();
            }
        }
    }

    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].type == eWeaponType.none)
            {
                return(weapons[i]);
            }
        }
        return (null);
    }

    void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(eWeaponType.none);
        }
    }
}
