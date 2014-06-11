using System.ComponentModel.DataAnnotations;

namespace WebAPIBlog.Model
{
	public sealed class SignInModel
	{
		[Required]
		public string UserName { get; set; }

		[Required, DataType(DataType.Password)]
		public string Password { get; set; }

		public bool IsPersistent { get; set; }
	}
}