namespace GMP.Users.IntegrationEvent.EventRestorePasswordUser
{
    public class RestorePasswordUserEvent
    {
        public string Id { get; set; }

        public string Mail { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public int TipoUsuario { get; set; }

        public string Token { get; set; }
    }
}
