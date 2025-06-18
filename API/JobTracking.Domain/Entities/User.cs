namespace JobTracking.Domain.Entities
{
    public class User : Base.IEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public Enums.UserRole Role { get; set; }
    }
}