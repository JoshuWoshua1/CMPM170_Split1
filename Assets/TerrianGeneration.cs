using UnityEngine;

public class TerrianGeneration : MonoBehaviour
{   

    public int worldwidth = 50;
    public int worldheight = 100;
    public int dirtLayerHeight = 20;
    public float seed;
    public float heightMultiplier = 20f;
    public float noiseFrequency = 0.05f;

    
    public Sprite stone;
    public Sprite grass;
    public Sprite dirt;
    
    private void Start()
    {   
        seed = Random.Range(-10000, 10000);
        GenerateTerrian();
    }

    public void GenerateTerrian()
    {
        for(int x = 0; x < worldwidth; x++)
        {
            
            float height = Mathf.PerlinNoise((x + seed) * noiseFrequency, seed * noiseFrequency) * heightMultiplier;
            for(int y = 0; y < height; y++)
            {
                float transitionNoise = Mathf.PerlinNoise(x * 0.1f, y * 0.1f);
                Sprite tileSprite;

                if (y == 0)
                {
                    tileSprite = grass;
                }
                else if (y < dirtLayerHeight - 1)
                {
                    if (transitionNoise > 0.35f)
                        tileSprite = dirt;
                    else 
                    {
                        tileSprite = stone;
                    }
                }
                else
                {
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
