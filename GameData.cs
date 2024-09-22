namespace SubnauticaBelowZeroArtemisGSI
{
    public class GameData
    {
        public GameData() {
            oxygen_available = 0;
            oxygen_available = 0;
            health = 0;
            max_health = 100;
            food = 0;
            water = 0;
            body_temperature = 0;
            depth_level = 0;
            biome = "";
            rain_intensity = 0;
            snow_intensity = 0;
            is_swimming = false;
            is_inside = false;
            PDA_state = PDA.State.Closed;
        }
        public float oxygen_available { get; set; }
        public float oxygen_capacity { get; set; }
        public float health { get; set; }
        public float max_health { get; set; }
        public float food { get; set; }
        public float water { get; set; }
        public float body_temperature { get; set; }
        public float depth_level { get; set; }
        public string biome { get; set; }
        public float rain_intensity { get; set; }
        public float snow_intensity { get; set; }
        public bool is_swimming { get; set; }
        public bool is_inside { get; set; }
        public PDA.State PDA_state { get; set; }
    }

    public enum Biomes{
        // TODO
    }
}
