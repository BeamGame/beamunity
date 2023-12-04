using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class Login : MonoBehaviour
{
    //public RawImage image;

    // Texture for encoding 
    private Texture2D encoded;
    private string LastResult;
    private bool shouldEncodeNow;

    protected VisualElement root;
    protected Button btnConnect;
    protected Button btnRegister;
    protected TextField txtEmail;
    protected TextField txtPassword;
    protected Label lblError;

    const string username = "username";
    const string password = "password";

    void Start()
    {
        encoded = new Texture2D(512, 512);

        root = GetComponent<UIDocument>().rootVisualElement;
        btnConnect = root.Q<Button>("btnConnect");
        btnRegister = root.Q<Button>("btnRegister");
        txtEmail = root.Q<TextField>("txtEmail");
        txtPassword = root.Q<TextField>("txtPassword");
        lblError = root.Q<Label>("lblError");

        btnConnect.clicked += BtnConnect_clicked;
        btnRegister.clicked += BtnRegister_clicked;

        txtEmail.value = PlayerPrefs.GetString(username, "");
        txtPassword.value = PlayerPrefs.GetString(password, "");

        lblError.text = string.Empty;

    }

    private void BtnRegister_clicked()
    {
        SceneManager.LoadScene("Register");
    }

    private async void BtnConnect_clicked()
    {
        try
        {
            Debug.Log("connected account " + txtEmail.text);

            LoginVM loginVM = new LoginVM() { Email = txtEmail.text, Password = txtPassword.text };
            // get auth infos
            var token = await ConnectionService.GetTokenAsync(loginVM);
            GameContext.Instance.Token = token.Token;
            GameContext.Instance.RefreshToken = token.RefreshToken;
            Debug.Log("Token " + token);
            var player = await ConnectionService.GetUserName();
            Debug.Log($"Player {player.Name}");
            GameContext.Instance.Name = player.Name;

            PlayerPrefs.SetString(password, txtPassword.text);
            PlayerPrefs.SetString(username, txtEmail.text);
            PlayerPrefs.Save();

            SceneManager.LoadScene("GamePlay");
            Debug.Log("Scene loaded");
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
