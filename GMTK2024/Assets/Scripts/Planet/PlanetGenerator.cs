using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject GROUND_TILE_PREFAB;

    [field: SerializeField]
    public float Radius { get; private set; }
    [SerializeField]
    private int NUM_SPOKES;
    [SerializeField]
    private int NUM_LAYERS;

    private Stack<GameObject>[] stackList;
    private float DEGREES_IN_PLANET = 360f;

    // Start is called before the first frame update
    void Start()
    {
        GeneratePlanet();
    }

    public void GeneratePlanet()
    {
        stackList = new Stack<GameObject>[NUM_SPOKES];
        for (int i = 0; i < stackList.Length; i++)
        {
            stackList[i] = new Stack<GameObject>();
        }
        for (int layer = 0; layer < NUM_LAYERS; layer++)
        {
            float currRadius = (Radius / NUM_LAYERS) * layer;
            for (int spoke = 0; spoke < NUM_SPOKES; spoke++)
            {
                float theta = (DEGREES_IN_PLANET / NUM_SPOKES) * spoke;
                theta *= Mathf.Deg2Rad;
                float x = Mathf.Cos(theta) * currRadius;
                float y = Mathf.Sin(theta) * currRadius;

                Vector2 worldPos = new Vector2(x, y);
                Vector2 position = worldPos + (Vector2)transform.position;
                GameObject groundTile = Instantiate(GROUND_TILE_PREFAB, position, Quaternion.identity, this.transform);
                stackList[spoke].Push(groundTile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    // y = sqrt(r^2 - x^2)
    // y = sin(theta)*r
    // x = cos(theta)*r
}