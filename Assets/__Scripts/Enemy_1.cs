using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    [Header("Enemy_1 Inscribed Fields")]
    [Tooltip("# of seconds for a full sine wave")]
    public float waveFrequency = 2;
    [Tooltip("Sine wave width in meters")]
    public float waveWidth = 4;
    [Tooltip("Amount the ship will roll left anf right with the sine wave")]
    public float waveRotY = 45;

    private float x0; // The initial x value of the Enemy_1
    private float birthTime;

    void Start()
    {
        score = 100;
        x0 = pos.x;
        birthTime = Time.time;
    }

    // Override the Move function on Enemy
    public override void Move()
    {
        // Because pos is a property, we can't directly modify pos.x. We need to use a temporary variable
        Vector3 tempPos = pos;
        // Adjust the y position based on the speed
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency; // Goes from 0 to 2PI every waveFrequency seconds
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        // rotate a bit around y
        Vector3 rot = new Vector3(0, waveRotY * sin, 0);
        this.transform.rotation = Quaternion.Euler(rot);

        // base.Move() still handles the movement down in y
        base.Move();

        // print(bndCheck.isOnScreen);
    }

}
