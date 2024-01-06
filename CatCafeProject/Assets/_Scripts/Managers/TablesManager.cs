using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablesManager : MonoBehaviour
{
    [SerializeField] CatMovement catMovement;    
    public List<GameObject> tableList; //de momento usar esta, realmente usar las de FurnitureManager
    private int uselessTables;
    //las mesas avisan cuando esten libres//ocupadas
    //este script avisa al gato a cual puede ir
    //si primer gato no puede pasar a x mesa avisara aqui diciendo que es inutil
    //si primer gato no puede ir a ninguna mesa avisa para que salga notificacion "reiniciar" dia

    private void Start()
    {
        catMovement.OnPathNotAvailable += CalculateUselessTables;//esto no ha funcionado
    }

    private void CalculateUselessTables() //calculate number of tables that the client cant reach (no path available)
    {
        uselessTables++;
        if(uselessTables >= tableList.Count)
        {
            Debug.Log("El cliente no puede pasar a ninguna mesa");
        }
    }
}
