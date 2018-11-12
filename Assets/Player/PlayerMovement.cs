using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveStopRadius = 5f;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickpoint;

    bool isInDirectMode = false;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }

    // Fixed update is called in sync with physics
    //private void fixedupdate()
    //{
    //    if (input.getkeydown(keycode.g))
    //    {
    //        isindirectmode = !isindirectmode;
    //        currentdestination = transform.position;
    //    }
    //    if (isindirectmode)
    //    {
    //        processdirectmovement();
    //    }
    //    else
    //    {
    //        processmousemovement();
    //    }
        
    //}

    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move(movement, false, false);
    }

    //private void processmousemovement()
    //{
    //    if (input.getmousebutton(0))
    //    {
    //        clickpoint = cameraraycaster.hit.point;
    //        switch (cameraraycaster.currentlayerhit)
    //        {
    //            case layer.walkable:
    //                currentdestination = shortdestination(clickpoint, walkmovestopradius);  // so not set in default case
    //                break;

    //            case layer.enemy:
    //                currentdestination = shortdestination(clickpoint, attackmovestopradius);
    //                break;

    //            default:
    //                print("not allowed to move here");
    //                return;
    //        }

    //    }

    //    walktodestination();
    //}

    private void WalkToDestination()
    {
        var playertoClickPoint = currentDestination - transform.position;
        if (playertoClickPoint.magnitude >= 0)
        {
            thirdPersonCharacter.Move(playertoClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    private Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    private void OnDrawGizmos()
    {
        //Draw Movement Gizmos
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, clickpoint);
        Gizmos.DrawSphere(currentDestination, 0.15f);
        Gizmos.DrawSphere(clickpoint, 0.1f);

        //Draw attack Sphere
        Gizmos.color = new Color(255f, 0f, 0f,0.7f);
        Gizmos.DrawWireSphere(transform.position,attackMoveStopRadius);
    }
}

