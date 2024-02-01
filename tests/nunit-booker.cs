using System.Diagnostics;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace NUnitBooker.Tests
{
    [TestFixture]
    public class BookerApiTests
    {
        private RestClient _client;
        private const string BASE_URL = "https://restful-booker.herokuapp.com";

        [SetUp]
        public void Setup()
        {
            _client = new RestClient(BASE_URL);
        }

        private string Autenticacao()
        {
            var request = new RestRequest("/auth", Method.Post);

            string jsonBody = File.ReadAllText(@"C:\Iterasys\TesteDeApi\fixtures\auth.json");

            request.AddBody(jsonBody);

            var response = _client.Execute(request);

            var responseBody = JsonConvert.DeserializeObject<dynamic>(response.Content);

            string token = responseBody.token.ToString();
            Console.WriteLine(token);
            return token;
        }

        [Test, Order(1)]
        public void TestCreateToken()
        {
            var request = new RestRequest("/auth", Method.Post);

            string jsonBody = File.ReadAllText(@"C:\Iterasys\TesteDeApi\fixtures\auth.json");

            request.AddBody(jsonBody);

            var response = _client.Execute(request);

            var responseBody = JsonConvert.DeserializeObject<dynamic>(response.Content);

            Assert.AreEqual(200, (int)response.StatusCode);

            string token = responseBody.token.ToString();
            Assert.IsNotNull(responseBody.token, "A resposta da API não contém o token.");

        }

        [Test, Order(2)]
        public void TestCreateBooking()
        {
            var request = new RestRequest("/booking", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(File.ReadAllText(@"C:\Iterasys\TesteDeApi\fixtures\create.json"));

            var response = _client.Execute(request);

            var responseBody = JsonConvert.DeserializeObject<dynamic>(response.Content);

            Assert.AreEqual(200, (int)response.StatusCode);

            string firstName = responseBody.booking.firstname.ToString();
            Assert.That(firstName, Is.EqualTo("Nathanielly"));

            Assert.IsNotNull(responseBody.bookingid, "A resposta da API não contém o ID.");

        }

        [Test, Order(3)]
        public void TestGetBookingId()
        {
            var request = new RestRequest("/booking", Method.Get);

            var response = _client.Execute(request);

            var responseBody = JsonConvert.DeserializeObject<dynamic>(response.Content);

            Assert.AreEqual(200, (int)response.StatusCode);

            Assert.IsNotNull(responseBody, "A resposta da API é nula.");
        }

        [Test, Order(4)]
        public void TestGetBooking()
        {
            var request = new RestRequest("/booking/2668", Method.Get);
            request.AddHeader("Accept", "application/json");

            var response = _client.Execute(request);

            var responseBody = JsonConvert.DeserializeObject<dynamic>(response.Content);

            
            Assert.AreEqual(200, (int)response.StatusCode);

            string firstName = responseBody.firstname.ToString();
            Assert.That(firstName, Is.EqualTo("Nathanielly"));
        }

        [Test, Order(5)]
        public void TestUpdateBooking()
        {
            string token = Autenticacao();

            var request = new RestRequest("/booking/2668", Method.Put);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Cookie", $"token={token}");
            request.AddJsonBody(File.ReadAllText(@"C:\Iterasys\TesteDeApi\fixtures\update.json"));

            var response = _client.Execute(request);

            var responseBody = JsonConvert.DeserializeObject<dynamic>(response.Content);

            
            Assert.AreEqual(200, (int)response.StatusCode);

            string checkin = responseBody.bookingdates.checkin.ToString();
            Assert.That(checkin, Is.EqualTo("2024-01-23"));
        }

        [Test, Order(6)]
        public void TestPartialUpdateBooking()
        {
            string token = Autenticacao();

            var request = new RestRequest("/booking/2668", Method.Patch);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Cookie", $"token={token}");
            request.AddJsonBody(File.ReadAllText(@"C:\Iterasys\TesteDeApi\fixtures\partialUpdate.json"));

            var response = _client.Execute(request);

            var responseBody = JsonConvert.DeserializeObject<dynamic>(response.Content);

            
            Assert.AreEqual(200, (int)response.StatusCode);

            string lastName = responseBody.lastname.ToString();
            Assert.That(lastName, Is.EqualTo("Martins"));
        }

        [Test, Order(7)]
        public void TestDeleteBooking()
        {
            string token = Autenticacao();

            var request = new RestRequest("/booking/2668", Method.Delete);

            request.AddHeader("Cookie", $"token={token}");

            var response = _client.Execute(request);
            
            Assert.AreEqual(201, (int)response.StatusCode);

        }


    }
}
