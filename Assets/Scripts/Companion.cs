using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ZXing.QrCode;
using ZXing;
using UnityEngine.SceneManagement;
using System;

public class Companion : MonoBehaviour
{
    // Texture for encoding 
    private Texture2D encoded;

    protected VisualElement root;
    protected VisualElement imgQrCode;
    protected TextField txtBeamcoin;
    protected Label lblBeamcoin;
    protected Label lblBeam;
    protected Button btnSwap;
    protected Button btnBack;

    protected Label lblState;

    void Start()
    {
        encoded = new Texture2D(512, 512);

        root = GetComponent<UIDocument>().rootVisualElement;
        //btnSwap = root.Q<Button>("btnSwap");
        btnBack = root.Q<Button>("btnBack");

        lblState = root.Q<Label>("lblState");
        /*  txtBeamcoin = root.Q<TextField>("txtBeamcoin");
          lblBeamcoin = root.Q<Label>("lblBeamcoin");
          lblBeam = root.Q<Label>("lblBeam");*/
        imgQrCode = root.Q<VisualElement>("imgQrCode");
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
            var textForEncoding = qrCode.Name;
            if (!string.IsNullOrEmpty(textForEncoding))
            {
                Debug.Log("set image");
                var color32 = Encode(textForEncoding, encoded.width, encoded.height);
                encoded.SetPixels32(color32);
                encoded.Apply();
                imgQrCode.style.backgroundImage = new StyleBackground(encoded);
                lblState.text = "";
            }
        }
        catch (Exception ex)
        {
            lblState.text = "Maybe this account was already linked";
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }
}
