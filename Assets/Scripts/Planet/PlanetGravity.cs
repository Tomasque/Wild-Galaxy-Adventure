using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    public float outerGravity;
    public float innerGravity;

    private void OnTriggerEnter(Collider other)
    {
        print("a ");
        PlayerMovement.instance.addPlanet(this);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovement.instance.removePlanet(this);
    }

}
