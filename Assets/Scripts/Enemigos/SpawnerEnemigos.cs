using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemigos : MonoBehaviour
{
    [System.Serializable]
    //Clase con la información necesaria para gestionar una wave de enemigos
    public class Wave
    {
        public string nombreWave;
        public List<GrupoEnemigos> grupos;  //Lista de grupos de enemigos a spawnear
        public int totalEnemigos; //Numero máximo de enemigos a spawnear
        public float frecuenciaSpawn;
        public int contadorSpawn; //Cuanta de enemigos spawneados hasta el momento en la wave
    }

    [System.Serializable]
    public class GrupoEnemigos
    {
        public string nombre; // nombre de cada enemigo spawneable
        public GameObject prefabEnemigo;   //prefab a spawnear
        public int cantidad;    //cantidad de enemigos de este tipo a spawnear en la wave
        public int contadorSpawn; //Contaador de enemigos de este tipo ya spawneados
    }

    public List<Wave> waves; //lista con todas las oleadas del juego
    public int waveActual; //contador para saber en que wave estamos

    //Para saber la posicion del jugador
    [Header("Posicion Jugador")]
    public Transform jugador;

    //Atributoss del Spawner
    [Header("Atributos del Spawner")]
    float temporizadorSpawn;
    float temporizadorSpawnWaves;
    public float intervaloWave;
    public int maxEnemigosSimultaneos;
    public int enemigosActivos;
    public int miniBossActivos;
    public bool maximoEnemigosAlcanzado = false; // true si hemos llegado al maximo de enemigos que puede haber en el mapa

    [Header("Posiciones de spawn")]
    public List<Transform> puntosSpawn; //Lista de puntos posibles de spawn de enemigos

    // Start is called before the first frame update
    void Start()
    {
        //jugador = FindObjectOfType<EstadisticasJugador>().transform;
        calcularEnemigosSpawneados();
    }

    // Update is called once per frame
    void Update()
    {
        temporizadorSpawnWaves += Time.deltaTime;
        //Si la oleda ha terminado oara comnenzar la siguiente 
        if (waveActual < waves.Count && waves[waveActual].contadorSpawn != 0 && temporizadorSpawnWaves >= intervaloWave)
        {
            temporizadorSpawnWaves = 0f;
            comenzarSiguienteWave();
        } 

        //Vamos sumando tiempo al temporizador hasta que llega a la frecuencia de spawn
        //  En ese caso reseteamos el timer y hacemos spawn de enemigos
        temporizadorSpawn += Time.deltaTime;

        if (temporizadorSpawn >= waves[waveActual].frecuenciaSpawn)
        {
            temporizadorSpawn = 0f;
            spawnEnemigos();
        }
    }

    //Implementacion de Corutina (coroutine) --> sirven para implementar logica basada en tiempo de forma mas eficiente
    // En general para implementacion de funciones asíncronas. En este caso lo usamos para la lógica de las oleadas por tiempo
    void comenzarSiguienteWave()
    {
        /*yield sirve para parar la ejecucion, en este caso por un tiempo específico
        yield return new WaitForSeconds(intervaloWave);*/

        //Esperado el tiempo, si hay más waves nos movemos a la siguiente
        if(waveActual < waves.Count - 1)
        {
            waveActual++;
            calcularEnemigosSpawneados();
        }
    }

    void calcularEnemigosSpawneados()
    {
        int enemigosSpawneados = 0;
        foreach(var grupoEnemigos in waves[waveActual].grupos)
        {
            enemigosSpawneados += grupoEnemigos.cantidad;
        }

        waves[waveActual].totalEnemigos = enemigosSpawneados;
        Debug.LogWarning(enemigosSpawneados);
    }

    void spawnEnemigos()
    {
        //Comprobamos que el numero maximo de enemigos de la wave no se excedaa
        if(waves[waveActual].contadorSpawn < waves[waveActual].totalEnemigos && !maximoEnemigosAlcanzado)
        {
            //para cada grupo de enemigos
            foreach(var grupo in waves[waveActual].grupos)
            {
                //spawneamos enemigo del grupo si no excedemos el nº maximo de enemigos de ese grupo
                if(grupo.contadorSpawn < grupo.cantidad)
                {
                    //Si hemos llegado al limite de enemigos paramos el spawn
                    if(enemigosActivos >= maxEnemigosSimultaneos)
                    {
                        maximoEnemigosAlcanzado = true;
                        return;
                    }

                    //Asignamos el jugador correspondiente al spawner
                    grupo.prefabEnemigo.GetComponent<MovimientoEnemigos>().jugador = jugador;

                    //elegimos un punto al azar de la lista de puntos y spawneamos al enemigo ahí
                    Instantiate(grupo.prefabEnemigo, jugador.position + puntosSpawn[Random.Range(0, puntosSpawn.Count - 1)].position, Quaternion.identity);

                    //Si el enemigo spawneado es miniboss, hacemos llamada a supercollider
                    if (grupo.prefabEnemigo.GetComponent<EstadisticasEnemigos>().estadisticas.EsMiniboss)
                    {
                        //Aumentamos el nº de minibosses activos
                        miniBossActivos++;
                        OSCHandler.Instance.SendMessageToClient("SuperCollider", "/cambioMinibosses", miniBossActivos);
                    }

                    grupo.contadorSpawn++;
                    waves[waveActual].contadorSpawn++;
                    enemigosActivos++;

                    //Llamada para actualizar los enemigos activos en supercollider al hacer spawn
                    OSCHandler.Instance.SendMessageToClient("SuperCollider", "/cambioEnemigosActivos", enemigosActivos);
                }
            }
        }

        //Si los enemigos vivos en el mapa bajan del limite, activamos de nuevo el spawner
        if(enemigosActivos < maxEnemigosSimultaneos)
        {
            maximoEnemigosAlcanzado = false;
        }
    }

    //funcion a llamar cuando se destruya un enemigo cualquiera para bajar el contador global
    public void OnEnemyKilled(bool miniboss)
    {
        enemigosActivos--;

        //Llamada para actualizar los enemigos activos en supercollider al morir
        OSCHandler.Instance.SendMessageToClient("SuperCollider", "/cambioEnemigosActivos", enemigosActivos);

        //Si el enemigo que murió es miniboss restamos 1 al numero de bosses
        if (miniboss)
        {
            miniBossActivos--;

            //Si no quedan minibosses, mandamos a supercollider señal para quitar filtro de resonancia
            if(miniBossActivos == 0)
            {
                OSCHandler.Instance.SendMessageToClient("SuperCollider", "/cambioMinibosses", miniBossActivos);
            }
        }
    }
}
 