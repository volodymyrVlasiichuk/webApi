using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace WebAPIBlog.Domain
{
	sealed class StringObjectIdConvention : ConventionBase, IPostProcessingConvention
	{
		public void PostProcess(BsonClassMap classMap)
		{
			var idMap = classMap.IdMemberMap;
			if (idMap != null && idMap.MemberName == "Id" && idMap.MemberType == typeof(string))
			{
				idMap.SetRepresentation(BsonType.ObjectId);
				idMap.SetIdGenerator(new StringObjectIdGenerator());
			}
		}
	}

	public abstract class MongoRepository<TItem> : IQueryable<TItem>, IDisposable
		where TItem : class
	{
		readonly MongoServer _server;
		readonly MongoDatabase _database;

		MongoCollection<TItem> _collection;

		protected MongoCollection<TItem> Collection
		{
			set
			{
				_collection = value;
				_collectionQueryable = value.AsQueryable();
			}
			get { return _collection; }
		}

		IQueryable<TItem> _collectionQueryable;

		static MongoRepository()
		{
			ConventionRegistry.Register("MyConventions", new ConventionPack {new StringObjectIdConvention()}, _ => true);
		}

		protected MongoRepository()
			: this(typeof (TItem).Name.ToLower() + "s")
		{
		}

		protected MongoRepository(string collectionName)
			: this(new MongoUrl(ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString), collectionName)
		{
		}

		protected MongoRepository(MongoUrl connectionUrl, string collectionName)
		{
			_server = new MongoClient(connectionUrl).GetServer();
			_database = _server.GetDatabase(connectionUrl.DatabaseName);
			Collection = _database.GetCollection<TItem>(collectionName);
		}

		public virtual void Dispose()
		{
			_server.Disconnect();
		}

		public IEnumerator<TItem> GetEnumerator()
		{
			return _collectionQueryable.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Expression Expression
		{
			get { return _collectionQueryable.Expression; }
		}

		public Type ElementType
		{
			get { return _collectionQueryable.ElementType; }
		}

		public IQueryProvider Provider
		{
			get { return _collectionQueryable.Provider; }
		}
	}
}