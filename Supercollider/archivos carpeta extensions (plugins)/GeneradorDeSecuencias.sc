/**
*  Clase que representa un nodo del grafo de transiciones entre acordes
*/
Nodo {
	var <>nombres = #[];
	var <>acordesPosibles = #[];
	var <>esInicial;
	*new { |nombres, arr, inicial = false|
		^super.new.init(nombres, arr, inicial)
	}

	init {arg nom, arr, ini;
		nombres = nom;
		acordesPosibles = arr;
		esInicial = ini;
	}

	/*
	* Método para comprobar si un acorde es una transición posible de un nodo
	*/
	esAcordePosible{
		arg acorde;

		~encontrado = false;
		for(0, acordesPosibles.size() - 1){
			arg i;

			if((acorde.asString == acordesPosibles[i].asString) && (~encontrado == false)){
				~encontrado = true;
			};
		};

		^~encontrado;
	}

	/*
	* Método para comprobar pertenece a un nodo
	*/
	comprobarPertenencia{
		arg acorde;

		~encontrado = false;
		for(0, nombres.size() - 1){
			arg i;

			if((acorde.asString == nombres[i].asString) && (~encontrado == false)){
				~encontrado = true;
			};
		};
		^~encontrado;
	}
}


/**
* Clase que almacena el grafo de transiciones y y los diccionarios necesarios para
*/
GeneradorSecuencias{
	var <>grafoTransiciones = #[];
	var <>secuenciaGenerada = #[];
	var <>escalas;
	var <>equivalencias;
	*new {
		^super.new.init()
	}

	init {
		escalas = Dictionary();
		equivalencias = Dictionary();
		secuenciaGenerada = [];
	}

	inicializarDiccionarios{

		//Inicializacion del diccionaario de escalas
		escalas.put('C', ["C", "D", "E", "F", "G", "A", "B"]);
		escalas.put('D', ["D", "E", "F#", "G", "A", "B", "C#"]);
		escalas.put('E', ["E", "F#", "G#", "A", "B", "C#", "D#"]);
		escalas.put('F', ["F", "G", "A", "B", "C", "D", "E"]);
		escalas.put('G', ["G", "A", "B", "C", "D", "E", "F#"]);
		escalas.put('A', ["A", "B", "C#", "D", "E", "F#", "G#"]);
		escalas.put('B', ["B", "C#", "D#", "E", "F#", "G#", "A#"]);

		equivalencias.put('I', 0);
		equivalencias.put('II', 1);
		equivalencias.put('III', 2);
		equivalencias.put('IV', 3);
		equivalencias.put('V', 4);
		equivalencias.put('VI', 5);
		equivalencias.put('VII', 6);
	}

	inicializarGrafoTransiciones{

		grafoTransiciones = [];

		~nodo1 = Nodo(['IIIm7b5'], [], true);

		~nodo1.acordesPosibles = ~nodo1.acordesPosibles.add('VIM');
		~nodo1.acordesPosibles = ~nodo1.acordesPosibles.add('VI7');
		~nodo1.acordesPosibles = ~nodo1.acordesPosibles.add('VI9');
		~nodo1.acordesPosibles = ~nodo1.acordesPosibles.add('VIb9');
		~nodo1.acordesPosibles = ~nodo1.acordesPosibles.add('IVM');
		~nodo1.acordesPosibles = ~nodo1.acordesPosibles.add('IV6');
		~nodo1.acordesPosibles = ~nodo1.acordesPosibles.add('IVM7');
		~nodo1.acordesPosibles = ~nodo1.acordesPosibles.add('IVm');
		~nodo1.acordesPosibles = ~nodo1.acordesPosibles.add('IVm6');

		grafoTransiciones = grafoTransiciones.add(~nodo1);

		~nodo2 = Nodo(['I#dim7'], [], true);

		~nodo2.acordesPosibles = ~nodo2.acordesPosibles.add('IIm');
		~nodo2.acordesPosibles = ~nodo2.acordesPosibles.add('IIm7');
		~nodo2.acordesPosibles = ~nodo2.acordesPosibles.add('IIm9');

		grafoTransiciones = grafoTransiciones.add(~nodo2);

		~nodo3 = Nodo(['VIM', 'VI7', 'VI9', 'VIb9'], []);

		~nodo3.acordesPosibles = ~nodo3.acordesPosibles.add('IIm');
		~nodo3.acordesPosibles = ~nodo3.acordesPosibles.add('IIm7');
		~nodo3.acordesPosibles = ~nodo3.acordesPosibles.add('IIm9');

		grafoTransiciones = grafoTransiciones.add(~nodo3);

		~nodo4 = Nodo(['IIm', 'IIm7', 'IIm9'], []);

		~nodo4.acordesPosibles = ~nodo4.acordesPosibles.add('IIIm');
		~nodo4.acordesPosibles = ~nodo4.acordesPosibles.add('IIIm7');
		~nodo4.acordesPosibles = ~nodo4.acordesPosibles.add('VM');
		~nodo4.acordesPosibles = ~nodo4.acordesPosibles.add('V7');
		~nodo4.acordesPosibles = ~nodo4.acordesPosibles.add('V9');
		~nodo4.acordesPosibles = ~nodo4.acordesPosibles.add('V11');
		~nodo4.acordesPosibles = ~nodo4.acordesPosibles.add('V13');
		~nodo4.acordesPosibles = ~nodo4.acordesPosibles.add('IVm7');
		~nodo4.acordesPosibles = ~nodo4.acordesPosibles.add('IIb7');

		grafoTransiciones = grafoTransiciones.add(~nodo4);

		~nodo5 = Nodo(['IIM', 'II7', 'II9', 'IIb9'], []);

		~nodo5.acordesPosibles = ~nodo5.acordesPosibles.add('VM');
		~nodo5.acordesPosibles = ~nodo5.acordesPosibles.add('V7');
		~nodo5.acordesPosibles = ~nodo5.acordesPosibles.add('V9');
		~nodo5.acordesPosibles = ~nodo5.acordesPosibles.add('V11');
		~nodo5.acordesPosibles = ~nodo5.acordesPosibles.add('V13');

		grafoTransiciones = grafoTransiciones.add(~nodo5);


		~nodo6 = Nodo(['IV#m7b5'], []);

		~nodo6.acordesPosibles = ~nodo6.acordesPosibles.add('VM');
		~nodo6.acordesPosibles = ~nodo6.acordesPosibles.add('V7');
		~nodo6.acordesPosibles = ~nodo6.acordesPosibles.add('V9');
		~nodo6.acordesPosibles = ~nodo6.acordesPosibles.add('V11');
		~nodo6.acordesPosibles = ~nodo6.acordesPosibles.add('V13');
		~nodo6.acordesPosibles = ~nodo6.acordesPosibles.add('VIIM');
		~nodo6.acordesPosibles = ~nodo6.acordesPosibles.add('VII7');
		~nodo6.acordesPosibles = ~nodo6.acordesPosibles.add('VII9');
		~nodo6.acordesPosibles = ~nodo6.acordesPosibles.add('VIIb9');

		grafoTransiciones = grafoTransiciones.add(~nodo6);


		~nodo7 = Nodo(['VM', 'V7', 'V9', 'V11', 'V13'], []);

		~nodo7.acordesPosibles = ~nodo7.acordesPosibles.add('IIIm');
		~nodo7.acordesPosibles = ~nodo7.acordesPosibles.add('IIIm7');
		~nodo7.acordesPosibles = ~nodo7.acordesPosibles.add('VIm');
		~nodo7.acordesPosibles = ~nodo7.acordesPosibles.add('VIm7');
		~nodo7.acordesPosibles = ~nodo7.acordesPosibles.add('VIm9');
		~nodo7.acordesPosibles = ~nodo7.acordesPosibles.add('IM');
		~nodo7.acordesPosibles = ~nodo7.acordesPosibles.add('I6');
		~nodo7.acordesPosibles = ~nodo7.acordesPosibles.add('IM7');
		~nodo7.acordesPosibles = ~nodo7.acordesPosibles.add('IM9');
		grafoTransiciones = grafoTransiciones.add(~nodo7);


		~nodo8 = Nodo(['VIIM', 'VII7', 'VII9', 'VIIb9'], []);

		~nodo8.acordesPosibles = ~nodo8.acordesPosibles.add('IIIm');
		~nodo8.acordesPosibles = ~nodo8.acordesPosibles.add('IIIm7');
		grafoTransiciones = grafoTransiciones.add(~nodo8);


		~nodo9 = Nodo(['II#dim7'], []);

		~nodo9.acordesPosibles = ~nodo9.acordesPosibles.add('IIIm');
		~nodo9.acordesPosibles = ~nodo9.acordesPosibles.add('IIIm7');
		grafoTransiciones = grafoTransiciones.add(~nodo9);

		~nodo10 = Nodo(['IIIm', 'IIIm7'], []);

		~nodo10.acordesPosibles = ~nodo10.acordesPosibles.add('IM');
		~nodo10.acordesPosibles = ~nodo10.acordesPosibles.add('I6');
		~nodo10.acordesPosibles = ~nodo10.acordesPosibles.add('IM7');
		~nodo10.acordesPosibles = ~nodo10.acordesPosibles.add('IM9');
		~nodo10.acordesPosibles = ~nodo10.acordesPosibles.add('IVM');
		~nodo10.acordesPosibles = ~nodo10.acordesPosibles.add('IV6');
		~nodo10.acordesPosibles = ~nodo10.acordesPosibles.add('IVM7');
		~nodo10.acordesPosibles = ~nodo10.acordesPosibles.add('IVm');
		~nodo10.acordesPosibles = ~nodo10.acordesPosibles.add('IVm6');
		~nodo10.acordesPosibles = ~nodo10.acordesPosibles.add('VIm');
		~nodo10.acordesPosibles = ~nodo10.acordesPosibles.add('VIm7');
		~nodo10.acordesPosibles = ~nodo10.acordesPosibles.add('VIm9');
		grafoTransiciones = grafoTransiciones.add(~nodo10);

		~nodo11 = Nodo(['V#dim7'], []);

		~nodo11.acordesPosibles = ~nodo11.acordesPosibles.add('VIm');
		~nodo11.acordesPosibles = ~nodo11.acordesPosibles.add('VIm7');
		~nodo11.acordesPosibles = ~nodo11.acordesPosibles.add('VIm9');
		grafoTransiciones = grafoTransiciones.add(~nodo11);

		~nodo12 = Nodo(['VIIm7b5'], []);

		~nodo12.acordesPosibles = ~nodo12.acordesPosibles.add('IIIM');
		~nodo12.acordesPosibles = ~nodo12.acordesPosibles.add('III7');
		~nodo12.acordesPosibles = ~nodo12.acordesPosibles.add('III9');
		~nodo12.acordesPosibles = ~nodo12.acordesPosibles.add('IIIb9');
		grafoTransiciones = grafoTransiciones.add(~nodo12);

		~nodo13 = Nodo(['IIIM', 'III7', 'III9', 'IIIb9'], []);

		~nodo13.acordesPosibles = ~nodo13.acordesPosibles.add('VIm');
		~nodo13.acordesPosibles = ~nodo13.acordesPosibles.add('VIm7');
		~nodo13.acordesPosibles = ~nodo13.acordesPosibles.add('VIm9');
		grafoTransiciones = grafoTransiciones.add(~nodo13);

		~nodo14 = Nodo(['VIm', 'VIm7', 'VIm9'], []);

		~nodo14.acordesPosibles = ~nodo14.acordesPosibles.add('IVM');
		~nodo14.acordesPosibles = ~nodo14.acordesPosibles.add('IV6');
		~nodo14.acordesPosibles = ~nodo14.acordesPosibles.add('IVM7');
		~nodo14.acordesPosibles = ~nodo14.acordesPosibles.add('IVm');
		~nodo14.acordesPosibles = ~nodo14.acordesPosibles.add('IVm6');
		~nodo14.acordesPosibles = ~nodo14.acordesPosibles.add('IIm');
		~nodo14.acordesPosibles = ~nodo14.acordesPosibles.add('IIm7');
		~nodo14.acordesPosibles = ~nodo14.acordesPosibles.add('IIm9');
		grafoTransiciones = grafoTransiciones.add(~nodo14);

		~nodo15 = Nodo(['Vm7'], []);

		~nodo15.acordesPosibles = ~nodo15.acordesPosibles.add('I7');
		~nodo15.acordesPosibles = ~nodo15.acordesPosibles.add('I9');
		~nodo15.acordesPosibles = ~nodo15.acordesPosibles.add('Ib9');
		grafoTransiciones = grafoTransiciones.add(~nodo15);

		~nodo16 = Nodo(['IM', 'I7', 'I9', 'Ib9'], []);

		~nodo16.acordesPosibles = ~nodo16.acordesPosibles.add('IVM');
		~nodo16.acordesPosibles = ~nodo16.acordesPosibles.add('IV6');
		~nodo16.acordesPosibles = ~nodo16.acordesPosibles.add('IVM7');
		~nodo16.acordesPosibles = ~nodo16.acordesPosibles.add('IVm');
		~nodo16.acordesPosibles = ~nodo16.acordesPosibles.add('IVm6');
		grafoTransiciones = grafoTransiciones.add(~nodo16);

		~nodo17 = Nodo(['IVM', 'IV6', 'IVM7', 'IVm', 'IVm6'], []);

		~nodo17.acordesPosibles = ~nodo17.acordesPosibles.add('IM');
		~nodo17.acordesPosibles = ~nodo17.acordesPosibles.add('I6');
		~nodo17.acordesPosibles = ~nodo17.acordesPosibles.add('IM7');
		~nodo17.acordesPosibles = ~nodo17.acordesPosibles.add('IM9');
		~nodo17.acordesPosibles = ~nodo17.acordesPosibles.add('VM');
		~nodo17.acordesPosibles = ~nodo17.acordesPosibles.add('V7');
		~nodo17.acordesPosibles = ~nodo17.acordesPosibles.add('V9');
		~nodo17.acordesPosibles = ~nodo17.acordesPosibles.add('V11');
		~nodo17.acordesPosibles = ~nodo17.acordesPosibles.add('V13');
		~nodo17.acordesPosibles = ~nodo17.acordesPosibles.add('IIm');
		~nodo17.acordesPosibles = ~nodo17.acordesPosibles.add('IIm7');
		~nodo17.acordesPosibles = ~nodo17.acordesPosibles.add('IIm9');
		grafoTransiciones = grafoTransiciones.add(~nodo17);

		~nodo18 = Nodo(['Im6'], []);

		~nodo18.acordesPosibles = ~nodo18.acordesPosibles.add('IIM');
		~nodo18.acordesPosibles = ~nodo18.acordesPosibles.add('II7');
		~nodo18.acordesPosibles = ~nodo18.acordesPosibles.add('II9');
		~nodo18.acordesPosibles = ~nodo18.acordesPosibles.add('IIb9');
		grafoTransiciones = grafoTransiciones.add(~nodo18);

		~nodo19 = Nodo(['IVm7'], []);

		~nodo19.acordesPosibles = ~nodo19.acordesPosibles.add('IM');
		~nodo19.acordesPosibles = ~nodo19.acordesPosibles.add('I6');
		~nodo19.acordesPosibles = ~nodo19.acordesPosibles.add('IM7');
		~nodo19.acordesPosibles = ~nodo19.acordesPosibles.add('IM9');
		grafoTransiciones = grafoTransiciones.add(~nodo19);

		~nodo20 = Nodo(['IIb7'], []);

		~nodo20.acordesPosibles = ~nodo20.acordesPosibles.add('IM');
		~nodo20.acordesPosibles = ~nodo20.acordesPosibles.add('I6');
		~nodo20.acordesPosibles = ~nodo20.acordesPosibles.add('IM7');
		~nodo20.acordesPosibles = ~nodo20.acordesPosibles.add('IM9');
		grafoTransiciones = grafoTransiciones.add(~nodo20);

		~nodo21 = Nodo(['IM', 'I6', 'IM7', 'IM9'], []);

		~nodo21.acordesPosibles = ~nodo21.acordesPosibles.add('IVM');
		~nodo21.acordesPosibles = ~nodo21.acordesPosibles.add('IV6');
		~nodo21.acordesPosibles = ~nodo21.acordesPosibles.add('IVM7');
		~nodo21.acordesPosibles = ~nodo21.acordesPosibles.add('IVm');
		~nodo21.acordesPosibles = ~nodo21.acordesPosibles.add('IVm6');
		~nodo21.acordesPosibles = ~nodo21.acordesPosibles.add('VIM');
		~nodo21.acordesPosibles = ~nodo21.acordesPosibles.add('VI7');
		~nodo21.acordesPosibles = ~nodo21.acordesPosibles.add('VI9');
		~nodo21.acordesPosibles = ~nodo21.acordesPosibles.add('VIb9');
		~nodo21.acordesPosibles = ~nodo21.acordesPosibles.add('IIM');
		~nodo21.acordesPosibles = ~nodo21.acordesPosibles.add('II7');
		~nodo21.acordesPosibles = ~nodo21.acordesPosibles.add('II9');
		~nodo21.acordesPosibles = ~nodo21.acordesPosibles.add('IIb9');
		grafoTransiciones = grafoTransiciones.add(~nodo21);

		//NO ESTA IMPLEMENTADO EL ULTIMO NODO C... (SE SUPONE QUE EN LOS OTROS YA CONOCEMOS LOS ACORDES YA QUE ES EL NODO FINAL. ANTE FALLO METERLO)
	}

	/**
	* Método para generar una secuencia de acordes de un tamaño específico
	* param: numAcordes: tamaño de la secuencia
	*/
	generarSecuencia{

		arg numAcordes;

		secuenciaGenerada = [];
		~nodoActual = grafoTransiciones.choose;
		~acordeActual = ~nodoActual.nombres.choose;
		secuenciaGenerada  = secuenciaGenerada.add(~acordeActual);

		/*--------------BUCLE DE BUSQUEDA DE ACORDES -----------------------*/
		for(1, numAcordes){ //Puesto que ya hemos añadido el acorde inicial al principio
			~encontrado = false;

			~siguienteAcorde = ~nodoActual.acordesPosibles.choose;
			while{~encontrado == false}{

				~nodoPosible = grafoTransiciones.choose;
				if(~nodoPosible.comprobarPertenencia(~siguienteAcorde) == true){
					~encontrado = true;
					~nodoActual = ~nodoPosible;
					secuenciaGenerada  = secuenciaGenerada.add(~siguienteAcorde);
				};
			};
		};
	}

	/**
	* Método para transformar la secuencia generada de grados genéricos a notas de una escala concreta usando los diccionarios
	* param: la escala a aplicar
	*/
	transformarSecuecia{

		arg escala;

		//Primero pasamos cada elemento a string para poder procesarlo como cadena de texto
		secuenciaGenerada.do{
			arg item, i;
			//secuencia

			secuenciaGenerada[i] = item.asString;
		};

		//Funcion que encapsula el cambio de grados por notas usando los diccionarios
		~cambiar = {
			arg esc;
			secuenciaGenerada.do{
				arg item, i;
				//secuencia
				secuenciaGenerada[i] = secuenciaGenerada[i].replace("VII", escalas.at(esc)[equivalencias.at(\VII)]);
				secuenciaGenerada[i] = secuenciaGenerada[i].replace("VI", escalas.at(esc)[equivalencias.at(\VI)]);
				secuenciaGenerada[i] = secuenciaGenerada[i].replace("IV", escalas.at(esc)[equivalencias.at(\IV)]);
				secuenciaGenerada[i] = secuenciaGenerada[i].replace("V", escalas.at(esc)[equivalencias.at(\V)]);
				secuenciaGenerada[i] = secuenciaGenerada[i].replace("III", escalas.at(esc)[equivalencias.at(\III)]);
				secuenciaGenerada[i] = secuenciaGenerada[i].replace("II", escalas.at(esc)[equivalencias.at(\II)]);
				secuenciaGenerada[i] = secuenciaGenerada[i].replace("I", escalas.at(esc)[equivalencias.at(\I)]);

                //Si hemos metido un # extra lo quitamos
                secuenciaGenerada[i] = secuenciaGenerada[i].replace("##", "#");

                //Si en el proceso hemos metido seguidos # y b debemos quitar el #
                secuenciaGenerada[i] = secuenciaGenerada[i].replace("#b", "b");
			};
		};

		//aplicamos la funcion para transformar los acordes
		~cambiar.value(escala);

		//Finalmente volvemos a combertir los acordes en simbolos para que funcionenen en el sistema
		secuenciaGenerada.do{
			arg item, i;
			//secuencia

			secuenciaGenerada[i] = item.asSymbol;
		};
	}
}