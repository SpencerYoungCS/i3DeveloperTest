using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnPartClick : MonoBehaviour, ILookAtPart
{
    Camera camera;
    GameController gameController;
    Text label;
    private string textHolder;
    private string componentsName;
    private string currentCarName;
    private GameObject carBody;
    private Vector3 newPos;
    void Start()
    {
        camera = Camera.main;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        // This will change the message on the bottom to the name of the part. textholder is to change it back
        label = GameObject.Find("Message").GetComponent<Text>();
        textHolder = label.text;

        // Special Exception for components
        currentCarName = gameController.GetComponent<GameController>().currentCar.name;
        componentsName = currentCarName + "Components";
        carBody = GameObject.Find(currentCarName + "Body");

    }

    private void OnMouseUpAsButton()
    {
        // We want this to be public, so we can make our own function
        LookAtPart();
    }

    public void LookAtPart()
    {
        if (!gameController.lookingAtPart)
        {
            gameController.lookingAtPart = true;
            HideMenu();
            if (gameObject.name != componentsName)
            {
                newPos = gameObject.transform.position;
                camera.transform.position = Vector3.zero;
                label.text = gameObject.name.Remove(0, currentCarName.Length);
                // Move the camera away from the center towards the part, then look at it
                camera.transform.position += (newPos - camera.transform.position).normalized * 3f;
                camera.transform.LookAt(gameObject.transform);
            }
            //Special interaction for components to disable the body for visual clarity
            else
            {
                newPos = camera.transform.position;
                newPos.Set(newPos.x - 2f, newPos.y, newPos.z);
                camera.transform.position = newPos;
                label.text = gameObject.name.Remove(0, currentCarName.Length);
                carBody.SetActive(false);
            }
        }
        // if not looking at a part already, reset by clicking on any part
        else
        {
            ShowMenu();
            carBody.SetActive(true);
            gameController.resetCameraPosition();
            gameController.lookingAtPart = false;
            label.text = textHolder;
        }
    }

    public void HideMenu()
    {
        gameController.partsListGUI.SetActive(false);
    }

    public void ShowMenu()
    {
        gameController.partsListGUI.SetActive(true);
    }
}
