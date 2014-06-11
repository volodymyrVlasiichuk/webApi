using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIBlog.Model
{
	public sealed class Comment
	{
		public Comment()
		{
			Created = DateTime.UtcNow;
		}

		public string Id { get; set; }

		public string Content { get; set; }

		public string UserId { get; set; }

		public DateTime Created { get; set; }
	}
}
