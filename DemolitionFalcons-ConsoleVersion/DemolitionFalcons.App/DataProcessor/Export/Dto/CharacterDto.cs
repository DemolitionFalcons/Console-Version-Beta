using Newtonsoft.Json;

namespace DemolitionFalcons.App.Commands.DataProcessor.Export.Dto
{
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
