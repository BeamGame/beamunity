using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Register : MonoBehaviour
{


    protected VisualElement root;
    protected TextField txtEmail;
    protected TextField txtPassword;
    protected Label lblError;
    protected Button btnCreate;
    protected Button btnBack;

    void Start()
    {


        root = GetComponent<UIDocument>().rootVisualElement;
        btnCreate = root.Q<Button>("btnCreate");
        btnBack = root.Q<Button>("btnBack");
        //txtName = root.Q<TextField>("txtName");
        txtEmail = root.Q<TextField>("txtEmail");
        txtPassword = root.Q<TextField>("txtPassword");
        lblError = root.Q<Label>("lblError");
        btnCreate.clicked += BtnCreate_clicked;
        btnBack.clicked += BtnBack_clicked;
        //btnCreate.visible = false;
        //txtName.visible = false;

        lblError.text = string.Empty;

        
    }

    private void BtnBack_clicked()
    {
        SceneManager.LoadScene("Login");
    }

    private async void BtnCreate_clicked()
    {

        try
        {
            RegisterVM reg = new RegisterVM() { Email = txtEmail.text, Username = txtEmail.text, Password = txtPassword.text };
            var player = await ConnectionService.RegisterUser(reg);
            lblError.text = "Player created";
            Debug.Log($"Player {player.Username}");
            GameContext.Instance.Name = player.Username;
            SceneManager.LoadScene("GamePlay");
        }
        catch (System.Exception ex)
        {
            lblError.text = ex.Message;
        }
    }



    void Update()
    {


    }
}
