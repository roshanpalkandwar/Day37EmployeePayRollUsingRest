using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using RestSharp;

namespace RestSharpTestCase
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string salary { get; set; }
    }
    [TestClass]

    public class RestSharpTestCase
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            client = new RestClient("http://localhost:4000/");
        }

        private IRestResponse getEmployeeList()
        {
            //arrange
            RestRequest request = new RestRequest("/employee", Method.GET);

            //act
            IRestResponse response = client.Execute(request);
            return response;
        }

        [TestMethod]
        public void OnCallingGETApi_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();

            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(5, dataResponse.Count);

            foreach (Employee e in dataResponse)
            {
                Console.WriteLine("id : " + e.id + " name : " + e.name + " salary : " + e.salary);
            }
        }

        [TestMethod]
        public void GivenEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            //arrange 
            RestRequest request = new RestRequest("/employee", Method.POST);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("name", "Ajay");
            jObjectbody.Add("salary", "15000");

            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Ajay", dataResponse.name);
            Assert.AreEqual("15000", dataResponse.salary);
        }


        [TestMethod]
        public void GivenMultipleEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            //arrange 
            RestRequest request = new RestRequest("/employee", Method.POST);

            JObject jObjectbody1 = new JObject();
            JObject jObjectbody2 = new JObject();
            jObjectbody1.Add("name", "Manoj");
            jObjectbody1.Add("salary", "18000");
            jObjectbody2.Add("name", "Vijay");
            jObjectbody2.Add("salary", "17000");

            request.AddParameter("application/json", jObjectbody1, ParameterType.RequestBody);
            request.AddParameter("application/json", jObjectbody2, ParameterType.RequestBody);


            //act
            IRestResponse response = client.Execute(request);
            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Manoj", dataResponse.name);
            Assert.AreEqual("18000", dataResponse.salary);
            Assert.AreEqual("Vijay", dataResponse.name);
            Assert.AreEqual("17000", dataResponse.salary);

        }

        [TestMethod]
        public void GivenEmployee_OnUpdate_ShouldreturnUpdatedEmployee()
        {
            //arrange
            RestRequest request = new RestRequest("/employee/7", Method.PUT);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("name", "Deepak");
            jObjectbody.Add("salary", "16000");

            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);
            //act
            var response = client.Execute(request);

            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Deepak", dataResponse.name);
            Assert.AreEqual("16000", dataResponse.salary);
            Console.WriteLine(response.Content);
        }

        [TestMethod]
        public void GivenEmployeeID_OnDelete_ShouldReturnSuccessStatus()
        {
            //arrange
            RestRequest request = new RestRequest("/employee/7", Method.DELETE);

            //act
            IRestResponse response = client.Execute(request);

            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Console.WriteLine(response.Content);
        }
    }

}