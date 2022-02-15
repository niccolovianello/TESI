using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MazeMovementHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private PlayerMazeMovement playerMazeMovement;
    public void Start()
    {
        playerMazeMovement = FindObjectOfType<PlayerMazeMovement>();
    }
    public bool isPressed;


    public void OnPointerDown(PointerEventData data)
    {
        isPressed = true;
    }
    public void OnPointerUp(PointerEventData data)
    {
        isPressed = false;
    }
}

