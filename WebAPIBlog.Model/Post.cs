using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPIBlog.Model
{
	public sealed class Post
	{
		public string Id { get; set; }

		[DataType(DataType.Text), Required, StringLength(150, MinimumLength = 10)]
		public string Title { get; set; }

		[DataType(DataType.Html), Required]
		public string Content { get; set; }

		public List<Comment> Comments { get; set; }

		public string Author { get; set; }

		public DateTime Created { get; set; }
	}
}