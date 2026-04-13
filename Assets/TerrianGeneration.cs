using UnityEngine;

public class TerrianGeneration : MonoBehaviour
{   
    public GameObject grassBlock;
    public GameObject dirtBlock;
    public GameObject stoneBlock;
    public GameObject goldBlock;
    public GameObject diamondBlock;
    public GameObject rubyBlock;
    public GameObject emeraldBlock;
    public GameObject greystoneBlock;

    public int worldwidth = 50;
    public int worldheight = 100;

    public int dirtLayerHeight = 10;
    public float seed;

    public float heightMultiplier = 5f;

    public float noiseFrequency = 0.05f;

    public float transitionNoiseFrequency = 0.1f;
    public float transitionVariation = 20f;


    public float goldoreNoiseFrequency = 0.5f;
    public int goldMinDepth = 20;
    public int goldMaxDepth = 50;
    public int goldMaxDepthbuffer = 10;


    public float goldTransitionNoiseFrequency = 0.1f;
    public float goldTransitionVariation = 10f;

    public float diamondNoiseFrequency = 0.5f;
    public int diamondMinDepth = 30;
    public int diamondMaxDepth = 60;
    public int diamondMaxDepthbuffer = 10;

    public float diamondTransitionNoiseFrequency = 0.15f;
    public float diamondTransitionVariation = 15f;

    public float rubyNoiseFrequency = 0.5f;
    public int rubyMinDepth = 40;
    public int rubyMaxDepth = 70;
    public int rubyMaxDepthbuffer = 10;

    public float rubyTransitionNoiseFrequency = 0.20f;
    public float rubyTransitionVariation = 15f;

    public float emeraldNoiseFrequency = 0.5f;
    public int emeraldMinDepth = 50;
    public int emeraldMaxDepth = 80;
    public int emeraldMaxDepthbuffer = 10;

    public float emeraldTransitionNoiseFrequency = 0.25f;
    public float emeraldTransitionVariation = 15f;

    public Sprite stone;
    public Sprite greystone;
    public Sprite grass;
    public Sprite dirt;
    public Sprite gold;
    public Sprite diamond;
    public Sprite ruby;
    public Sprite emerald;



    
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
            float goldtransitionLine = (goldMaxDepth - goldMaxDepthbuffer) + Mathf.PerlinNoise((x + seed) * goldTransitionNoiseFrequency, seed) * goldTransitionVariation;
            float diamondtransitionLine = (diamondMaxDepth - diamondMaxDepthbuffer) + Mathf.PerlinNoise((x + seed) * diamondTransitionNoiseFrequency, seed) * diamondTransitionVariation;
            float rubytransitionLine = (rubyMaxDepth - rubyMaxDepthbuffer) + Mathf.PerlinNoise((x + seed) * rubyTransitionNoiseFrequency, seed) * rubyTransitionVariation;
            float emeraldtransitionLine = (emeraldMaxDepth - emeraldMaxDepthbuffer) + Mathf.PerlinNoise((x + seed) * emeraldTransitionNoiseFrequency, seed) * emeraldTransitionVariation;

            for(int y = 0; y < worldheight; y++)
            {
                float transitionNoise = Mathf.PerlinNoise(x * 0.1f, y * 0.1f);
                GameObject blocktoplace = null;

                if (y == 0)
                {
                    blocktoplace = grassBlock;
                }
                else if (y < transitionLine)
                {
                    blocktoplace = dirtBlock;
                }
                else if (y < goldtransitionLine)
                {
                    float goldNoise = Mathf.PerlinNoise((x + seed) * goldoreNoiseFrequency, (y + seed) * goldoreNoiseFrequency);
                    bool isGoldPatch = (y > goldMinDepth && y < goldMaxDepth) && goldNoise > .7f;
                    if (isGoldPatch)
                    {
                        blocktoplace = goldBlock;
                    }  
                    else
                        blocktoplace = stoneBlock;
                }
                else if (y < diamondtransitionLine)
                {
                    float diamondNoise = Mathf.PerlinNoise((x + seed) * diamondNoiseFrequency, (y + seed) * diamondNoiseFrequency);
                    bool isDiamondPatch = (y > diamondMinDepth && y < diamondMaxDepth) && diamondNoise > .7f;
                    if (isDiamondPatch)
                    {
                        blocktoplace = diamondBlock;
                    }  
                    else
                        blocktoplace = stoneBlock;
                }
                else if (y < rubytransitionLine)
                {
                    float rubyNoise = Mathf.PerlinNoise((x + seed) * rubyNoiseFrequency, (y + seed) * rubyNoiseFrequency);
                    bool isRubyPatch = (y > rubyMinDepth && y < rubyMaxDepth) && rubyNoise > .7f;
                    if (isRubyPatch)
                    {
                        blocktoplace = rubyBlock;
                    }  
                    else
                        blocktoplace = greystoneBlock;
                }
                else if (y < emeraldtransitionLine)
                {
                    float emeraldNoise = Mathf.PerlinNoise((x + seed) * emeraldNoiseFrequency, (y + seed) * emeraldNoiseFrequency);
                    bool isEmeraldPatch = (y > emeraldMinDepth && y < emeraldMaxDepth) && emeraldNoise > .7f;
                    if (isEmeraldPatch)
                    {
                        blocktoplace = emeraldBlock;
                    }  
                    else
                        blocktoplace = greystoneBlock;
                }
                else
                {
                    blocktoplace = greystoneBlock;
                }

                GameObject newTile = new GameObject(name = "tile");
                newTile.AddComponent<SpriteRenderer>();
                newTile.GetComponent<SpriteRenderer>().sprite = blocktoplace.GetComponent<SpriteRenderer>().sprite;
                newTile.transform.position = new Vector2(x + 0.5f, -(y + 0.5f));
            }
        }
    }

    
}
