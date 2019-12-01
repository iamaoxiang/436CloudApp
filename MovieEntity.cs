using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Query
{
	internal class MovieEntity : TableEntity
	{
        public string Query;
		public string Title;
		public string Data;
        public string PosterUrl;

        public MovieEntity()
		{
		}

		public MovieEntity(string query, string json)
		{
			Parse(query, json);
		}

		public void Parse(string query, string json)
		{
            JObject o = JObject.Parse(json);

            Query = query.Trim();
            Data = json;
            if (!o.Property("Response").Value.Value<bool>())
            {
                Title = "Movie not found!";
                PosterUrl = "";
            }
            else
            {
                Title = o.Property("Title").Value.ToString();
                PosterUrl = o.Property("Poster").Value.ToString();
            }

            // required properties
            PartitionKey = Query;
            RowKey = Title;
            ETag = "*"; // always overwrite
		}

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            IDictionary<string, EntityProperty> dict = base.WriteEntity(operationContext);
            dict.Add("Query", new EntityProperty(this.Query));
            dict.Add("Title", new EntityProperty(this.Title));
            dict.Add("Data", new EntityProperty(this.Data));
            dict.Add("PosterUrl", new EntityProperty(this.PosterUrl));
            return dict;
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
             foreach (var kvp in properties)
            {
                if (kvp.Key == "Title")
                {
                    this.Title = kvp.Value.StringValue;
                }
                else if (kvp.Key == "Data")
                {
                    this.Data = kvp.Value.StringValue;
                }
                else if (kvp.Key == "Query")
                {
                    this.Query = kvp.Value.StringValue;
                }
                else if (kvp.Key == "PosterUrl")
                {
                    this.PosterUrl = kvp.Value.StringValue;
                }
            }
            base.ReadEntity(properties, operationContext);
        }


        public override string ToString()
        {
            return Data.Trim();
        }
    }
}