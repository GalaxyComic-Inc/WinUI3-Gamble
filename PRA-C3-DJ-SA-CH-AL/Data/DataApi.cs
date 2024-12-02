using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PRA_C3_DJ_SA_CH_AL.Models;

namespace PRA_C3_DJ_SA_CH_AL.Data
{
    internal class DataApi
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        // Base URL should ideally come from a configuration file or environment variable
        private const string BaseUrl = "http://127.0.0.1:8000";

        static DataApi()
        {
            // Configure the HttpClient (e.g., timeout, default headers)
            _httpClient.Timeout = TimeSpan.FromSeconds(30); // Set a 30-second timeout for requests
        }

        /// <summary>
        /// Generic method to fetch data from an API and deserialize it into the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the data to deserialize into.</typeparam>
        /// <param name="url">The API endpoint URL (relative to the base URL).</param>
        /// <returns>Deserialized data of type <typeparamref name="T"/>.</returns>
        public async Task<T> GetDataAsync<T>(string url)
        {
            try
            {
                // Combine the base URL with the provided endpoint
                var fullUrl = $"{BaseUrl}{url}";

                HttpResponseMessage response = await _httpClient.GetAsync(fullUrl);

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                // Deserialize and return the JSON response
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Allow case-insensitive matching for JSON properties
                });
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception($"HTTP error while accessing {url}: {httpEx.Message}", httpEx);
            }
            catch (JsonException jsonEx)
            {
                throw new Exception($"JSON deserialization error: {jsonEx.Message}", jsonEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error while calling {url}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Fetches a list of matches from the API.
        /// </summary>
        /// <returns>A list of matches.</returns>
        public async Task<List<Matches>> GetMatchesAsync()
        {
            return await GetDataAsync<List<Matches>>("/api/matches");
        }

        /// <summary>
        /// Fetches a list of results from the API.
        /// </summary>
        /// <returns>A list of results.</returns>
        public async Task<List<Results>> GetResultsAsync()
        {
            return await GetDataAsync<List<Results>>("/api/results");
        }

        /// <summary>
        /// Fetches goals for a specific match by ID.
        /// </summary>
        /// <param name="matchId">The ID of the match.</param>
        /// <returns>A list of goals for the match.</returns>
        public async Task<List<Goals>> GetGoalsAsync(int matchId)
        {
            return await GetDataAsync<List<Goals>>($"/api/goals/match_id={matchId}");
        }

        /// <summary>
        /// Adds a custom header to the HttpClient (useful for authentication tokens).
        /// </summary>
        /// <param name="key">The header key.</param>
        /// <param name="value">The header value.</param>
        public void AddHeader(string key, string value)
        {
            if (_httpClient.DefaultRequestHeaders.Contains(key))
            {
                _httpClient.DefaultRequestHeaders.Remove(key);
            }
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }
    }
}
