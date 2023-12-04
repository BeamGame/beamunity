using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ZXing.QrCode;
using ZXing;
using UnityEngine.SceneManagement;
using System;

public class Swap : MonoBehaviour
{
    protected VisualElement root;
    protected TextField txtBeamcoin;
    protected Label lblBeamcoin;
    protected Label lblBeam;
    protected Button btnSwap;
    protected Button btnBack;

    protected Label lblState;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        btnSwap = root.Q<Button>("btnSwap");
        btnBack = root.Q<Button>("btnBack");

        lblState = root.Q<Label>("lblState");
        txtBeamcoin = root.Q<TextField>("txtBeamcoin");
        lblBeamcoin = root.Q<Label>("lblBeamcoin");
        lblBeam = root.Q<Label>("lblBeam");
        //   btnSwap.clicked += BtnSwap_clicked;
        btnBack.clicked += BtnBack_clicked;
        //btnCreate.visible = false;
        //txtName.visible = false;


        Load();
    }

    private void BtnSwap_clicked()
    {
        //throw new System.NotImplementedException();
        SceneManager.LoadScene("GamePlay");
    }

    private void BtnBack_clicked()
    {
        SceneManager.LoadScene("GamePlay");
    }

    private async void Load()
    {
        // encode the last found
        try
        {
            var qrCode = await ConnectionService.GetQrCode();
      
        }
        catch (Exception ex)
        {
            lblState.text = "Error on swap retry later";
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

   
}
