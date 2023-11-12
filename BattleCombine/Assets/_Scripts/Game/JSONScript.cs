using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Linq;
using UnityEditor;
using Assets._Scripts.Game;

public class JSONScript : MonoBehaviour
{
    [SerializeField] private string _health;

    void Start()
    {
        StartCoroutine(SheetData());
    }

    IEnumerator SheetData()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://tools.aimylogic.com/api/googlesheet2json?sheet=Sheet1&id=1RuM2FHpDWz_Z-TVc1mkK1rZ7_TzwvhlNsEq3tvySEI0");
        yield return www.SendWebRequest();
        if(www == null)
        {
            Debug.Log("ERROR");
        }
        else
        {
            string json = www.downloadHandler.text;
            Debug.Log(json);
            JToken jsonString = JToken.Parse(json);
            Debug.Log(jsonString.ToString());

            List<PlayerStats> list = jsonString.ToObject<List<PlayerStats>>(); 
            Debug.Log(list.ToString());

            //JArray w = (JArray)jsonString["Health"].ToString();
        }
        
    }

    //https://tools.aimylogic.com/api/googlesheet2json?sheet=Sheet1&id=1RuM2FHpDWz_Z-TVc1mkK1rZ7_TzwvhlNsEq3tvySEI0
    //https://tools.aimylogic.com/api/googlesheet2json?sheet=Sheet2&id=1RuM2FHpDWz_Z-TVc1mkK1rZ7_TzwvhlNsEq3tvySEI0
}
