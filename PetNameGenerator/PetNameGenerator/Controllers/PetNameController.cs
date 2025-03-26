using Microsoft.AspNetCore.Mvc;

namespace PetNameGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetNameController : ControllerBase
    {
        private readonly string[] dogNames = new string[] 
        { 
            "Labang", "Brownie", "Blacky", "Rocky", "Jumbo"
        };
        private readonly string[] catNames = new string[] 
        { 
            "Mingming", "Mara", "Luna", "NiHao", "FineShyt"
        };
        private readonly string[] birdNames = new string[] 
        { 
            "Rio", "Sky", "Kapitan", "Bacarrat", "Araw" 
        };

        [HttpPost("generate")]
        public IActionResult GeneratePetName([FromBody] PetNameRequest request)
        {
            if (string.IsNullOrEmpty(request.AnimalType))
            {
                return BadRequest(new { error = "The 'animalType' field is required." });
            }

            string[] validAnimalTypes = { "dog", "cat", "bird" };
            if (!validAnimalTypes.Contains(request.AnimalType.ToLower()))
            {
                return BadRequest(new { error = "Invalid animal type. Allowed values: dog, cat, bird." });
            }

            if (request.TwoPart.HasValue && !(request.TwoPart.Value is bool))
            {
                return BadRequest(new { error = "The 'twoPart' field must be a boolean (true or false)." });
            }

            string[] animalNames = request.AnimalType.ToLower() switch
            {
                "dog" => dogNames, "cat" => catNames, "bird" => birdNames, _ => throw new InvalidOperationException("Invalid animal type.")
            };

            Random rnd = new Random();
            string name;
            if (request.TwoPart == true)
            {
                string firstPart = animalNames[rnd.Next(animalNames.Length)];
                string secondPart = animalNames[rnd.Next(animalNames.Length)];
                name = firstPart + secondPart;
            }
            else
            {
                name = animalNames[rnd.Next(animalNames.Length)];
            }

            return Ok(new { name });
        }
    }

    public class PetNameRequest
    {
        public string AnimalType { get; set; }
        public bool? TwoPart { get; set; }
    }
}

