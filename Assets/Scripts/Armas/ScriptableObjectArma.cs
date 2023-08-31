using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ScriptableObjectArma", menuName = "ScriptableObjects/Armas")]
public class ScriptableObjectArma : ScriptableObject
{

    //Aqui vamos a poner las estadisticas globales de las armas
    //Separamos la estadistica que modificamos de la que se usa en el gameplay para evitar errores por modificar mientras se usa.
    [SerializeField]
    GameObject prefab;
    public GameObject Prefab { get => prefab; private set => prefab = value; }

    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }

    [SerializeField]
    float rapidez;
    public float Rapidez { get => rapidez; private set => rapidez = value; }

    [SerializeField]
    float enfriamiento;
    public float Enfriamiento { get => enfriamiento; private set => enfriamiento = value; }


    [SerializeField]
    int perforacion;
    public int Perforacion { get => perforacion; private set => perforacion = value; }

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
