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
    private static string serviceUrl = "https://localhost:7171/";


    static ConnectionService()
    {
        serviceUrl = "https://beam-server.azurewebsites.net/";

#if UNITY_EDITOR
        serviceUrl = "https://localhost:7171/";
#endif

    }

    public static async Task<TokenResponse> GetTokenAsync(LoginVM loginInfo)
    {
        Debug.Log("GetTokenAsync");
        var url = serviceUrl + "api/account/login";
        var response = await UnityRequestClient.Post<TokenResponse>(url, loginInfo);
        Debug.Log("token created");
        return response;
    }

    public static async Task<PlayerName> GetUserName()
    {
        Debug.Log("get player name");
        var url = serviceUrl + "api/login";
        var response = await UnityRequestClient.Get<PlayerName>(url);
        Debug.Log("name" + response.Name);
        return response;
    }

    public static async Task<PlayerName> GetQrCode()
    {
        Debug.Log("get player name");
        var url = serviceUrl + "api/login/ConnectionRequest";
        var response = await UnityRequestClient.Get<PlayerName>(url);
        Debug.Log("name" + response.Name);
        return response;
    }


    public static async Task<BalanceDto> GetBalance()
    {
        Debug.Log("GetBalance");
        var url = serviceUrl + "api/swap";
        var response = await UnityRequestClient.Get<BalanceDto>(url);
        Debug.Log("balnce" + response.BeamonCoin);
        return response;
    }

    public static async Task<decimal> GetPrice(decimal priceIn)
    {
        Debug.Log("GetPrice");
        var url = serviceUrl + "api/swap/Price";
        var response = await UnityRequestClient.Get<decimal>(url + "?price=" + priceIn);
        Debug.Log("price" + response);
        return response;
    }

    public static async Task Swap(BalanceDto dto)
    {
        Debug.Log("Swap");
        var url = serviceUrl + "api/swap";
        await UnityRequestClient.Post<BalanceDto>(url, dto);
    }


    public static async Task<RegisterVM> RegisterUser(RegisterVM loginInfo)
    {
        Debug.Log("get player name");
        var url = serviceUrl + "api/account/register";
        await UnityRequestClient.Post(url, loginInfo);
        LoginVM loginVM = new LoginVM() { Email = loginInfo.Email, Password = loginInfo.Password };
        // get auth infos
        var token = await GetTokenAsync(loginVM);
        GameContext.Instance.Token = token.Token;
        GameContext.Instance.RefreshToken = token.RefreshToken;
        return loginInfo;
    }

    public static async Task<PlayerName> UpdateUsername(PlayerName loginInfo)
    {
        Debug.Log("update player name");
        var url = serviceUrl + "api/login/update";
        var player = await UnityRequestClient.Post<PlayerName>(url, loginInfo);
        var token = await RefreshToken();
        GameContext.Instance.Token = token.Token;
        GameContext.Instance.RefreshToken = token.RefreshToken;
        return player;
    }

    public static async Task<MessageVM> RequestConnection(string account)
    {
        Debug.Log("GetTokenAsync");

        var url = $"{serviceUrl}/RequestConnection?account={account}";

        return await UnityRequestClient.Get<MessageVM>(url);
    }

    public static async Task<TokenResponse> RefreshToken()
    {
        Debug.Log("RefreshToken");
        var url = serviceUrl + "api/account/refresh";
        var player = await UnityRequestClient.Post<TokenResponse>(url, new TokenResponse() { RefreshToken = GameContext.Instance.RefreshToken });
        return player;
    }
}
