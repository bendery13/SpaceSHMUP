using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 10f;   // The movement speed is 10m/s
    public float fireRate = 0.3f; // Seconds/shot (Unused)
    public float health = 10; // Damage needed tp destroy this enemy
    public int score = 100; // Points earned for destroying this
    public float powerUpDropChance = 1f; // Chance to drop a power-up
    protected bool calledShipDestroyed = false; // Flag to prevent multiple calls to ShipDestroyed()
    protected BoundsCheck bndCheck;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        bndCheck.keepOnScreen = false;
    }

    // This is a property: A method that acts like a field
    public Vector3 pos
    {
        get
        {
            return this.transform.position;
        }
        set
        {
            this.transform.position = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown))
        {
            Destroy(gameObject);
        }
        // Check whether the enemy has gone off screen
        // if (!bndCheck.isOnScreen)
        // {
        //     if (pos.y < bndCheck.camHeight - bndCheck.radius)
        //     {
        //         // It's gone off the bottom of the screen. Destroy it
        //         Destroy(gameObject);
        //     }
        // }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter(Collision coll){
        GameObject otherGO = coll.gameObject;

        //Check for collisions with projectileHero
        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if (p != null)
        {
            if (bndCheck.isOnScreen)
            {
                health -= Main.GET_WEAPON_DEFINITION(p.type).damageOnHit;
                if (health <= 0)
                {
                    ScoreCounter.AddScore(score);
                    HighScore.TRY_SET_HIGH_SCORE(ScoreCounter.SCORE);
                    // Tell Main that this ship has been destroyed
                    if (!calledShipDestroyed)
                    {
                        calledShipDestroyed = true;
                        Main.SHIP_DESTROYED(this);
                    }
                    Destroy(this.gameObject);
                }
            }
            Destroy(otherGO);
        }
        else
        {
            print ("Enemy hit by non-ProjectileHero: " + otherGO.name);
        }
    }
}
