using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEnd : MonoBehaviour
{
    private GameObject winPanel;
    private GameObject losePanel;

    private void Awake()
    {
        winPanel = GameObject.Find("WinPanel");
        losePanel = GameObject.Find("LosePanel");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Destroy(other.gameObject);
            winPanel.SetActive(true);
            losePanel.SetActive(false);
        }
    }
}
