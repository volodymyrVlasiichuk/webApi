using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using WebAPIBlog.Domain.Services;
using WebAPIBlog.Model;

namespace WebAPIBlog.Controllers.API
{
	[RoutePrefix("post")]
	public sealed class PostController : ApiController
	{
		const int _MaxPerRequest = 100;

		readonly IPostService _repository;

		public PostController(IPostService repository)
		{
			_repository = repository;
		}

		[Route]
		public Post Post([FromBody] Post post)
		{
            //skd: allow posting only for registered users
            if (!User.Identity.IsAuthenticated)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

			post.Created = DateTime.UtcNow;
			post.Comments = new List<Comment>();
			post.Content = Startup.Sanitizer.Sanitize(post.Content);
            post.Author = User.Identity.Name; 
                //User.Identity.IsAuthenticated ? User.Identity.Name : "Anonymous";

			_repository.Add(post);
			return post;
		}

		[Route]
		public void Delete(int id)
		{
            /* skd: changed mind, better do not allow deletion of posts
            if (!User.Identity.IsAuthenticated)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            
            var res = _repository.GetById(id);

            if (res == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (res.Author != User.Identity.Name)
                throw new HttpResponseException(HttpStatusCode.Forbidden);

            if (!_repository.Delete(id))
            	throw new HttpResponseException(HttpStatusCode.NotFound);
            //skd: reworked ^^
             */

			// Todo: add identity check
			//if (!_repository.Delete(id))
			//	throw new HttpResponseException(HttpStatusCode.NotFound);
            //skd: removed. better disallow.
		}

		[Route("{id}")]
		public Post Get(string id)
		{
			var res = _repository.GetById(id);
			if (res == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);
			return res;
		}

		[Route]
		public IEnumerable<Post> Get(int offset, int count)
		{
			return _repository.Get(offset, Math.Min(count, _MaxPerRequest));
		}

		[Route]
		public void Put([FromBody] Post post)
		{

		}

		[Route]
		public void PutComment(string postId, [FromBody] Comment comment)
		{
            if (!User.Identity.IsAuthenticated)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            comment.UserId = User.Identity.Name;

			if (!_repository.AddComment(postId, comment))
				throw new HttpResponseException(HttpStatusCode.NotFound);
		}
	}
}