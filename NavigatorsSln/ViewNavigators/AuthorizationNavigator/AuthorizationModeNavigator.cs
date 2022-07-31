using ViewModelProperties;
using Simplified;

namespace ViewNavigators
{
    public class AuthorizationModeNavigator: ViewModelBase
    {
        private AuthorizationMode _authorizationMode;

        /// <summary>Тип контента авторизации.</summary>
        public AuthorizationMode AuthorizationMode { get => _authorizationMode; set => Set(ref _authorizationMode, value); }

    }
}
