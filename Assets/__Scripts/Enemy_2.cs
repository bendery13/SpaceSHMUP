using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Enemy_2 Inscribed Fields")]
    public float lifeTime = 10;

    // Enemy2 uses a Sine wave to modify a 2-point linear interpolation
    [Tooltip("Determines how much the Sine wave will ease the interpolation")]
    public float sineEccentricity = 0.6f;
    public AnimationCurve rotCurve;

    [Header("Enemy_2 Private Fields")]
    [SerializeField] private float birthTime;
    [SerializeField] private Vector3 p0, p1;

    private Quaternion baseRotation;
    // Start is called before the first frame update
    void Start()
    {
        // Pick andy point on the left side of the screen
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        // Pick any point on the right side of the screen
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        // Possibly swap sides
        if (Random.value > 0.5f)
        {
            p0.x *= -1;
            p1.x *= -1;
        }

        birthTime = Time.time;

        // Set up the initial ship rotation
        transform.position = p0;
        transform.LookAt(p1, Vector3.back);
        baseRotation = transform.rotation;

    }

    public override void Move()
    {
        // Becomes a parameter from 0 to 1
        float u = (Time.time - birthTime) / lifeTime;

        // If u > 1, then the enemy is too old and should be destroyed
        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        // Use the AnimationCurve to adjust the rotation
        float shipRot = rotCurve.Evaluate(u) * 360;
        // if (p0.x > p1.x) shipRot = -shipRot;
        // transform.rotation = Quaternion.Euler(0, shipRot, 0);
        transform.rotation = baseRotation * Quaternion.Euler(-shipRot, 0, 0);

        // Adjust u by adding a Sine wave to it
        u  += sineEccentricity * Mathf.Sin(u * Mathf.PI * 2);

        // Interpolate the two linear positions
        pos = (1 - u) * p0 + u * p1;
    }

}
