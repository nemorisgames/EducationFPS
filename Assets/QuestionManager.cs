using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestionManager : MonoBehaviour {

	[System.Serializable]
	public struct Pregunta{
		public string enunciado;
		public string [] alternativas;
		public int respuestaCorrecta;
		public int dificultad;
	}

	public Pregunta [] preguntas;

	List<Pregunta> preguntasRestantes;

	void Start(){
		preguntasRestantes = preguntas.ToList();
	}

	public Pregunta getPregunta(int index = -1){
		//si no quedan preguntas en lista, recarga
		if(preguntasRestantes.Count == 0)
			preguntasRestantes = preguntas.ToList();
		//Default: regresa pregunta al azar de cualquier dificultad
		if(index == -1)
			index = Random.Range(0,preguntasRestantes.Count);
		//No default: regresa pregunta al azar de la dificultad indicada
		else{
			List<Pregunta> auxPreguntas = new List<Pregunta>();
			foreach(Pregunta p in preguntasRestantes)
				if(p.dificultad == index)
					auxPreguntas.Add(p);
			//Indice: al azar desde la nueva lista de preguntas restantes
			Debug.Log(auxPreguntas.Count);
			index = Random.Range(0,auxPreguntas.Count);
		}
		//genera copia de respuesta a entregar, la borra del pool de preguntas restantes, y la retorna
		Pregunta aux = preguntasRestantes[index];
		preguntasRestantes.RemoveAt(index);
		return aux;	
	}

}
