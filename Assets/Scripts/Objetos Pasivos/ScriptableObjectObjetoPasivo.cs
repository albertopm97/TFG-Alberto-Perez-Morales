using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjectObjetoPasivo", menuName = "ScriptableObjects/Objeto Pasivo")]
public class ScriptableObjectObjetoPasivo : ScriptableObject
{
    [SerializeField]
    float multiplicador;

    public float Multiplicador { get => multiplicador; private set => multiplicador = value; }

    [SerializeField]
    int nivel;
    public int Nivel { get => nivel; private set => nivel = value; }

    [SerializeField]
    GameObject prefabSiguienteNivel;
    public GameObject PrefabSiguienteNivel { get => prefabSiguienteNivel; private set => prefabSiguienteNivel = value; }

    [SerializeField]
    string nombre;
    public string Nombre { get => nombre; private set => nombre = value; }

    [SerializeField]
    string descripcion;
    public string Descripcion { get => descripcion; private set => descripcion = value; }

    [SerializeField]
    Sprite icono;
    public Sprite Icono { get => icono; private set => icono = value; }
}
