using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;


class LoginRequest
{
    /// <summary>
    /// The user's email address which acts as a user name.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The user's password.
    /// </summary>
    public string Password { get; set; }

}

public class ConnectionService
{
    private static string serviceUrl = "https://localhost:7062/api/login";


    static ConnectionService()
    {
        serviceUrl = "https://beam-server.azurewebsites.net/api/account/login";

#if UNITY_EDITOR
        serviceUrl = "https://localhost:7171/api/account/login";
#endif

    }

    public static async Task<string> GetTokenAsync(LoginVM loginInfo)
    {
        Debug.Log("GetTokenAsync");
        var url = serviceUrl;
        var response = await UnityRequestClient.Post<TokenResponse>(url, loginInfo);
        Debug.Log("token created");
        return response.Token;
    }

    public static async Task<MessageVM> RequestConnection(string account)
    {
        Debug.Log("GetTokenAsync");

        var url = $"{serviceUrl}/RequestConnection?account={account}";

        return await UnityRequestClient.Get<MessageVM>(url);
    }
}
