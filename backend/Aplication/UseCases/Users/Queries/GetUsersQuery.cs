namespace TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Queries
{
    public record GetUsersQuery
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
