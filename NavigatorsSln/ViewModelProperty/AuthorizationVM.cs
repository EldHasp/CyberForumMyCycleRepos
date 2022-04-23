using ViewModelProperties;
using Simplified;

namespace ViewModelProperties
{
    public class AuthorizationVM : ViewModelBase
    {
        private string _login = string.Empty;
        private AuthorizationMode _authorizationMode;
        private bool _isAuthorized;

        /// <summary>Тип контента авторизации.</summary>
        public AuthorizationMode AuthorizationMode { get => _authorizationMode; set => Set(ref _authorizationMode, value); }

        /// <summary>Авторизация проведена.</summary>
        public bool IsAuthorized { get => _isAuthorized; private set => Set(ref _isAuthorized, value); }

        /// <summary>Логин авторизированного пользователя.</summary>
        public string Login { get => _login; private set => Set(ref _login, value); }

        /// <summary>Команда попытки авторизации пользователя.</summary>
        public RelayCommand AuthorizationСommand => GetCommand<LoginPassword>
        (
            lp => // Метод авторизации
            {
                Login = lp.Username;
                IsAuthorized = true;
            },
            lp => !IsAuthorized && // Метод валидации параметра команды
                  !string.IsNullOrWhiteSpace(lp.Username) &&
                  !string.IsNullOrWhiteSpace(lp.Password)
        );
    }
}
