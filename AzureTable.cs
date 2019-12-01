using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Query
{
	internal class AzureTable : AzureStorage
	{
        private string tableName;

		private static CloudTableClient _client = null;
		internal CloudTableClient Client
		{
			get
			{
				if (_client == null)
				{
					_client = Account.CreateCloudTableClient();
				}
				return _client;
			}
		}


		private static CloudTable _table = null;
		internal CloudTable Table
		{
			get
			{
				if (_table == null)
				{
					_table = Client.GetTableReference(tableName);
					RetryCreateIfNotExist(_table);
                }
                return _table;
			}
		}

        public AzureTable(string tableName = "hw436p4table")
        {
            this.tableName = tableName;
        }

        public bool Initialized
        {
            get { return _table != null; }
        }


        public void Insert(MovieEntity item)
		{	
			// Create the TableOperation that inserts the entity.
			var insertOperation = TableOperation.InsertOrReplace(item);

			// Execute the insert operation.
			var result = Table.Execute(insertOperation);

			// check inserted MovieEntity
			MovieEntity inserted = result.Result as MovieEntity;
		}

		public void Delete()
		{
			if (_table != null)
				_table.DeleteIfExists();
            _table = null;
        }

		// if you just deleted, CreateIfNotExists throws.  So retry until previous delete finished
		private bool RetryCreateIfNotExist(CloudTable table)
		{
			do
			{
				try
				{
					return table.CreateIfNotExists();
				}
				catch (StorageException e)
				{
					if (e.RequestInformation.HttpStatusCode == 409)
						Thread.Sleep(1000);
					else
						throw;
				}
			} while (true);
		}


		public List<MovieEntity> Query(string title)
		{
			TableQuery<MovieEntity> query = null;
			string filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, title.Trim());
			query = new TableQuery<MovieEntity>().Where(filter);

			var result = Table.ExecuteQuery(query);

			return result.ToList();
		}

	}
}