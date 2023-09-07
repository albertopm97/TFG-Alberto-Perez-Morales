# Trabajo Fin de Grado 
## Generación procedimental de sonido en videojuegos
Autor: Alberto Pérez Morales

Director: Antonio Bautista Bailón Morillas

Este repositorio recoge el código realizado para el trabajo de fin de grado del alumno Alberto Pérez Morales. Consiste en la creación usando Unity y SuperCollider de un generador de musica procedimental.

## Abstract
La generación procedimental es una práctica que permite a los desarrolladores de videojuegos crear gran cantidad de tipos de contenido distintos para su producto sin necesidad de recurrir a la contratación de artistas, ahorrando así tanto en tiempo como en recursos económicos. También consigue que el contenido generado sea más dinámico y permite que se adapte a lo que ocurre en las partidas, mejorando en gran medida la experiencia del jugador y fomentando la rejugabilidad. El impacto de la generación procedimental en el mundo de los videojuegos ha sido tan fuerte que incluso ha provocado la aparición de géneros nuevos que no podrían existir sin ella, como, por ejemplo, el roguelike.

De entre todos los tipos de contenido posibles, uno de los menos explorados, y en el cual vamos a centrar este trabajo, es la generación procedimental de música. A diferencia de otros, la música es un elemento abstracto en el que emitir juicios en cuanto a la calidad, adecuación o adaptabilidad se complican de forma notable. Además, la música cumple un objetivo concreto dentro del juego, que es servir de acompañamiento para el apartado gráfico y la narrativa, induciendo emociones al jugador, lo cual es muy difícil de lograr de forma efectiva usando música generada de manera procedural.

Para la experimentación práctica de este trabajo, presentamos el proceso seguido para implementar un generador de música para un videojuego, usando para ello el entorno y el lenguaje proporcionados por el software de código abierto SuperCollider y el motor de videojuegos Unity. Para desarrollar dicho generador, en primer lugar, se ha implementado un grafo dirigido de transiciones entre acordes, gracias al cual es posible generar secuencias de acordes con sentido haciendo recorridos aleatorios. Después, se han definido sintetizadores y patrones de SuperCollider para la percusión, armonía y melodías y, finalmente, se han parametrizado ciertas variables del juego, como el número de enemigos o los puntos de salud del jugador, para conseguir una adaptación dinámica al estado de la partida.

##
La memoria completa puede consultarse en: https://github.com/albertopm97/TFG-Alberto-Perez-Morales/blob/main/Memoria/TFG_AlbertoPerezMorales.pdf



