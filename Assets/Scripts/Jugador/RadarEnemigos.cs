using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarEnemigos : MonoBehaviour
{

    private GameObject[] enemigos;
    public Transform enemigoMasCercano;
    public bool contacto;

    // Start is called before the first frame update
    void Start()
    {
        enemigoMasCercano = null;
        contacto = false;    

    }

    // Update is called once per frame
    void Update()
    {
        enemigoMasCercano = getEnemigoMasCercano();
        Debug.Log(enemigoMasCercano.name);
    }

    public Transform getEnemigoMasCercano()
    {
        enemigos = GameObject.FindGameObjectsWithTag("Enemigos");
        float menorDistancia = Mathf.Infinity;
        Transform aux = null;

        foreach(GameObject go in enemigos)
        {
            float distanciaActual;
            distanciaActual = Vector3.Distance(transform.position, go.transform.position);

            if (distanciaActual < menorDistancia)
            {
                menorDistancia = distanciaActual;
                aux = go.transform;
            }
        }

        return aux;
        //posible mejora usar los enemigos que se ven en la camara 
    }

        
}
