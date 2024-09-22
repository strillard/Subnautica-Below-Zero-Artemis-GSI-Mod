using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Net.Http;
using System;
using System.Reflection;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Linq;

namespace SubnauticaBelowZeroArtemisGSI
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    public class Plugin : BaseUnityPlugin
    {
        private const string PLUGIN_GUID = "00fae7dd-6469-4136-b0c6-34e0ebb067e5";
        private string SERVER_URL;
        public new static ManualLogSource Logger { get; private set; }
        private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

        private GameData gameData = new GameData();

        private HttpClient httpClient;
        private void Awake()
        {
            // set project-scoped logger instance
            Logger = base.Logger;

            try
            {
                SERVER_URL = File.ReadLines(Environment.GetEnvironmentVariable("ProgramData") + "/Artemis/webserver.txt").First();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(SERVER_URL);

            // register harmony patches, if there are any
            Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            Logger.LogInfo($"Artemis server URL is {SERVER_URL}");
        }

        
        private async void Update()
        {
            Player player = Player.main;
            WeatherManager weatherManager = WeatherManager.main;

            if (player)
            {
                gameData.oxygen_available = player.GetOxygenAvailable();
                gameData.oxygen_capacity = player.GetOxygenCapacity();
                gameData.health = player.liveMixin.health;
                gameData.max_health = player.liveMixin.maxHealth;
                gameData.food = player.gameObject.GetComponent<Survival>().food;
                gameData.water = player.gameObject.GetComponent<Survival>().water;
                gameData.body_temperature = 100 - player.GetComponent<Survival>().bodyTemperature.playerColdValue.Value;
                gameData.depth_level = player.depthLevel;
                gameData.biome = player.CalculateBiome();
                gameData.rain_intensity = weatherManager.rainIntensity.Value;
                gameData.snow_intensity = weatherManager.snowIntensity.Value;
                gameData.is_swimming = player.IsSwimming();
                gameData.is_inside = player.IsInside();
                gameData.PDA_state = player.GetPDA().state;


            }
            
            SendGameDataToArtemisApi(gameData);
            
           }

        private void OnApplicationQuit()
        {
            SendGameDataToArtemisApi(new GameData());
        }
        private void SendGameDataToArtemisApi(GameData gameData)
        {
            string json = JsonConvert.SerializeObject(gameData);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("plugins/" + PLUGIN_GUID + "/SubnauticaBelowZero", content);
        }

    }
}