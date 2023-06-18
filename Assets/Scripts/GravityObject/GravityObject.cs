using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    [SerializeField] protected bool disableGravity = false;
    [SerializeField] protected Rigidbody rb;

    protected Vector3 down = Vector3.zero;
    protected List<PlanetGravity> activePlanets = new List<PlanetGravity>();

    private void Awake()
    {
        gameObject.layer = 7;
    }

    protected void FixedUpdate()
    {
        down = Vector3.zero;

        if (disableGravity) return;

        foreach (PlanetGravity p in activePlanets)
        {
            Vector3 distanceVector = p.transform.position - transform.position;
            float t = (p.transform.lossyScale.x - distanceVector.magnitude) / p.transform.lossyScale.x;
            float gravity = Mathf.Lerp(p.outerGravity, p.innerGravity, t);

            down += gravity * distanceVector.normalized;
        }

        
        rb.AddForce(down);
    }

    public void addPlanet(PlanetGravity x)
    {
        if (activePlanets.Contains(x))
        {
            Debug.LogError($"activePlanets already contains {x}");
        }
        activePlanets.Add(x);
    }

    public void removePlanet(PlanetGravity x)
    {
        if (!activePlanets.Contains(x))
        {
            Debug.LogError($"activePlanets doesn't contain {x}");
        }
        activePlanets.Remove(x);
    }
}
