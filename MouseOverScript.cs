using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverScript : MonoBehaviour
{
    public GameObject gameController;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    // This is will enable or disable emission when hovering overing an object. 
    // This can be changed to do something else in case if emissions may not be viable in the future
    private void OnMouseOver()
    {
        // Make sure im not in the middle of rotating the object before enabling emissions
        if (!gameController.GetComponent<GameController>().isRotating)
        {
            GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.yellow * .2f);
        }
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }
}
