using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSupply : MonoBehaviour
{
    public GameObject[] ObjetosPraSpawnar;

    // Update is called once per frame
    public void SpawnarSupplyDentroDaArea()
    {
        
        // Random position within this transform
        Vector3 rndPosWithin;
        rndPosWithin = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
        rndPosWithin = transform.TransformPoint(rndPosWithin * .5f);
        Instantiate(ObjetosPraSpawnar[Random.Range(0,ObjetosPraSpawnar.Length)], rndPosWithin, Quaternion.identity);
        
    }
}
