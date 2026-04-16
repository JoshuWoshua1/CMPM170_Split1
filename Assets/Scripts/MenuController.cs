using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public TempPlayer player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (menuCanvas == null)
        {
            Debug.LogError("menuCanvas is not assigned!");
            return;
        }
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)) {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            player.SetMovementEnabled(!menuCanvas.activeSelf); // Disable player movement when menu is active
        }
    }
}
