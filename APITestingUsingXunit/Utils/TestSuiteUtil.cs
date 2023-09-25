
namespace APITestingUsingXunit.Utils
{
	public class TestSuiteUtil
	{
        public static RestRequest CreateRestRequest(string resource, Method method)
        {
            return new RestRequest(resource, method);
        }
    }
}

