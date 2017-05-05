using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TeamCityQuery
{
	class Program
	{
		static HttpClient client = new HttpClient();
		static void Main(string[] args)
		{
			client.BaseAddress = new Uri("http://build.palaso.org/guestAuth/app/rest/");
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

			DoTheStuff().Wait();
		}

		static async Task DoTheStuff()
		{
			var content = await GetData("buildTypes");
			var doc = XDocument.Parse(content);
			foreach (XElement node in doc.Root.Nodes())
			{
				var id = node.Attribute("id").Value;
				var uriForLastBuild =
					$"builds?locator=buildType:{id},count:1&fields=build(id,buildTypeId,number,status,finishDate)";
				var build = await GetData(uriForLastBuild);
				var dateString = "";
				var buildDoc = XDocument.Parse(build);
				var buildElement = buildDoc.Root.Element("build");
				if (buildElement != null)
				{
					if (buildElement.Attribute("status").Value != "SUCCESS")
					{
						dateString = buildElement.Element("finishDate").Value;
					}
				}
				if (!dateString.StartsWith("2017"))
					Debug.WriteLine(id + "\t" + node.Attribute("name").Value + "\t" + dateString);
			}
		}

		static async Task<string> GetData(string api)
		{
			var response = await client.GetAsync(api);
			return await response.Content.ReadAsStringAsync();
		}
	}

	class Config
	{
		public string id;
		public string name;
	}
}
