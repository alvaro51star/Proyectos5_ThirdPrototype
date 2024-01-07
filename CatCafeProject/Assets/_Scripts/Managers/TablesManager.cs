using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablesManager : MonoBehaviour
{
    private CatMovement catMovement;
    private CatDataSO catData;
    public List<GameObject> tableList; //de momento usar esta, realmente usar las de FurnitureManager
    public List<TableData> tableDataList;
    private List<TableData> availableTableDataList;
    private int uselessTables;
    //las mesas avisan cuando esten libres//ocupadas
    //este script avisa al gato a cual puede ir
    //si primer gato no puede pasar a x mesa avisara aqui diciendo que es inutil
    //si primer gato no puede ir a ninguna mesa avisa para que salga notificacion "reiniciar" dia

    private void Start()
    {
        if(GameManager.instance.initialGameMode == GameModes.Cafeteria)//esto ahora para debug
        {
            for (int i = 0; i < tableList.Count; i++)
            {
                tableDataList.Add(tableList[i].GetComponent<TableData>());
                tableDataList[i].tablesManager = this;
                //tableDataList[i].OnAvailableTable += CheckAvailableTables;
            }
        }              
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CatMovement>() != null)
        {
            catMovement = other.GetComponent<CatMovement>();
            catData = other.GetComponent<ClientData>().catTpe;
            catMovement.tablesManager = this;
            catMovement.OnPathNotAvailable += CalculateUselessTables;

            //if cat es primero en cola
            StartCoroutine(WaitForClientMovement());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CatMovement>() != null)
        {            
            catMovement.OnPathNotAvailable -= CalculateUselessTables;
            availableTableDataList.Clear();
        }
    }

    private void CalculateUselessTables() //calculate number of tables that the client cant reach (no path available)
    {
        uselessTables++;
        Debug.Log("useless tables: " + uselessTables);
        if(uselessTables >= tableList.Count)
        {
            Debug.Log("El cliente no puede pasar a ninguna mesa");//aqui ira el menu de "reinicio"
        }
    }   

    public Transform CheckAvailableTables()//en futuro llamado por evento en TableData
    {
        for(int i = 0; i < tableDataList.Count; i++)
        {
            if(!tableDataList[i].isOcupied)
            {
                availableTableDataList.Add(tableDataList[i]);                
            }
            else
            {
                return null;
            }
        }

        for(int i = 0; i < catData.likes.Count; i++)
        {
            for (int j = 0; j < availableTableDataList.Count; j++)
            {
                if (catData.likes[i] == availableTableDataList[j].furnitureTheme)
                {
                    return availableTableDataList[j].selectedChair;
                }
            }
        }

        /*for(int i = 0; i < availableTableDataList.Count; i++)
        {
            for(int j = 0; j < catData.likes.Count; j++)
            {
                if (availableTableDataList[i].furnitureTheme == catData.likes[j])
                {
                    //catMovement.MovementToDestination(tableDataList[i].selectedChair);
                    //StartCoroutine(WaitForClientMovement(tableDataList[i].selectedChair));
                    return tableDataList[i].selectedChair;
                }
                else 
                    return null;
            }
        }*/

        /*for(int i = 0; i < availableTableDataList.Count; i++) //como hago que return la que menos distancia???
        {
            Vector3.Distance(this.gameObject.transform.position, availableTableDataList[i].selectedChair.position);
        }*/

        return null;
    }

    private IEnumerator WaitForClientMovement()
    {
        yield return new WaitForSeconds(3f);
        catMovement.MovementToDestination(CheckAvailableTables());
        //availableTableDataList.Clear();
    }

}
