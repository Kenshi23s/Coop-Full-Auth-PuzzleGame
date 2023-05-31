using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ExampleSerialization : MonoBehaviour
{
    //path donde se guarda la info
    [SerializeField] string path = "Assets/Serializacion/Data/";
    [SerializeField] string fileName = "MyData";

    [SerializeField] string username;
    [SerializeField] string password;


    public UserInfo myInfo;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            BinarySerialize();
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            BinaryDeserialize();
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            JSONSerialize();
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            JSONDeserialize();
        }
    }


    void BinarySerialize()
    {
        //creamos ref a formateador (el q traspasa la info original a binary)
        BinaryFormatter bf = new BinaryFormatter();

        //creamos el archivo en el path 
        FileStream file = File.Create(path + fileName + ".bin");

        //serializamos
        bf.Serialize(file, myInfo.username);
        bf.Serialize(file, myInfo.password);

        //cerramos el archivo
        file.Close();

        Debug.Log("Serializacion binaria completa");
    }

    void BinaryDeserialize()
    {
        //si no existe el archivo, corta 
        if (!File.Exists(path + fileName + ".bin")) return;

        //esto tmb se puede crear desde afuera
        BinaryFormatter bf = new BinaryFormatter();

        //levantamos el archivo
        FileStream file = File.Open(path + fileName + ".bin", FileMode.Open);

        //Deserializamos
        username = (string)bf.Deserialize(file);
        password = (string)bf.Deserialize(file);

        //Cerramos el archivo
        file.Close();

        Debug.Log("Deserializacion binaria completa");
    }

    void JSONSerialize()
    {
        //creamos el archivo en el path
        StreamWriter file = File.CreateText(path + fileName + ".json");

        //pasamos la info a string(json)
        string json = JsonUtility.ToJson(myInfo, true);

        Debug.Log(json);

        //guardamos la info en el file
        file.Write(json);

        file.Close();
        Debug.Log("Serializacion JSON completa");
    }

    void JSONDeserialize()
    {
        string finalPath = path + fileName + ".json";

        if (!File.Exists(finalPath)) return;

        //tomo el texto dela rchivo
        string json = File.ReadAllText(finalPath);
        
        //paso el texto a la clase
        myInfo = JsonUtility.FromJson<UserInfo>(json);               

        Debug.Log("Deserializacion JSON completa");
    }

}


