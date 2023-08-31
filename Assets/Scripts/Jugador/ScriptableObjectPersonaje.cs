using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjectPersonaje", menuName = "ScriptableObjects/Personaje")]
public class ScriptableObjectPersonaje : ScriptableObject
{
    [SerializeField]
    GameObject armaInicial;
    public GameObject ArmaInicial { get => armaInicial; private set => armaInicial = value; }

    [SerializeField]
    float vidaMaxima;
    public float VidaMaxima { get => vidaMaxima; private set => vidaMaxima = value; }

    [SerializeField]
    float recuperacion;
    public float Recuperacion { get => recuperacion; private set => recuperacion = value; }

    [SerializeField]
    float velocidadMovimiento;
    public float VelocidadMovimiento { get => velocidadMovimiento; private set => velocidadMovimiento = value; }

    [SerializeField]
    float poder;
    public float Poder { get => poder; private set => poder = value; }

    [SerializeField]
    float rapidezProyectil;
    public float RapidezProyectil { get => rapidezProyectil; private set => rapidezProyectil = value; }

    [SerializeField]
    float imanObjetos;
    public float ImanObjetos { get => imanObjetos; private set => imanObjetos = value; }

}
