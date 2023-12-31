using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    //ponerlo en cada encimera
    //cada x segundos (modificable) instancia un prefab en x punto (centro encimera)
    [SerializeField] private GameObject foodGO;
    [SerializeField] private float secondsToSpawn;
    [SerializeField] private Transform spawnLocation;//centro encimera

    private void Start()
    {
        //se spawnea desde inicio el GO
        SpawnFood(0);
    }
    //timer en corutina para evitar update, TENER CUIDADO EN PAUSA

    public void SpawnFood (float seconds) //se llamara el resto de veces con un evento (cuando has cogido la comida)
    {
        StartCoroutine(SpawnTimer(seconds));
        Instantiate(foodGO, spawnLocation);
    }
    private IEnumerator SpawnTimer (float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
