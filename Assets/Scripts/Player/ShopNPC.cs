using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopNPC : MonoBehaviour
{
    private bool playerInRange = false;
    private bool shopOpen = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!shopOpen)
            {
                SceneManager.LoadScene("ShopScene_Testing", LoadSceneMode.Additive);
                shopOpen = true;
            }
            else
            {
                SceneManager.UnloadSceneAsync("ShopScene_Testing");
                shopOpen = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
