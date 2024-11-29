using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // The total number of acorns collected.
    public int acornCount = 0;

    public TextMeshProUGUI acornCountText;

    private void Start()
    {
        UpdateAcornCountUI();
    }
    public void CollectAcorn()
    {
        // Increase the acorn count.
        acornCount++;

        UpdateAcornCountUI();
    }

    public void ReduceAcorn()
    {
        if (acornCount != 0)
        {
            acornCount -= 1;
        }
        UpdateAcornCountUI();
    }

    public void ResetAcorn()
    {
        if (acornCount != 0)
        {
            while (acornCount != 0)
            {
                acornCount -= 1;
            }
        }
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
