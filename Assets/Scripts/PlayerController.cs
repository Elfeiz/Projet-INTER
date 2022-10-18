using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1;//0:left 1:middle 2:right
    public float laneDistance = 4;//the distance between 2 lanes
    public float jumpForce;
    public float Gravity = -20;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!PlayerManager.isGameStarted)
        {
            return;
        }

        
        if(forwardSpeed < maxSpeed)
        {
            forwardSpeed +=0.1f * Time.deltaTime;
        }
        direction.z = forwardSpeed;
        controller.Move(direction*Time.deltaTime);

       
        if(controller.isGrounded)
        {
            
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
        }
        else
        {
            direction.y += Gravity*Time.deltaTime;
        }

        // if(Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     StartCoroutine(Slide());
        // }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane==0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane==2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        if (transform.position != targetPosition  &&  !PlayerManager.gameOver)
        {

            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
            if (moveDir.sqrMagnitude < diff.magnitude)
                controller.Move(diff);
            else
                controller.Move(diff);
        }
            
    } 

    private void Jump()
    {
        direction.y = jumpForce; 
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag =="Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }

   // private IEnumerator Slide()
   // {
   //     direction.y -= 1;
   // }
}
