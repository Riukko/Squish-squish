using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : MonoBehaviour
{
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject losePanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Destroy(other.gameObject);
            winPanel.SetActive(false);
            losePanel.SetActive(true);
        }
    }
}
