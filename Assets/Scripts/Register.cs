using System.Threading.Tasks;
using Assets.Service;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Web3Unity;
using ZXing;
using ZXing.QrCode;

public class Register : MonoBehaviour
{


    protected VisualElement root;
    protected TextField txtName;
    protected Label lblError;
    protected Button btnCreate;

    void Start()
    {


        root = GetComponent<UIDocument>().rootVisualElement;
        btnCreate = root.Q<Button>("btnCreate");
        txtName = root.Q<TextField>("txtName");
        lblError = root.Q<Label>("lblError");
        btnCreate.clicked += BtnCreate_clicked;
        btnCreate.visible = false;
        txtName.visible = false;

        lblError.text = "Loading player ...";

        GetPlayer();
    }


    private async void BtnCreate_clicked()
    {
        if (!string.IsNullOrWhiteSpace(txtName.text) && txtName.text.Length > 3)
        {
            try
            {
                var player = await RegisterService.CreatePlayer(txtName.text);
                lblError.text = "Player created";
                Debug.Log($"Player {player.Name}");
                GameContext.Instance.Name = player.Name;
                SceneManager.LoadScene("GamePlay");
            }
            catch (System.Exception ex)
            {
                lblError.text = ex.Message;
            }
        }
        else
        {
            lblError.text = "Name must be more than 3 characters";
        }
    }

    private async void GetPlayer()
    {
        try
        {
            var player = await RegisterService.GetPlayer();
            Debug.Log($"Player {player.Name}");
            GameContext.Instance.Name = player.Name;
            SceneManager.LoadScene("GamePlay");
        }
        catch (System.Exception ex)
        {
            btnCreate.visible = true;
            txtName.visible = true;
            Debug.LogWarning(ex.Message);
            lblError.text = "";
        }
    }

    void Update()
    {


    }
}
