using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDropper : MonoBehaviour
{
    public GameObject[] prefabs;

    public float destroyTime = 10f;

    public float dropDelay = 1f;

    public float dropRange = 10f;

    public void Start()
    {
        StartCoroutine(DropPrefabs());
    }

    private IEnumerator DropPrefabs()
    {
        while (true) 
        {
           
            DropRandomPrefab();

            yield return new WaitForSeconds(dropDelay);
        }
    }

    private void DropRandomPrefab()
    {
        if (prefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, prefabs.Length);

            float randomX = transform.position.x + Random.Range(-dropRange, dropRange);
            Vector3 dropPosition = new Vector3(randomX, transform.position.y, transform.position.z);

            GameObject droppedObject = Instantiate(prefabs[randomIndex], dropPosition, Quaternion.identity);

            StartCoroutine(DestroyAfterDelay(droppedObject, destroyTime));
        }
        else
        {
            Debug.LogWarning("No prefabs assigned to the prefab array.");
        }
    }

    private IEnumerator DestroyAfterDelay(GameObject droppedObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(droppedObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
