using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Inscribed")]
    public float rotationsPerSecond = 0.1f;

    [Header("Dynamic")]
    public int levelShown = 0; // This is set between lines // c & d

    // This is a non-public variable that will not appear in the Inspector
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        // Read the current shield level from the Hero Singleton
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);
        // If this is different from levelShown...
        if (levelShown != currLevel)
        {
            levelShown = currLevel; // Set levelShown to the new level
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0); // Move the texture
        }
        // Rotate the shield a bit every frame in a time-based way
        float rz = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rz);
    }
}
