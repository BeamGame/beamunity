using System.Collections.Generic;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace Assets.Service
{
    public class MonsterService
    {
        private static string serviceUrl = "https://localhost:7171/api/monster";


        static MonsterService()
        {
            serviceUrl = "https://beam-server.azurewebsites.net/api/monster";

#if UNITY_EDITOR
            serviceUrl = "https://localhost:7171/api/monster";
#endif

        }
        public static async Task<List<TokenDto>> MintStarter()
        {
            Debug.Log("MintStarter");
            var url = serviceUrl + "/MintStarter";
            return await UnityRequestClient.Post<List<TokenDto>>(url);
        }

        public static async Task<List<TokenDto>> MintMonster(AddMonsterDto monsterInfo)
        {
            Debug.Log("MintMonster");
            var url = serviceUrl + "/MintMonster";
            return await UnityRequestClient.Post<List<TokenDto>>(url, monsterInfo);
        }

        public static async Task<bool> UpdateMonster(UpdateMonsterDto monsterInfo)
        {
            Debug.Log("UpdateMonster");
            var url = serviceUrl + "/UpdateMonster";
            return await UnityRequestClient.Post<bool>(url, monsterInfo);
        }

        public static async Task<List<TokenDto>> GetMonsters()
        {
            Debug.Log("GetMonsters");
            var url = serviceUrl + "/GetMonsters";
            return await UnityRequestClient.Get<List<TokenDto>>(url);
        }

        public static async Task<bool> HaveStarter()
        {
            Debug.Log("HaveStarter");
            var url = serviceUrl + "/HaveStarter";
            return await UnityRequestClient.Get<bool>(url);
        }

        public static async Task<List<TokenDto>> TransferMonster(int tokenId, string playerName)
        {
            Debug.Log("TransferMonster");
            var url = serviceUrl + "/TransferMonsters";
            var monsterInfo = new TransferMonsterDto()
            {
                MonsterId = tokenId,
                UserName = playerName
            };
            return await UnityRequestClient.Post<List<TokenDto>>(url, monsterInfo);
        }
    }
}
