using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    public float outerGravity;
    public float innerGravity;
    public Transform cam;

    private void Awake()
    {
        gameObject.layer = 6;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.SendMessage("addPlanet", this);
    }

    private void OnTriggerExit(Collider other)
    {
        other.SendMessage("removePlanet", this);
    }

}
