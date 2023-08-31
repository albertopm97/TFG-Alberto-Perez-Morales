using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorDropRate : MonoBehaviour
{
    [System.Serializable]
    public class Drops //Representa un objeto que puede ser dropeado, con toda su información necesaria
    {
        public string nombre;
        public GameObject itemPrefab;
        public float probabilidad;
    }

    //Lista de todos los posibles items que pueden ser dropeados
    public List<Drops> drops;

    //Funcion que es llamada cuando un enemigo muere para lanzar los drops
    void OnDestroy()
    {
        //Si la escena no esta cargada no hacemos nada (evitar drops al cerrar el juego)
        if (!gameObject.scene.isLoaded)
        {
            return;
        }

        //Generamos un aleatorio enter 0 y 100 y comparamos con las probabilidades de drop, instanciando el prefab si se cumple
        float aleatorio = UnityEngine.Random.Range(0f, 100f);

        //BUG: es posible spawnear más de 1 item con este sistema. Solución: Añadir los items a una lista y elegir 1 solo.
        List<Drops> dropsposibles = new List<Drops>();

        foreach (Drops d in drops)
        {
            if(aleatorio <= d.probabilidad)
            {
                dropsposibles.Add(d);
            }
        }
        if(dropsposibles.Count > 0)
        {
            /*Lineas originales
            Drops elegido = dropsposibles[UnityEngine.Random.Range(0, dropsposibles.Count)];
            Instantiate(elegido.itemPrefab, transform.position, Quaternion.identity);*/

            /*Mejora de justicia **La justicia no funciona **
            float prob = float.MaxValue;

            foreach (var drop in drops)
            {
                if(drop.probabilidad < prob)
                {
                    prob = drop.probabilidad;
                    elegido = drop;
                }
            }  Fin mejora*/

            //Funciona si la lista de drops esta ordenada de menos probabilidad a más
            Drops elegido = dropsposibles[0];
            Instantiate(elegido.itemPrefab, transform.position, Quaternion.identity);
        }
    }
}

