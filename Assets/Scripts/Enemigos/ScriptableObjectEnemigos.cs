using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjectEnemigo", menuName = "ScriptableObjects/Enemigos")]
public class ScriptableObjectEnemigos : ScriptableObject
{
    //Estadisticas de los enemigos
    //Separamos la estadistica modificable de la privada igual que en el scriptableObjectArmas
    [SerializeField]
    float rapidez;
    public float Rapidez { get => rapidez; private set => rapidez = value; }

    [SerializeField]
    float vidaMaxima;
    public float VidaMaxima { get => vidaMaxima; private set => vidaMaxima = value; }

    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }

}
