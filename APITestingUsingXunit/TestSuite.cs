using APITestingUsingXunit.Constants;
using APITestingUsingXunit.Utils;

namespace APITestingUsingXunit;

public class TestSuite
{

    private readonly ITestOutputHelper output;
    private readonly RestClient _client;

    public TestSuite(ITestOutputHelper output)
    {
        this.output = output;
        _client = new RestClient(ApiConstants.BaseUrl);
    }

    [Fact]
    public void TestGetMethod()
    {
        //Arrange
        //Rest Client     
        var request = TestSuiteUtil.CreateRestRequest("todos/{id}", Method.Get);
        request.AddUrlSegment("id", 3);

        //Act
        var response = _client.Execute(request);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Content);

        output.WriteLine("This is output from {0}", response.Content);

        if (!string.IsNullOrEmpty(response.Content))
        {
            var jsonResponse = JsonConvert.DeserializeObject<GetUserResponse>(response.Content);
            Assert.NotNull(jsonResponse);
            Assert.Equal(1, jsonResponse.userId);
            Assert.Equal(3, jsonResponse.id);
            Assert.Equal("fugiat veniam minus", jsonResponse.title);
            Assert.False(jsonResponse.Completed);
            output.WriteLine($"User ID: {jsonResponse.userId}, User title:{jsonResponse.title}");
        }
        else
        {
            output.WriteLine("Request was not successful. Status code: {0} " + response.StatusCode);
        }
    }

    [Fact]
    public void TestPostMethod()
    {
        var request = TestSuiteUtil.CreateRestRequest("posts", Method.Post);

        var requestBody = new PostUserResponse
        {
            title = "Updated Title",
            body = "Updated Body",
            userId = 1
        };

        request.AddJsonBody(requestBody);
        var response = _client.Execute(request);
        var jsonResponse = JsonConvert.DeserializeObject<PostUserResponse>(response.Content);


        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(jsonResponse);
        Assert.Equal("Updated Title", jsonResponse.title);
        Assert.Equal("Updated Body", jsonResponse.body);
        Assert.Equal(1, jsonResponse.userId);

        int createdId = jsonResponse.id;
        output.WriteLine("Request was successful. createdId: {0} " + createdId);
    }
    [Fact]
    public void TestPutMethod()
    {
        var request = TestSuiteUtil.CreateRestRequest("todos/{id}", Method.Put);
        request.AddUrlSegment("id", 101);


        var requestBody = new PutUserResponse
        {
            id = 101,
            title = "Updated Title",
            completed = true
        };

        request.AddJsonBody(requestBody);
        var response = _client.Execute(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var jsonResponse = JsonConvert.DeserializeObject<PutUserResponse>(response.Content);

        Assert.NotNull(jsonResponse);
        Assert.Equal(requestBody.id, jsonResponse.id); 
        Assert.Equal(requestBody.title, jsonResponse.title); 
        Assert.True(jsonResponse.completed);

    }
    [Fact]
    public void TestDeleteMethod()
    {
        var request = TestSuiteUtil.CreateRestRequest("todos/{id}", Method.Delete);
        request.AddUrlSegment("id", 13);

        // Execute the DELETE request
        var response = _client.Execute(request);

        // Assertions for DELETE request
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }
}