using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorPersonaje : MonoBehaviour
{
    //Instancia para el singleton
    public static SelectorPersonaje instancia;

    public ScriptableObjectPersonaje datosPj;
    [HideInInspector]

    void Awake()
    {
        //La idea es conseguir un patron singleton, de forma que solo haya una instancia activa del selector de pj
       if(instancia == null)
       {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Borrado: " + this);
            Destroy(gameObject);
        }
    }

    public static ScriptableObjectPersonaje getDatosPJ()
    {
        return instancia.datosPj;
    }
   

    public void seleccionarPj(ScriptableObjectPersonaje datos)
    {
        datosPj = datos;
    }

    //importante destruir la informacion no necesaria para preservar el patron singleton
    public void DestruirSingleton()
    {
        instancia = null;
        Destroy(gameObject);
    }
}
