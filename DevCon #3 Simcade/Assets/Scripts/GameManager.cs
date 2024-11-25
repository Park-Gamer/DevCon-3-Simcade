using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // The total number of acorns collected.
    private int acornCount = 0;

    // UI element to display the acorn count (optional).
    public TextMeshProUGUI acornCountText;

    private void Start()
    {
        // Initialize acorn count display
        UpdateAcornCountUI();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CollectAcorn();

        }
    }
    public void CollectAcorn()
    {
        // Increase the acorn count.
        acornCount++;

        // Optionally update the UI or other game systems.
        UpdateAcornCountUI();
    }

    private void UpdateAcornCountUI()
    {
        if (acornCountText != null)
        {
            acornCountText.text = acornCount.ToString();
        }
    }

    public int GetAcornCount()
    {
        return acornCount;
    }

}
