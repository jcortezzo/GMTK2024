using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject GROUND_TILE_PREFAB;
    [SerializeField]
    private GameObject CORE_TILE_PREFAB;
    [SerializeField]
    private GameObject DECORATION_PREFAB;

    [Header("Planet Settings")]
    [SerializeField]
    private int NUM_SPOKES;
    [SerializeField]
    private int NUM_LAYERS;
    [SerializeField]
    private float DECORATION_DENSITY;

    [field: SerializeField]
    public float Radius { get; private set; }
    [field: SerializeField]
    public float CoreRatio { get; private set; }

    private IDictionary<int, ISet<GameObject>> _layers;

    void Awake()
    {
        _layers = new Dictionary<int, ISet<GameObject>>();
        for (int i = 0; i < NUM_LAYERS; i++)
        {
            _layers[i] = new HashSet<GameObject>();
        }
    }

    private int CORE_LAYERS
    {
        get
        {
            return (int)Mathf.Floor(NUM_LAYERS * CoreRatio);
        }
    }

    private Stack<GameObject>[] stackList;
    private float DEGREES_IN_PLANET = 360f;

    [field: SerializeField]
    public bool GeneratePlanetOnStart { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        if (GeneratePlanetOnStart) GeneratePlanet(Radius, NUM_SPOKES, NUM_LAYERS);
    }

    public void GeneratePlanet(float radius, int numSpokes, int numLayers)
    {
        stackList = new Stack<GameObject>[numSpokes];
        for (int i = 0; i < stackList.Length; i++)
        {
            stackList[i] = new Stack<GameObject>();
        }
        for (int layer = 0; layer < numLayers; layer++)
        {
            bool isCoreLayer = layer < CORE_LAYERS;

            float currRadius = (radius / numLayers) * layer;
            for (int spoke = 0; spoke < numSpokes; spoke++)
            {
                float theta = (DEGREES_IN_PLANET / numSpokes) * spoke;
                theta *= Mathf.Deg2Rad;
                float x = Mathf.Cos(theta) * currRadius;
                float y = Mathf.Sin(theta) * currRadius;

                Vector2 worldPos = new Vector2(x, y);
                Vector2 position = worldPos + (Vector2)transform.position;
                Vector3 upDirection = (position - new Vector2(transform.position.x, transform.position.y)).normalized; // Assuming transform.position is the center of the planet
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, upDirection);
                GameObject groundTile = Instantiate(!isCoreLayer ? GROUND_TILE_PREFAB : CORE_TILE_PREFAB, position, rotation, this.transform);
                stackList[spoke].Push(groundTile);
                _layers[layer].Add(groundTile);
            }
        }

        // decorate
        foreach (GameObject ground in _layers[NUM_LAYERS - 1])
        {
            if (Random.value > DECORATION_DENSITY)
            {
                continue;
            }
            // rotation
            Vector3 upDirection = (ground.transform.position - transform.position).normalized; // Assuming transform.position is the center of the planet
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, upDirection);
            // position
            SpriteRenderer groundRenderer = ground.GetComponent<SpriteRenderer>();
            SpriteRenderer decorationRenderer = DECORATION_PREFAB.GetComponent<SpriteRenderer>();
            float buffer = 2f / groundRenderer.sprite.pixelsPerUnit;
            Vector3 groundTopPosition = ground.transform.position + ground.transform.up * (groundRenderer.bounds.extents.y + decorationRenderer.bounds.extents.y - buffer);
            Instantiate(DECORATION_PREFAB, groundTopPosition, rotation, ground.transform);
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