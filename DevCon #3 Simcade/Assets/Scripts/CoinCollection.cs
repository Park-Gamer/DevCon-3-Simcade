using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{

    private int Ammo = 0;

    public TextMeshProUGUI AmmoText;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Ammo")
        {
            Ammo++;
            AmmoText.text = "Chestnuts: " + Ammo.ToString();
            Debug.Log(Ammo);
            Destroy(other.gameObject);
        }
    }

}
