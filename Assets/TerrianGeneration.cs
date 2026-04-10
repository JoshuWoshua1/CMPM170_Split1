using UnityEngine;

public class TerrianGeneration : MonoBehaviour
{   

    public int worldwidth = 50;
    public int worldheight = 100;
    public int dirtLayerHeight = 5;
   
    public float seed;
    public float noiseFrequency = 0.05f;

    public float transitionNoiseFrequency = 0.5f;
    public float transitionVariation = 5f;

    public float oreNoiseFrequency = 0.1f;
    public float goldPatchSize = 0.6f;
    public int goldMinDepth = 40;
    public int goldMaxDepth = 60;


    
    public Sprite stone;
    public Sprite grass;
    public Sprite dirt;
    public Sprite gold;

    
    private void Start()
    {   
        seed = Random.Range(-10000, 10000);
        GenerateTerrian();
    }

    public void GenerateTerrian()
    {
        for(int x = 0; x < worldwidth; x++)
        {
            float transitionLine = dirtLayerHeight + Mathf.PerlinNoise((x + seed) * transitionNoiseFrequency, seed) * transitionVariation;
            for(int y = 0; y < worldheight; y++)
            {
                float transitionNoise = Mathf.PerlinNoise(x * 0.1f, y * 0.1f);
                Sprite tileSprite;

                if (y == 0)
                {
                    tileSprite = grass;
                }
                else if (y < transitionLine)
                {
                    tileSprite = dirt;
                }
                else
                {
                    float goldNoise = Mathf.PerlinNoise((x + seed) * oreNoiseFrequency, (y + seed) * oreNoiseFrequency);
                    bool isGoldPatch = (y > goldMinDepth && y < goldMaxDepth) && goldNoise > .7f;
                    if (isGoldPatch)
                    {
                        tileSprite = gold;
                    }  
                    else
                        tileSprite = stone;
                }
                GameObject newTile = new GameObject(name = "tile");
                newTile.AddComponent<SpriteRenderer>();
                newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
                newTile.transform.position = new Vector2(x + 0.5f, -(y + 0.5f));
            }
        }
    }

    
}
