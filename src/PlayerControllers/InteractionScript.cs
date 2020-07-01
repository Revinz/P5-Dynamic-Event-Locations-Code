using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InteractionScript
{
    public LayerMask EventMask;
    public IThrowable throwableObject = null;
    public Vector3 stoneOffset;

    public InteractionScript()
    {
        EventMask = LayerMask.GetMask("Event");
    }

    public void Update()
    {

        //Throw the stone if one is picked up, else try to pick up a new stone
        if (Input.GetMouseButtonDown(0) ||
            Input.GetButtonDown("Oculus_Button_X") || //X or A button on controllers
            Input.GetButtonDown("Oculus_Button_A")) //Note: in the input settings the VR inputs have been changed from Joystick axis to Mouse press + snap has been enabled
        {
            if (throwableObject != null)
                Throw();
            else
                EmitInteractionRay();
        }
               
    }

    public void EmitInteractionRay() {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        
        if (Physics.Raycast(ray, out hit, 10f, EventMask)) {
            Interact(hit.transform.gameObject);
        }
    }


    /// <summary>
    /// Interacts with IInteractable and IThrowable on the GameObject
    /// </summary>
    public void Interact(GameObject o)
    {
            IInteractable Interactable = o.GetComponent<IInteractable>();
            IThrowable throwable = o.gameObject.GetComponent<IThrowable>();
            if (Interactable != null)
            {
                Interactable.Interact();
            }
            else if (throwable != null)
            {
                throwableObject = throwable;
                throwableObject.Pickup();
            }
    }

    /// <summary>
    /// Throws the stone in the direction of the camera
    /// </summary>
    public void Throw()
    {
        throwableObject.Throw(Camera.main.transform.forward);
        throwableObject = null;
    }



}
