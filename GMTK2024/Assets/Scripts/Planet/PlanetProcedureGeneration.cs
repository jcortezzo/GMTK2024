using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetProcedureGeneration : MonoBehaviour
{
    public static PlanetProcedureGeneration Instance;

    [SerializeField]
    private GameObject planetPrefab;

    void Start()
    {
        Instance = this;
    }

    public void GeneratePlanet(Planet startingPlanet)
    {
        // get starting planet radius
        // get new random planet radius

        // loop try generate X times
        //      pick random direction vector + 
        //      create circle cast to see if it overlap with anything
        //      if yes: retry
        //      if no:
        //        generate planet

    }
}
