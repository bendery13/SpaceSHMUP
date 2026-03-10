using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks whether a GameObject is on screen and can force it to stay on screen
/// Keeps a GameObject on screen
/// Note that this only works for an orthographic Main Camera
/// </summary>
public class BoundsCheck : MonoBehaviour
{
    [System.Flags]
    public enum eScreenLocs
    {
        onScreen = 0,
        offRight = 1,
        offLeft = 2,
        offUp = 4,
        offDown = 8

    }
    public enum eType {center, inset, outset};
    [Header("Inscribed")]
    public eType boundsType = eType.center;
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Dynamic")]
    public eScreenLocs screenLocs = eScreenLocs.onScreen;
    // public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;


    void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        float checkRadius = 0;
        if (boundsType == eType.inset) checkRadius = -radius;
        if (boundsType == eType.outset) checkRadius = radius;

        Vector3 pos = transform.position;
        screenLocs = eScreenLocs.onScreen;

        // Restrict the X position to camWidth
        if (pos.x > camWidth + checkRadius)
        {
            pos.x = camWidth + checkRadius;
            screenLocs |= eScreenLocs.offRight; // This adds the offRight flag to screenLocs
            // isOnScreen = false;
        }
        if (pos.x < -camWidth - checkRadius)
        {
            pos.x = -camWidth - checkRadius;
            screenLocs |= eScreenLocs.offLeft; // This adds the offLeft flag to screenLocs
            // isOnScreen = false;
        }

        // Restrict the Y position to camHeight
        if (pos.y > camHeight + checkRadius)
        {
            pos.y = camHeight + checkRadius;
            screenLocs |= eScreenLocs.offUp; // This adds the offUp flag to screenLocs
            // isOnScreen = false;
        }
        if (pos.y < -camHeight - checkRadius)
        {
            pos.y = -camHeight - checkRadius;
            screenLocs |= eScreenLocs.offDown; // This adds the offDown flag to screenLocs
            // isOnScreen = false;
        }

        if (keepOnScreen && !isOnScreen){
            transform.position = pos;
            screenLocs = eScreenLocs.onScreen; // This resets screenLocs to onScreen
            // isOnScreen = true;
        }
    }

    public bool isOnScreen
    {
        get {return (screenLocs == eScreenLocs.onScreen);}
    }

    public bool LocIs(eScreenLocs checkLoc){
        if (checkLoc == eScreenLocs.onScreen) return isOnScreen;
        return ((screenLocs & checkLoc) == checkLoc);
    }
}
