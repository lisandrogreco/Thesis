using Amazon.DynamoDBv2.DataModel;

namespace Legacy_Todo.Models
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsLogin { get; set; }
    }

    [DynamoDBTable("Identity")]
    public class User
    {
        [DynamoDBHashKey]
        public string id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public interface IEntityBase
    {
        string Id { get; set; }
    }
}
