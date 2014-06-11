using System.Collections.Concurrent;
using System.Collections.Generic;
using WebAPIBlog.Model;

namespace WebAPIBlog.Domain.Services
{
	public interface IPostService
	{
		IEnumerable<Post> Get(int offset, int count);
		Post GetById(string id);
		IEnumerable<Post> GetByAuthorAll(string author);
		Post GetByTitle(string title);
		bool Add(Post post);
		bool Update(Post post);
		bool Delete(int id);
		bool AddComment(string postId, Comment comment);
	}
}