using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public SphereMove sphereMove;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sphereMove.StopMovement();
        }
            
    }
}
