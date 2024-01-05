using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablesManager : MonoBehaviour
{
    [SerializeField] CatMovement catMovement;
    public bool tableIsFree;
    //las mesas avisan cuando esten libres//ocupadas
    //este script avisa al gato a cual puede ir
    //si primer gato no puede pasar a x mesa avisara aqui diciendo que es inutil
    //si primer gato no puede ir a ninguna mesa avisa para que salga notificacion "reiniciar" dia
    //necesitare una lista/array de las mesas, como las referencio???

    private void Start()
    {
        catMovement.OnPathNotAvailable += CalculateUselessTables;
    }

    private void CalculateUselessTables() //calculate number of tables that the client cant reach (no path available)
    {

    }
}
