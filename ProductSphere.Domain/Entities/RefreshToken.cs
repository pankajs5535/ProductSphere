namespace ProductSphere.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAtUtc { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? RevokedAtUtc { get; set; }

        public bool IsRevoked => RevokedAtUtc.HasValue;

        public Guid UserId { get; set; }

        // Navigation Property
        public User User { get; set; } = null!;
    }
}