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
        GameObject otherGo = coll.gameObject;
        if (otherGo.GetComponent<ProjectileHero>() != null)
        {
            Destroy(otherGo);   // Destroy the ProjectileHero
            Destroy(gameObject); // Destroy this Enemy
        } else{
            Debug.LogWarning("Enemy hit by non-ProjectileHero: " + otherGo.name);
        }
    }
}
