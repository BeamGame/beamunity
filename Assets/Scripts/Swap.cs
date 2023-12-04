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
    protected Label lblAddress;
    protected Button btnPrice;
    protected Button btnSwap;
    protected Button btnBack;

    protected Label lblState;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        btnSwap = root.Q<Button>("btnSwap");
        btnBack = root.Q<Button>("btnBack");
        btnPrice = root.Q<Button>("btnPrice");

        lblState = root.Q<Label>("lblState");
        txtBeamcoin = root.Q<TextField>("txtBeamcoin");
        lblBeamcoin = root.Q<Label>("lblBeamcoin");
        lblBeam = root.Q<Label>("lblBeam");
        lblAddress = root.Q<Label>("lblAddress");
        btnSwap.clicked += BtnSwap_clicked;
        btnBack.clicked += BtnBack_clicked;
        btnPrice.clicked += BtnPrice_clicked;
        //btnCreate.visible = false;
        //txtName.visible = false;
        lblState.text = string.Empty;


        Load();
    }

    private async void BtnPrice_clicked()
    {
        lblState.text = "Get price ...";
        decimal res = await ConnectionService.GetPrice(decimal.Parse(txtBeamcoin.text));
        lblState.text = $"You will get {res} BEAM";
    }

    private async void BtnSwap_clicked()
    {
        try
        {
            lblState.text = "Swaping ...";
            BalanceDto balanceDto = new BalanceDto() { BeamonCoin = decimal.Parse(txtBeamcoin.text), Address = "0x0", Native = 0.00001m };
            await ConnectionService.Swap(balanceDto);
        }
        catch (System.Exception ex)
        {
            lblState.text = "Error on swap retry later";
        }
        finally
        {
            Load();
        }
        //throw new System.NotImplementedException();
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
            var balance = await ConnectionService.GetBalance();

            lblBeamcoin.text = "Balance " + balance.BeamonCoin.ToString() + " BMC";
            lblBeam.text = "Balance " + balance.Native.ToString() + " BEAM";
            lblAddress.text = "Public Address " + balance.Address;
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
