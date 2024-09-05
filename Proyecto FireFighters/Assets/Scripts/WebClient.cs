// TC2008B Modelación de Sistemas Multiagentes con gráficas computacionales
// C# client to interact with Python server via POST
// Sergio Ruiz-Loza, Ph.D. March 2021

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Linq;

using Debug = UnityEngine.Debug;
using System.Collections.Specialized;

public class WebClient : MonoBehaviour
{

	string jsonString;
    public GameObject pared;
	public GameObject paredDaniada;
	public GameObject paredDestruida;
	public GameObject puerta;
	public GameObject amarillo;
	public GameObject DescubrirIP;

	public GameObject humo;
	public GameObject fuego;
	public GameObject bombero;
	public GameObject destruir_humo_fuego;

	public GameObject puntoInteres;
	public GameObject BomberoCarga;
	public GameObject BomberoAtaca;
	public GameObject destruirMuro;
	public GameObject destruirIP;
	public GameObject moverBombero;

	public GameObject Carpeta;
	//public Quaternion rotacion;
	public Vector3 pos;
	int[,,] pos_Age;

	public int contador_id = 0;
	public int contador_Size = 0;
	public int contador_index = 0;
	public int contador_Grids = 0;

	GameObject CarpetaVacia;

	public GameObject[] Bomberos = new GameObject[5];



	Class_Paredes paredes_lista;

	// IEnumerator - yield return
	IEnumerator SendData(string data)
    {
		WWWForm form = new WWWForm();
        form.AddField("bundle", "the data");
        string url = "http://localhost:8585";

		using (UnityWebRequest www = UnityWebRequest.Post(url, form))        
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //www.SetRequestHeader("Content-Type", "text/html");
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();          // Talk to Python
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
				Debug.Log(www.downloadHandler.text.Replace('\'', '\"'));
                jsonString = www.downloadHandler.text.Replace('\'', '\"');


				paredes_lista = JsonUtility.FromJson<Class_Paredes>(jsonString);

				string paredes_string = JsonUtility.ToJson(paredes_lista);

				//var resultado = (from lista in paredes_lista.Paredes select lista);

				Invoke("LeerID", 1f);
				
				

			}
		}

    }

    // Start is called before the first frame update
    void Start()
    {
		//string call = "What's up?";
		Vector3 fakePos = new Vector3(3.44f, 0, -15.707f);
        string json = EditorJsonUtility.ToJson(fakePos);
        //StartCoroutine(SendData(call));
        StartCoroutine(SendData(json));
        // transform.localPosition
    }

    // Update is called once per frame
    void Update()
    {

    }

	public void LeerID()
    {
		Debug.Log("Leer");
		Debug.Log(paredes_lista.ID[contador_id]);
		if(paredes_lista.ID[contador_id] == -1)
        {
			contador_id++;
			Invoke("Generar_Paredes", 1f);
        }
		else if (paredes_lista.ID[contador_id] == -2)
		{
			contador_id++;
			Invoke("Generar_BomberoYEntorno", 1f);
		}
		else if (paredes_lista.ID[contador_id] == -3)
		{
			contador_id++;
			Invoke("Generar_Bomberos", 1f);
		}
		else if (paredes_lista.ID[contador_id] == 0)
		{
			contador_id++;
			Invoke("CrearHumo", 1f);
		}
		else if (paredes_lista.ID[contador_id] == 1)
		{
			contador_id++;
			Invoke("PrendeFuego", 1f);
		}
		else if (paredes_lista.ID[contador_id] == 2)
		{
			contador_id++;
			Invoke("MoverBombero", 1f);
		}
		else if (paredes_lista.ID[contador_id] == 3)
		{
			contador_id++;
			Invoke("ExtinguirFuegoHumo", 1f);
		}
		else if (paredes_lista.ID[contador_id] == 4)
		{
			contador_id++;
			Invoke("ActualizarMuro", 1f);
		}
		else if (paredes_lista.ID[contador_id] == 5)
		{
			contador_id++;
			Invoke("BomberoCargando", 1f);
		}
		else if (paredes_lista.ID[contador_id] == 6)
		{
			contador_id++;
			Invoke("RomperUsar", 1f);
		}
		else if (paredes_lista.ID[contador_id] == 7)
		{
			contador_id++;
			Invoke("EliminarIP", 1f);
		}
		else if (paredes_lista.ID[contador_id] == 8)
		{
			contador_id++;
			Invoke("CrearIP", 1f);
		}
		else if (paredes_lista.ID[contador_id] == 9)
		{
			contador_id++;
			Invoke("DescubreIP", 1f);
		}





	}

	public void Generar_Paredes()
	{
		Debug.Log("Generar_Paredes");
		CarpetaVacia = Instantiate(Carpeta, transform.position, Quaternion.identity);
		int[] lista_indices;
		lista_indices = new int[paredes_lista.Size[contador_Size]];

		for (int index = 0; index < ((paredes_lista.Size[contador_Size])); index++) {
			lista_indices[index] = paredes_lista.Index[contador_index];
			Debug.Log(contador_index);
			contador_index++;
		}
		contador_Size++;

		int pos_i;
		pos_i = lista_indices[0];
		Debug.Log(pos_i);
		int val_paredes_i = pos_i;

		int pos_j;
		pos_j = lista_indices[1];
		Debug.Log(pos_j);			
		int val_paredes_j = pos_j;

		int pos_k;
		pos_k = lista_indices[2];
		Debug.Log(pos_k);			
		int val_paredes_k = pos_k;

		for (int i = 0; i < val_paredes_i; i++)
		{
			for (int j = 0; j < val_paredes_j; j++)
			{
				for (int k = 0; k < val_paredes_k; k++)
				{

					if (paredes_lista.Grids[contador_Grids] != 0)
					{
						pos = new Vector3(j * 5f, 1f, i * 5f);
						//rotacion = Quaternion.Euler(0,0,0);
						float rotacionY = 0f;


						if (k == 0)
						{
							rotacionY = 90f;
							pos.z = pos.z - 2f;
						}

						if (k == 1)
						{
							rotacionY = 0f;
							pos.x = pos.x - 2f;
						}

						if (k == 2)
						{
							rotacionY = 90f;
							pos.z = pos.z + 2f;
						}

						if (k == 3)
						{
							rotacionY = 0f;
							pos.x = pos.x + 2f;
						}

						Vector3 rotationVector = new Vector3(0, rotacionY, 0);
						Quaternion rotation = Quaternion.Euler(rotationVector);

						if (paredes_lista.Grids[contador_Grids] == 1)
						{
							Instantiate(pared, pos, rotation, CarpetaVacia.transform);
						}

						if (paredes_lista.Grids[contador_Grids] == 6)
						{
							Instantiate(puerta, pos, rotation, CarpetaVacia.transform);
						}

						if (paredes_lista.Grids[contador_Grids] == 4)
						{
							Instantiate(amarillo, pos, rotation, CarpetaVacia.transform);
						}


					}

					contador_Grids++;
					Debug.Log(contador_Grids);

				}
			}
		}

		LeerID();
	}

	public void Generar_BomberoYEntorno()
	{
		Debug.Log("Generar_BomberoYEntorno");
		int count = 0;

		int contador_i;
		contador_i = paredes_lista.Index[contador_index];
		contador_index++;

		int contador_j;
		contador_j = paredes_lista.Index[contador_index];
		contador_index++;

		contador_Size++;



		for (int i = 0; i < contador_i; i++)
		{
			for (int j = 0; j < contador_j; j++)
			{


				if (paredes_lista.Grids[contador_Grids] != 0)
				{
					pos = new Vector3(j * 5f, 1f, i * 5f);
					//rotacion = Quaternion.Euler(0,0,0);

					Vector3 rotationVector = new Vector3(0, 180, 0);
					Quaternion rotation = Quaternion.Euler(rotationVector);

					if (paredes_lista.Grids[contador_Grids] == 1)
					{
						rotationVector = new Vector3(270, 0, 0);
						rotation = Quaternion.Euler(rotationVector);
						Debug.Log("CrearHumo");
						Instantiate(humo, pos, rotation, CarpetaVacia.transform);
					}

					if (paredes_lista.Grids[contador_Grids] == 2)
					{
						Debug.Log("CrearFuego");
						Instantiate(fuego, pos, rotation, CarpetaVacia.transform);
					}

					if (paredes_lista.Grids[contador_Grids] == 3)
					{
						Debug.Log("CrearBombero");
						Debug.Log(pos);
						Instantiate(bombero, pos, rotation, CarpetaVacia.transform);
					}
					if (paredes_lista.Grids[contador_Grids] == 4 || paredes_lista.Grids[contador_Grids] == 5)
					{
						Debug.Log("CrearPuntosInteres");
						Instantiate(puntoInteres, pos, rotation, CarpetaVacia.transform);
					}

				}

				contador_Grids++;
			}
		}


		LeerID();
	}

	public void Generar_Bomberos()
    {
		int i= paredes_lista.Index[contador_index];
		contador_index++;

		int j = paredes_lista.Index[contador_index];
		contador_index++;

		int id = paredes_lista.Index[contador_index];
		contador_index++;

		pos = new Vector3(j * 5f, 1f, i * 5f);
		//rotacion = Quaternion.Euler(0,0,0);

		Vector3 rotationVector = new Vector3(0, 180, 0);
		Quaternion rotation = Quaternion.Euler(rotationVector);

		Debug.Log(id);

		Bomberos[id] = Instantiate(bombero, pos, rotation);
		//"Bombero_" + id = Instantiate(bombero, pos, rotation, CarpetaVacia.transform);

		contador_Size++;
		LeerID();
	}

	public void CrearHumo()
	{
		int x = paredes_lista.Index[contador_index];
		contador_index++;

		int y = paredes_lista.Index[contador_index];
		contador_index++;

		pos = new Vector3(x * 5f, 1f, y * 5f);

		Debug.Log("CrearHumo");
		Debug.Log(pos);

		Vector3 rotationVector = new Vector3(270, 0, 0);
		Quaternion rotation = Quaternion.Euler(rotationVector);
		Debug.Log("CrearHumo");
		Instantiate(humo, pos, rotation, CarpetaVacia.transform);

		contador_Size++;
		LeerID();
	}

	public void PrendeFuego()
	{
		Debug.Log("PrendeFuego");
		int x = paredes_lista.Index[contador_index];
		contador_index++;

		int y = paredes_lista.Index[contador_index];
		contador_index++;

		pos = new Vector3(x * 5f, 1f, y * 5f);

		Vector3 rotationVector = new Vector3(0, 0, 0);
		Quaternion rotation = Quaternion.Euler(rotationVector);
		Debug.Log("CrearFuego");
		Instantiate(fuego, pos, rotation, CarpetaVacia.transform);

		contador_Size++;
		LeerID();
	}

	public void MoverBombero()
    {
		Debug.Log("MoverBombero");
		/* Posicion Original
		int x = paredes_lista.Index[contador_index];
		contador_index++;

		int y = paredes_lista.Index[contador_index];
		contador_index++;
		*/


		int x_move = paredes_lista.Index[contador_index] * 5;
		contador_index++;

		int y_move = paredes_lista.Index[contador_index] * 5;
		contador_index++;

		int id = paredes_lista.Index[contador_index];
		contador_index++;

		PlayerMovement script_Bomberos = Bomberos[id].GetComponent<PlayerMovement>();
		script_Bomberos.x_towards = x_move;
		script_Bomberos.y_towards = y_move;
		script_Bomberos.movimiento = true;

		contador_Size++;
		LeerID();
	}

	public void ExtinguirFuegoHumo()
	{
		Debug.Log("ExtinguirFuegoHumo");
		int x = paredes_lista.Index[contador_index];
		contador_index++;

		int y = paredes_lista.Index[contador_index];
		contador_index++;

		pos = new Vector3(x * 5f, 1f, y * 5f);

		Vector3 rotationVector = new Vector3(0, 0, 0);
		Quaternion rotation = Quaternion.Euler(rotationVector);
		Instantiate(destruir_humo_fuego, pos, rotation, CarpetaVacia.transform);

		contador_Size++;
		LeerID();
	}

	public void ActualizarMuro()
	{
		Debug.Log("ActualizarMuro");
		int x = paredes_lista.Index[contador_index];

		int y = paredes_lista.Index[contador_index + 1];

		int k = paredes_lista.Index[contador_index + 2];

		pos = new Vector3(x * 5f, 1f, y * 5f);

		//rotacion = Quaternion.Euler(0,0,0);
		float rotacionY = 0f;


		if (k == 0)
		{
			rotacionY = 90f;
			pos.z = pos.z - 2f;
		}

		if (k == 1)
		{
			rotacionY = 0f;
			pos.x = pos.x - 2f;
		}

		if (k == 2)
		{
			rotacionY = 90f;
			pos.z = pos.z + 2f;
		}

		if (k == 3)
		{
			rotacionY = 0f;
			pos.x = pos.x + 2f;
		}


		Vector3 rotationVector = new Vector3(0, 0, 0);
		Quaternion rotation = Quaternion.Euler(rotationVector);
		Debug.Log("CrearDestructor");
		Instantiate(destruirMuro, pos, rotation, CarpetaVacia.transform);

		Invoke("ConstruirMuro", 0.2f);
	}

	public void ConstruirMuro()
    {
		Debug.Log("ConstruirMuro");
		int x = paredes_lista.Index[contador_index];
		contador_index++;

		int y = paredes_lista.Index[contador_index];
		contador_index++;

		int k = paredes_lista.Index[contador_index];
		contador_index++;

		int vidas = paredes_lista.Index[contador_index];
		contador_index++;

		pos = new Vector3(x * 5f, 1f, y * 5f);

		//rotacion = Quaternion.Euler(0,0,0);
		float rotacionY = 0f;


		if (k == 0)
		{
			rotacionY = 90f;
			pos.z = pos.z - 2f;
		}

		if (k == 1)
		{
			rotacionY = 0f;
			pos.x = pos.x - 2f;
		}

		if (k == 2)
		{
			rotacionY = 90f;
			pos.z = pos.z + 2f;
		}

		if (k == 3)
		{
			rotacionY = 0f;
			pos.x = pos.x + 2f;
		}

		Vector3 rotationVector = new Vector3(0, rotacionY, 0);
		Quaternion rotation = Quaternion.Euler(rotationVector);


		Debug.Log("CrearDestructor");
		if(vidas == 2)
        {
			Instantiate(paredDaniada, pos, rotation, CarpetaVacia.transform);
		}
		if (vidas == 3)
		{
			Instantiate(paredDestruida, pos, rotation, CarpetaVacia.transform);
		}

		contador_Size++;
		LeerID();
	}

	public void BomberoCargando()
	{
		Debug.Log("BomberoCargando");

		int id = paredes_lista.Index[contador_index];
		contador_index++;

		GameObject playerRescued = Bomberos[id].transform.GetChild(0).gameObject;
		if (playerRescued.GetComponent<Activado>().estado_Activado == 0)
		{
			playerRescued.SetActive(true);
			playerRescued.GetComponent<Activado>().cambiarEstado();
		}
		else
		{
			playerRescued.GetComponent<Activado>().cambiarEstado();
			playerRescued.SetActive(false);
		}

		contador_Size++;
		LeerID();
	}

	public void RomperUsar()
	{
		Debug.Log("RomperUsar");

		int id = paredes_lista.Index[contador_index];
		contador_index++;

		Bomberos[id].GetComponent<PlayerMovement>().Pateando();

		contador_Size++;
		LeerID();
	}

	public void EliminarIP()
	{
		Debug.Log("EliminarIP");
		int x = paredes_lista.Index[contador_index];
		contador_index++;

		int y = paredes_lista.Index[contador_index];
		contador_index++;

		pos = new Vector3(x * 5f, 1f, y * 5f);

		Vector3 rotationVector = new Vector3(0, 0, 0);
		Quaternion rotation = Quaternion.Euler(rotationVector);
		Debug.Log("CrearFuego");
		Instantiate(destruirIP, pos, rotation, CarpetaVacia.transform);

		contador_Size++;
		LeerID();
	}
	public void CrearIP()
	{
		Debug.Log("CrearIP");
		int x = paredes_lista.Index[contador_index];
		contador_index++;

		int y = paredes_lista.Index[contador_index];
		contador_index++;

		pos = new Vector3(x * 5f, 1f, y * 5f);

		Vector3 rotationVector = new Vector3(0, 0, 0);
		Quaternion rotation = Quaternion.Euler(rotationVector);
		Instantiate(puntoInteres, pos, rotation, CarpetaVacia.transform);

		contador_Size++;
		LeerID();
	}
	public void DescubreIP()
	{
		Debug.Log("DescubreIP");
		int x = paredes_lista.Index[contador_index];
		contador_index++;

		int y = paredes_lista.Index[contador_index];
		contador_index++;

		pos = new Vector3(x * 5f, 1f, y * 5f);

		Vector3 rotationVector = new Vector3(0, 0, 0);
		Quaternion rotation = Quaternion.Euler(rotationVector);
		Instantiate(DescubrirIP, pos, rotation, CarpetaVacia.transform);

		contador_Size++;
		LeerID();
	}
	

}



 [System.Serializable]
public class Class_Paredes
{
	public int[] Grids;
	public int[] Index;
	public int[] Size;
	public int[] ID;
}