using UnityEngine;

public class BlockDestroy : MonoBehaviour
{
    float maxHealth = 0f;
    void SetHealthForBlock()
    {
        if(gameObject.name.Contains("Dirt"))
        {
            maxHealth = 50f;
        }
        else if(gameObject.name.Contains("Stone"))
        {
            maxHealth = 150f;
        }
        else if(gameObject.name.Contains("Gold"))
        {
            maxHealth = 300f;
        }
        else if(gameObject.name.Contains("Diamond"))
        {
            maxHealth = 500f;
        }
        else if(gameObject.name.Contains("Ruby"))
        {
            maxHealth = 700f;
        }
        else if(gameObject.name.Contains("Emerald"))
        {
            maxHealth = 1000f;
        }
        else if(gameObject.name.Contains("Lava"))
        {
            maxHealth = 1500f;
        }
        else
        {
            maxHealth = 100f; // Default health for unknown blocks
        }
    }
    void OnMouseDown()
    {
        Destroy(gameObject);
    }

}
