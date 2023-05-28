using System.Text;

namespace PlaningPoker.Api.Test.PlaningHub.Fixtures
{
    public class PlaningPokerClient
    {
        private const string MediaType = "application/json";
        private readonly HttpClient client;

        public PlaningPokerClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<string> GetJsonContent(string path)
        {
            using var jsonReaderPost = new StreamReader(path);
            return await jsonReaderPost.ReadToEndAsync();
        }

        public async Task<HttpResponseMessage> Post(string requestUri, string json)
        {
            var responsePost = await client!.PostAsync(requestUri, new StringContent(json,
                Encoding.Default,
                MediaType));
            responsePost.EnsureSuccessStatusCode();
            return responsePost;
        }

        public async Task<HttpResponseMessage> Put(string requestUri, string json)
        {
            var responsePut = await client!.PutAsync(requestUri, new StringContent(json, Encoding.Default,
                MediaType));
            responsePut.EnsureSuccessStatusCode();
            return responsePut;
        }

        public async Task<HttpResponseMessage> Get(string requestUri)
        {
            var response = await client!.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}