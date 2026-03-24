using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static private Main S; // Singleton
    static private Dictionary<eWeaponType, WeaponDefinition> WEAP_DICT; 

    [Header("Inscribed")]
    public bool spawnEnemies = true;
    public GameObject[] prefabEnemies; // Array of Enemy prefabs
    public float enemySpawnPerSecond = 0.5f; // # Enemies/second
    public float enemyInsetDefault = 1.5f; // Padding for position the spawned enemies
    public float gameRestartDelay = 2f; // Time to wait before restarting the scene
    public GameObject prefabPowerUp; // A prefab for the PowerUp
    public WeaponDefinition[] weaponDefinitions; // Array of WeaponDefinintions
    public eWeaponType[] powerUpFrequency = new eWeaponType[] {
      eWeaponType.blaster, eWeaponType.blaster, 
      eWeaponType.spread, eWeaponType.shield  };
    private BoundsCheck bndCheck;

    void Awake()
    {
        S = this; // Set the Singleton
        bndCheck = GetComponent<BoundsCheck>();
        if (bndCheck == null)
        {
            // Main uses camera bounds for spawning; ensure a BoundsCheck exists.
            bndCheck = gameObject.AddComponent<BoundsCheck>();
        }

        // Invoke SpawnEnemy() once (in 2 seconds), then continue to invoke SpawnEnemy() once per enemySpawnPerSecond seconds
        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);

        // A generic Dictionary with eWeaponType as the key
        WEAP_DICT = new Dictionary<eWeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    public void SpawnEnemy()
    {
        if (!spawnEnemies)
        {
            Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
            return;
        }
        if (prefabEnemies == null || prefabEnemies.Length == 0)
        {
            Debug.LogError("Main.SpawnEnemy() - prefabEnemies is empty. Assign enemy prefabs in the Inspector.");
            return;
        }

        // Pick a random Enemy prefab to instantiate
        int ndx = Random.Range(0, prefabEnemies.Length);
        if (prefabEnemies[ndx] == null)
        {
            Debug.LogError("Main.SpawnEnemy() - Selected enemy prefab is null.");
            return;
        }
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        // Position the Enemy above the screen with a random x position
        float enemyInset = enemyInsetDefault;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyInset = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        // Set the initial position for the spawned Enemy
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyInset;
        float xMax = bndCheck.camWidth - enemyInset;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyInset;
        go.transform.position = pos;

        // Invoke SpawnEnemy() again
        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
    }

    void DelayedRestart()
    {
        // Invoke the Restart() method in delay seconds
        Invoke(nameof(Restart), gameRestartDelay);
    }

    void Restart()
    {
       // Reload _Scene_0 to restart the game
        SceneManager.LoadScene("__Scene_0"); 
    }

    static public void HERO_DIED()
    {
        // Call the DelayedRestart() method on the singleton Main instance
        S.DelayedRestart();
    }
    
    static public WeaponDefinition GET_WEAPON_DEFINITION(eWeaponType wt)
    {
        // Check to make sure that the key exists in the Dictionary
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        // This will return a definition for eWeaponType.none, which has the "none" string and a white color. This way, it will return something even if we forgot to define the key in the Inspector.
        return (new WeaponDefinition());
    }

    /// <summary>
    ///  Called by an Enemy when it is destroyed. It may or may not 
    /// cause a PowerUp to be created.
    ///  <summary>
    static public void SHIP_DESTROYED(Enemy e)
    {
        // Potentially generate a PowerUp
        if (Random.value < e.powerUpDropChance)
        {
            // Choose which PowerUp to pick
            int ndx = Random.Range(0, S.powerUpFrequency.Length);
            eWeaponType pUpType = S.powerUpFrequency[ndx];

            // Spawn a PowerUp
            GameObject go = Instantiate<GameObject>(S.prefabPowerUp);
            PowerUp pUp = go.GetComponent<PowerUp>();
            pUp.SetType(pUpType);

            // Set the PowerUp to the position of the destroyed ship
            pUp.transform.position = e.transform.position;
        }
    }

}
