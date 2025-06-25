using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    public List<SlotButtonMono> BtnMonos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnEnable()
    {
        for (int i = 0; i < BtnMonos.Count; i++)
        {
            BtnMonos[i].Setup();
        }
    }

}
