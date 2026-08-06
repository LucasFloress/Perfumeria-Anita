using PerfumeriaAnita.Models;

namespace PerfumeriaAnita.Services
{
    /// <summary>
    /// Servicio scoped que gestiona la sesión del usuario activo en Blazor Server.
    /// Mantiene el estado en memoria para el circuito de conexión actual.
    /// </summary>
    public class AuthService
    {
        public User? UsuarioActual { get; private set; }
        public bool IsAuthenticated => UsuarioActual != null;

        public event Action? OnChange;

        public void Login(User user)
        {
            UsuarioActual = user;
            NotifyStateChanged();
        }

        public void Logout()
        {
            UsuarioActual = null;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
