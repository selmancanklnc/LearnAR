using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class kapat : MonoBehaviour
{
    private Button kapatbutonu;
    // Start is called before the first frame update
    void Start()
    {
        kapatbutonu = GetComponent<Button>();
        kapatbutonu.onClick.AddListener(Kapat);
    }

    

    private void Kapat()
    {
       Application.Quit();
    }
}
