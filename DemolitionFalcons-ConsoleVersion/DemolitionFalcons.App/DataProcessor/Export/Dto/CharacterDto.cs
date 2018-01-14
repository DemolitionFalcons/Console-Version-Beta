namespace DemolitionFalcons.App.Commands.DataProcessor.Export.Dto
{
    using Newtonsoft.Json;

    public class CharacterDto
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "hero")]
        public string Name { get; set; }
    }
}
