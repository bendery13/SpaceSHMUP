using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static private Main S; // Singleton

    [Header("Inscribed")]
    public GameObject[] prefabEnemies; // Array of Enemy prefabs
    public float enemySpawnPerSecond = 0.5f; // # Enemies/second
    public float enemyInsetDefault = 1.5f; // Padding for position the spawned enemies
    public float gameRestartDelay = 2f; // Time to wait before restarting the scene

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
    }

    public void SpawnEnemy()
    {
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
}
