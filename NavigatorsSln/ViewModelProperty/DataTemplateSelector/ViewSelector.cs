using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ViewModelProperties
{
    /// <summary>Селектор Шаблонов данных для значений используемых в View.</summary>
    /// <remarks>В <see cref="AuthorizationVM"/> два свойства определяют тип страницы
    /// представления: <see cref="AuthorizationVM.IsAuthorized"/> и
    /// <see cref="AuthorizationVM.AuthorizationMode"/>. У этих свойств разный 
    /// тип, поэтому по нему можно легко определить какой шаблон требуется вернуть. <br/>
    /// Если же нужно реализовать разные Представления для одних и тех же значений из разных источников,
    /// то тогда нужно для каждого источника реализовать свой Селектор Шаблонов и/или создать в XAML два
    /// разных экземпляра Селектора с разными Шаблонами Данных.<br/>
    /// Или можно, кроме параметра item в методе <see cref="SelectTemplate(object, DependencyObject)"/>,
    /// анализировать ещё и параметр container по типу, имени и другим характеристикам.</remarks>
    public class ViewSelector : DataTemplateSelector
    {
        public DataTemplate? Welcome { get; set; }
        public DataTemplate? SignIn { get; set; }
        public DataTemplate? SignUp { get; set; }

        public DataTemplate? IsAuthorized { get; set; }
        public DataTemplate? NotAuthorized { get; set; }

        public override DataTemplate SelectTemplate(object? item, DependencyObject container)
        {
            DataTemplate? template = null;
            if (item == null)
            { 
                if (container is FrameworkElement element)
                {
                    item = element.Name switch
                    {
                        "authorizedView" => false,
                        "authorizationModeView" => AuthorizationMode.Welcome,
                        _ => null
                    };
                }
            } 
            if (item is AuthorizationMode mode)
            {
                template = mode switch
                {
                    AuthorizationMode.Welcome => Welcome,
                    AuthorizationMode.SignIn => SignIn,
                    AuthorizationMode.SignUp => SignUp,
                    _ => null
                };
            }
            else if (item is bool isAuth)
            {
                template = isAuth switch
                {
                    true => IsAuthorized,
                    _ => NotAuthorized
                };
            }

            Debug.WriteLine($"{item}: {template}");

            if (template != null)
                return template;
            return base.SelectTemplate(item, container);
        }
    }
}
