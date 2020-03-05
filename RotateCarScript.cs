using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Simple script to rotate an object's Y axis when a player click and holds it
public class RotateCarScript : MonoBehaviour, ILookAtPart
{
    public float sensitvity = 3f;
    public bool mouseDown;
    public bool isRotating;
    public GameController gameController;
    private GameObject currentCar;
    private float timeToActivate;
    private Vector3 newPos;
    private Text label;
    private string textHolder;

    private void Start()
    {
        currentCar = GameObject.FindGameObjectWithTag("Car");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        label = GameObject.Find("Message").GetComponent<Text>();
        textHolder = label.text;
    }

    void Update()
    {
        // This allows the user to click and hold the body to rotate the whole thing!
        if (mouseDown)
        {
            timeToActivate -= Time.deltaTime;
            if (timeToActivate < 0)
            {
                gameController.isRotating = true;
                isRotating = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                // This rotates the Y axis, by getting the mouse X axis input
                currentCar.transform.Rotate(0, Input.GetAxis("Mouse X") * -sensitvity, 0);
            }
        }
    }

    private void OnMouseDown()
    {
        timeToActivate = 0.2f;
        mouseDown = true;
    }
    private void OnMouseUp()
    {
        mouseDown = false;
        isRotating = false;
        gameController.isRotating = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (timeToActivate > 0)
        {
            LookAtPart();
        }
    }

    public void LookAtPart()
    {
        if (!gameController.lookingAtPart)
        {
            HideMenu();
            gameController.lookingAtPart = true;
            newPos = Camera.main.transform.position;
            newPos.Set(newPos.x - 1.5f, newPos.y, newPos.z);
            Camera.main.transform.position = newPos;
            label.text = gameObject.name.Remove(0, currentCar.name.Length);
        }
        // if not looking at a part already, reset by clicking on any part
        else
        {
            ShowMenu();
            gameController.resetCameraPosition();
            gameController.lookingAtPart = false;
            label.text = textHolder;
        }
    }

    public void HideMenu()
    {
        gameController.partsListGUI.SetActive(false);
    }
    public void ShowMenu() { 
        gameController.partsListGUI.SetActive(true);
    }
}
