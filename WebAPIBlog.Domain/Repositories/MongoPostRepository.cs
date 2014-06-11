using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using WebAPIBlog.Domain.Services;
using WebAPIBlog.Model;

namespace WebAPIBlog.Domain.Repositories
{
	public sealed class MongoPostRepository : MongoRepository<Post>, IPostService
	{
		public IEnumerable<Post> Get(int offset, int count)
		{
			return this.OrderByDescending(post => post.Created).Skip(offset).Take(count);
		}

		public Post GetById(string id)
		{
			return this.FirstOrDefault(post => post.Id == id);
		}

		public IEnumerable<Post> GetByAuthorAll(string userId)
		{
			return this.Where(post => post.Id == userId);
		}

		public Post GetByTitle(string title)
		{
			return this.First(post => post.Title == title);
		}

		public IQueryable<Post> All()
		{
			return  this.OrderBy(post => post.Created);
		}

		public bool Add(Post post)
		{
			return Collection.Save(post).Ok;
		}

		public bool Update(Post post)
		{
			throw new NotImplementedException();
		}

		public bool Delete(int id)
		{
			return Collection.Remove(Query.EQ("Id", id)).Ok;
		}

		public bool AddComment(string postId, Comment comment)
		{
			return Collection.Update(Query.EQ("Id", postId), MongoDB.Driver.Builders.Update.Push("Comments", comment.ToBsonDocument())).Ok;
		}
	}
}