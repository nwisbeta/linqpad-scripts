<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\Accessibility.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Deployment.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.Formatters.Soap.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

//Script to keep only selected resources from a swagger doc generated with fhir swagger (https://www.npmjs.com/package/fhir-swagger)

//List of FHIR Resources to keep
var targetItems = new[] { "DiagnosticReport", "Observation", "Organization", "Patient", "Practitioner", "PractitionerRole" };

//Show Dialog to choose location of swagger doc (in JSON format)
var swaggerPath = string.Empty;
var dialog = new OpenFileDialog { Multiselect = false};
if (dialog.ShowDialog() == DialogResult.OK)
{
	swaggerPath = dialog.FileName;
}
var swagger = JObject.Parse(File.ReadAllText(swaggerPath));

//Remove defintions, paths and tags
var defs = swagger["definitions"].Where(o => !targetItems.Contains((o as JProperty)?.Name)).ToList();
defs.ForEach(p => ((JProperty)p).Remove());

var paths = swagger["paths"].Where(o => !targetItems.Contains((string)(((o as JProperty)?.Value as JObject)["get"]["tags"].First()))).ToList();
paths.ForEach(p => ((JProperty)p).Remove());

var tags = swagger["tags"].Where(o => !targetItems.Contains((string)o["name"])).ToList();
tags.ForEach(i => ((JArray)swagger["tags"]).Remove(i));

Console.WriteLine(swagger.ToString());