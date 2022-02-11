
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;



public class PlayerMazeMovement : MonoBehaviour
{
    private CharacterController controller;

    private float playerSpeed = 10.0f;
    public Button up, down, left, right;
    public Vector3 move = new Vector3(0, 0, 0);

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        up = GameObject.Find("/Ui_Maze/UpMoveImage/UpMove").GetComponent<Button>();
        down = GameObject.Find("/Ui_Maze/DownMoveImage/DownMove").GetComponent<Button>();
        right = GameObject.Find("/Ui_Maze/RightMoveImage/RightMove").GetComponent<Button>();
        left = GameObject.Find("/Ui_Maze/LeftMoveImage/LeftMove").GetComponent<Button>();

        Debug.Log(up + " " + down + " " + left + " " + right);


    }

 
    private void Update()
    {
        move = Vector3.zero;

        if (up.GetComponent<MazeMovementHandler>().isPressed == true)
        {
            move += Vector3.forward;

        }

        if(down.GetComponent<MazeMovementHandler>().isPressed == true)
        {
            move +=  Vector3.back;

        }

        if (left.GetComponent<MazeMovementHandler>().isPressed == true)
        {
            move += Vector3.left;            
        }

        if (right.GetComponent<MazeMovementHandler>().isPressed == true)
        {
            move += Vector3.right;
            
        }

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

    }



}
