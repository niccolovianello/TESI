
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Niantic.ARDKExamples.Helpers;
    



public class PlayerMazeMovement : MonoBehaviour
{
    private CharacterController controller;

    private bool initializedFlag = false;
    private float playerSpeed = .1f;
    public Button up, down, left, right;
    public Vector3 move = new Vector3(0, 0, 0);
    public Camera cam;

    public Transform initialTransform;
    private float previousAngleCamY, difBetweenAngles;

    public void InitializeMazePlayerController()
    {
        initialTransform = transform;
        controller = GetComponent<CharacterController>();
        cam = FindObjectOfType<ARCursorRenderer>().GetComponent<Camera>();

        //float scaler = FindObjectOfType<MazeLoader>().MazeScaleFactor;
        //controller.height = 1.6f * scaler;
        //controller.radius = 0.5f * scaler;
        //controller.center = new Vector3(0, 0.85f * scaler , 0);

        up = GameObject.Find("/Ui_Maze/UpMoveImage/UpMove").GetComponent<Button>();
        down = GameObject.Find("/Ui_Maze/DownMoveImage/DownMove").GetComponent<Button>();
        right = GameObject.Find("/Ui_Maze/RightMoveImage/RightMove").GetComponent<Button>();
        left = GameObject.Find("/Ui_Maze/LeftMoveImage/LeftMove").GetComponent<Button>();

        Debug.Log(up + " " + down + " " + left + " " + right);

        initializedFlag = true;
        previousAngleCamY = cam.transform.rotation.y;
    }

 
    private void Update()
    {
        difBetweenAngles = cam.transform.rotation.y - previousAngleCamY;
        if (Input.GetKey(KeyCode.E))
        {
            Debug.Log(move);
        }
        //transform.rotation.SetFromToRotation(new Vector3(0, transform.rotation.y, 0), new Vector3(0, cam.transform.rotation.y, 0));

        if (initializedFlag)
        {
            move = Vector3.zero;

            if (up.GetComponent<MazeMovementHandler>().isPressed == true)
            {
                move += Vector3.forward;
                //move = new Vector3(transform.position.x, initialTransform.position.y, transform.position.z);
                Debug.Log("Forward");

            }

            if (down.GetComponent<MazeMovementHandler>().isPressed == true)
            {
                move += Vector3.back;
               //move = new Vector3(transform.position.x, initialTransform.position.y, transform.position.z);

            }

            if (left.GetComponent<MazeMovementHandler>().isPressed == true)
            {
                move += Vector3.left;
                
            }

            if (right.GetComponent<MazeMovementHandler>().isPressed == true)
            {
                move += Vector3.right;
                

            }
            move = Quaternion.AngleAxis(difBetweenAngles, Vector3.up) * move;
            //gameObject
            if (move != Vector3.zero)
            {
                //gameObject.transform.rotation = ;
                
                controller.Move(move * Time.deltaTime * playerSpeed);
                
            }

            //gameObject.transform.rotation = new Vector3(0, cam.transform.rotation.y , 0 );




        }

    }

    



}
