using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class ImportJsonCardsDTO
    {
        [Required]
        [RegularExpression("^(\\d{4})\\s(\\d{4})\\s(\\d{4})\\s(\\d{4})$")]
        public string Number { get; set; }
        [JsonProperty("CVC")]
        [Required]
        [RegularExpression("^(\\d{3})$")]
        public string Cvc { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
