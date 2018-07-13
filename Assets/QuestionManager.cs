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
		private int dificultad;
	}
	[System.Serializable]
	public struct Panel{
		public QuestionPanel panel;
		public bool preguntaAlAzar;
		public int idPregunta;
	}
	[Header("Preguntas (id empieza en 0)")]
	public Pregunta [] preguntas;
	[Header("Paneles")]
	public Panel [] paneles;
	public float currentScore;
	[SerializeField]
	public List<Pregunta> preguntasRestantes;
	System.Random random = new System.Random();

	void Start(){
		preguntasRestantes = preguntas.ToList();
		foreach(Panel p in paneles){
			if(p.panel == null)
				return;
			p.panel.preguntaAlAzar = p.preguntaAlAzar;
			p.panel.idPregunta = p.idPregunta;
			p.panel.CargarPanel();
		}
	}

	public Pregunta getPregunta(){
		int index = random.Next(0,preguntasRestantes.Count);
		Debug.Log(index+" | "+preguntasRestantes.Count);
		//genera copia de respuesta a entregar, la borra del pool de preguntas restantes, y la retorna
		Pregunta aux = preguntasRestantes[index];
		preguntasRestantes.RemoveAt(index);
		if(preguntasRestantes == null || preguntasRestantes.Count <= 0){
			Debug.Log("Recarga");
			preguntasRestantes = preguntas.ToList();
		}
		return aux;	
	}

	public Pregunta getPreguntaId(int id){
		//Obtiene pregunta directo de la lista total por id
		id = Mathf.Clamp(id,0,preguntas.Length-1);
		return preguntas[id];
	}

	[ExecuteInEditMode]
	public void CargarPreguntas(){
		TextAsset asset = (TextAsset)Resources.Load("Preguntas_1");
		string[] fullText = asset.text.Split('\n');
		string [] lineText;
		string [] alternativas;
		List<Pregunta> preguntas = new List<Pregunta>();
		Pregunta p;
		for(int i = 0; i < fullText.Length; i++){
			lineText = fullText[i].Split('|');
			if(lineText[0].StartsWith("//") || lineText[0].Trim() == "")
				continue;
			else{
				p = new Pregunta();
				p.enunciado = lineText[0].Trim();
				alternativas = lineText[1].Split(';');
				for(int j = 0; j < alternativas.Length; j++){
					alternativas[j] = alternativas[j].Trim();
				}
				p.alternativas = alternativas;
				p.respuestaCorrecta = int.Parse(lineText[2].Trim());
				preguntas.Add(p);
			}
		}
		this.preguntas = preguntas.ToArray();
		preguntasRestantes = preguntas.ToList();
	}
}
