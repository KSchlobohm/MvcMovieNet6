using System;
using Newtonsoft.Json;
using WpfMovie.Models;

namespace WpfMovie.Services
{
    /// <summary>
    /// Provides serialization/deserialization of <see cref="Movie"/> objects using JSON (Newtonsoft.Json)
    /// and Base64 encoding of the UTF8 JSON payload to mirror the API of MovieStateManager.
    /// </summary>
    public class JsonMovieStateManager
    {
        private readonly JsonSerializerSettings _settings;

        public JsonMovieStateManager()
        {
            _settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            };
        }

        /// <summary>
        /// Serializes the movie to JSON then Base64 encodes the UTF8 bytes of that JSON string.
        /// </summary>
        public string Serialize(Movie anyMovie)
        {
            if (anyMovie == null) throw new ArgumentNullException(nameof(anyMovie));
            var json = JsonConvert.SerializeObject(anyMovie, _settings);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Recreates a Movie instance from a Base64 encoded JSON string.
        /// </summary>
        public Movie Deserialize(string movieString)
        {
            if (string.IsNullOrWhiteSpace(movieString)) throw new ArgumentException("Value cannot be null or empty", nameof(movieString));
            var bytes = Convert.FromBase64String(movieString);
            var json = System.Text.Encoding.UTF8.GetString(bytes);
            var movie = JsonConvert.DeserializeObject<Movie>(json, _settings);
            if (movie == null)
            {
                throw new InvalidOperationException("Deserialization produced a null Movie instance.");
            }
            return movie;
        }
    }
}
