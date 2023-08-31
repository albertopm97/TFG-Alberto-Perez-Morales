using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestorInventario : MonoBehaviour
{
    EstadisticasJugador jugador;

    //Listas para almacenar los objetos equipados y sus niveles correspondientes
    public List<ControladorArmas> ranurasArmas = new List<ControladorArmas>(6);
    public int[] nivelesArmas = new int[6];
    public List<ObjetoPasivo> ranurasObjetos = new List<ObjetoPasivo>(6);
    public int[] nivelesObjetos = new int[6];
    
    //Clases para gestionar la pantalla de seleccion de mejoras
    [System.Serializable]
    public class MejoraArma
    {
        public int idMejoraArma;
        public GameObject armaInicial;
        public ScriptableObjectArma datosArma;
    }

    [System.Serializable]
    public class MejoraPasivo
    {
        public int idMejoraPasivo;
        public GameObject objetoInicial;
        public ScriptableObjectObjetoPasivo datosObjeto;
    }

    [System.Serializable]
    public class UIMejora
    {
        public Text nombreMejora;
        public Text descripcionMejora;
        public Image iconoMejora;
        public Button botonMejora;
    }

    public List<MejoraArma> opcionesMejoraArma = new List<MejoraArma>();
    public List<MejoraPasivo> opcionesMejorapasivo = new List<MejoraPasivo>();
    public List<UIMejora> opcionesMejoraUI = new List<UIMejora>();

    void Start()
    {
        jugador = GetComponent<EstadisticasJugador>();   
    }
    public void equiparArma(int ranura, ControladorArmas arma)
    {
        ranurasArmas[ranura] = arma;
        nivelesArmas[ranura] = arma.estadisticas.Nivel;

        if(GameManager.instancia != null && GameManager.instancia.mejorandoEquipamiento)
        {
            GameManager.instancia.finMenuMejora();
        }
    }

    public void equiparPasivo(int ranura, ObjetoPasivo pasivo)
    {
        ranurasObjetos[ranura] = pasivo;
        nivelesObjetos[ranura] = pasivo.datosObjeto.Nivel;

        if (GameManager.instancia != null && GameManager.instancia.mejorandoEquipamiento)
        {
            GameManager.instancia.finMenuMejora();
        }
    }


    public void mejorarArma(int ranura, int id)
    {
        //Importante comprobar que haya arma en la ranura correspondiente antes de intentar mejorarla
        if(ranurasArmas.Count > ranura)
        {
            ControladorArmas armaBase = ranurasArmas[ranura];

            //Comprobamos si el siguiente nivel del arma existe (Salimos si es lv maximo o el siguiente no esta implementado)
            if (!armaBase.estadisticas.PrefabSiguienteNivel)
            {
                Debug.LogError("No hay mejora para " + armaBase.name);
                return;
            }
            GameObject armaMejorada = Instantiate(armaBase.estadisticas.PrefabSiguienteNivel, transform.position, Quaternion.identity);
            armaMejorada.transform.SetParent(transform);
            equiparArma(ranura, armaMejorada.GetComponent<ControladorArmas>());
            Destroy(armaBase.gameObject);
            nivelesArmas[ranura] = armaMejorada.GetComponent<ControladorArmas>().estadisticas.Nivel;

            opcionesMejoraArma[id].datosArma = armaMejorada.GetComponent<ControladorArmas>().estadisticas;

            if (GameManager.instancia != null && GameManager.instancia.mejorandoEquipamiento)
            {
                GameManager.instancia.finMenuMejora();
            }
        }
    }

    public void mejorarPasivo(int ranura, int id)
    {
        //Importante comprobar que haya objeto en la ranura correspondiente antes de intentar mejorarlo
        if (ranurasObjetos.Count > ranura)
        {
            ObjetoPasivo objetoBase = ranurasObjetos[ranura];
            //Comprobamos si el siguiente nivel del objeto existe (Salimos si es lv maximo o el siguiente no esta implementado)
            if (!objetoBase.datosObjeto.PrefabSiguienteNivel)
            {
                Debug.LogError("No hay mejora para " + objetoBase.name);
                return;
            }
            GameObject objetoMejorado = Instantiate(objetoBase.datosObjeto.PrefabSiguienteNivel, transform.position, Quaternion.identity);
            objetoMejorado.transform.SetParent(transform);
            equiparPasivo(ranura, objetoMejorado.GetComponent<ObjetoPasivo>());
            Destroy(objetoBase.gameObject);
            nivelesObjetos[ranura] = objetoMejorado.GetComponent<ObjetoPasivo>().datosObjeto.Nivel;

            opcionesMejorapasivo[id].datosObjeto = objetoMejorado.GetComponent<ObjetoPasivo>().datosObjeto;

            if (GameManager.instancia != null && GameManager.instancia.mejorandoEquipamiento)
            {
                GameManager.instancia.finMenuMejora();
            }
        }
    }

    void aplicarOpcionesMejora()
    {
        List<MejoraArma> mejorasArmaDisponibles = new List<MejoraArma>(opcionesMejoraArma);
        List<MejoraPasivo> mejorasPasivoDisponibles = new List<MejoraPasivo>(opcionesMejorapasivo);

        foreach (var opcion in opcionesMejoraUI)
        {
            //Si no hay mejoras disponibles salimos
            if(mejorasArmaDisponibles.Count == 0 && mejorasPasivoDisponibles.Count == 0)
            {
                return;
            }

            int tipoMejora;

            if(mejorasArmaDisponibles.Count == 0) 
            {
                tipoMejora = 2;
            }
            else if(mejorasPasivoDisponibles.Count == 0) 
            {
                tipoMejora = 1;
            }
            else
            {
                tipoMejora = Random.Range(1, 3);
            }

            if(tipoMejora == 1)
            {
                MejoraArma mejoraArmaElegida = mejorasArmaDisponibles[Random.Range(0, mejorasArmaDisponibles.Count)];

                mejorasArmaDisponibles.Remove(mejoraArmaElegida);

                if(mejoraArmaElegida != null)
                {

                    activarUIMejoras(opcion);

                    bool nuevaArma = false;
                    for(int i = 0; i < ranurasArmas.Count; i++)
                    {
                        if(ranurasArmas[i] != null && ranurasArmas[i].estadisticas == mejoraArmaElegida.datosArma)
                        {
                            nuevaArma = false;
                            if (!nuevaArma)
                            {
                                //Si no esta implementada en el juego la mejora no hacemos nada
                                if (!mejoraArmaElegida.datosArma.PrefabSiguienteNivel)
                                {
                                    desactivarUIMejoras(opcion);
                                    break;
                                }

                                //Aplicamos el onlcik del boton la funcion mejorar arma usando expresion lambda
                                opcion.botonMejora.onClick.AddListener(() => mejorarArma(i, mejoraArmaElegida.idMejoraArma));

                                //Inicializamos nombre y descripcion con los del prefab del siguiente nivel (mejora)
                                opcion.nombreMejora.text = mejoraArmaElegida.datosArma.PrefabSiguienteNivel.GetComponent<ControladorArmas>().estadisticas.Nombre;
                                opcion.descripcionMejora.text = mejoraArmaElegida.datosArma.PrefabSiguienteNivel.GetComponent<ControladorArmas>().estadisticas.Descripcion;
                            }
                            break;
                        }
                        else
                        {
                            nuevaArma = true;
                        }
                    }
                    if (nuevaArma)
                    {
                        //Aplicamos el onlcik del boton la funcion mejorar arma usando expresion lambda
                        opcion.botonMejora.onClick.AddListener(() => jugador.equiparNuevaArma(mejoraArmaElegida.armaInicial));

                        //inicializamos nombre y descripcion con los atributos de la nueva arma
                        opcion.nombreMejora.text = mejoraArmaElegida.datosArma.Nombre;
                        opcion.descripcionMejora.text = mejoraArmaElegida.datosArma.Descripcion;
                    }

                    opcion.iconoMejora.sprite = mejoraArmaElegida.datosArma.Icono;
                }
            }
            else if(tipoMejora == 2)
            {
                MejoraPasivo mejoraPasivoElegida = mejorasPasivoDisponibles[Random.Range(0, mejorasPasivoDisponibles.Count)];

                mejorasPasivoDisponibles.Remove(mejoraPasivoElegida);

                if(mejoraPasivoElegida != null)
                {
                    activarUIMejoras(opcion);

                    bool nuevoPasivo = false;
                    for(int i = 0; i < ranurasObjetos.Count; i++)
                    {
                        if(ranurasObjetos[i] != null && ranurasObjetos[i].datosObjeto == mejoraPasivoElegida.datosObjeto)
                        {
                            nuevoPasivo = false;

                            if (!nuevoPasivo)
                            {

                                //Si no esta implementada en el juego la mejora no hacemos nada
                                if (!mejoraPasivoElegida.datosObjeto.PrefabSiguienteNivel)
                                {
                                    desactivarUIMejoras(opcion);
                                    break;
                                }

                                opcion.botonMejora.onClick.AddListener(() => mejorarPasivo(i, mejoraPasivoElegida.idMejoraPasivo)); //Aplicamos el onlcik del boton la funcion mejorar arma usando expresion lambda

                                //Inicializamos nombre y descripcion con los del prefab del siguiente nivel (mejora)
                                opcion.nombreMejora.text = mejoraPasivoElegida.datosObjeto.PrefabSiguienteNivel.GetComponent<ObjetoPasivo>().datosObjeto.Nombre;
                                opcion.descripcionMejora.text = mejoraPasivoElegida.datosObjeto.PrefabSiguienteNivel.GetComponent<ObjetoPasivo>().datosObjeto.Descripcion;
                            }
                            break;
                        }
                        else
                        {
                            nuevoPasivo = true;
                        }
                    }
                    if (nuevoPasivo)
                    {
                        opcion.botonMejora.onClick.AddListener(() => jugador.equiparNuevoPasivo(mejoraPasivoElegida.objetoInicial));

                        //inicializamos nombre y descripcion con los atributos de la nueva arma
                        opcion.nombreMejora.text = mejoraPasivoElegida.datosObjeto.Nombre;
                        opcion.descripcionMejora.text = mejoraPasivoElegida.datosObjeto.Descripcion;
                    }

                    opcion.iconoMejora.sprite = mejoraPasivoElegida.datosObjeto.Icono;
                }
            }
        }
    }

    void eliminarOpcionMejora()
    {
        foreach(var opcion in opcionesMejoraUI)
        {
            opcion.botonMejora.onClick.RemoveAllListeners();
            desactivarUIMejoras(opcion);
        }
    }

    void EliminarOpcionesYAplicarMejoras()
    {
        eliminarOpcionMejora();
        aplicarOpcionesMejora();
    }

    public void desactivarUIMejoras(UIMejora ui)
    {
        ui.nombreMejora.transform.parent.gameObject.SetActive(false);
    }

    public void activarUIMejoras(UIMejora ui)
    {
        ui.nombreMejora.transform.parent.gameObject.SetActive(true);
    }
}
