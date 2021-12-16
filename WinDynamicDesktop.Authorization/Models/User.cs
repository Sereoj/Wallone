using System;
using System.Collections.Generic;
using System.Text;

namespace WinDynamicDesktop.Authorization.Models
{
    public class User
    {
        /**
         * [Availability: r - register, l - login]
         * [r] Name - Имя пользователя
         * [r, l] Email - Электронный адрес
         * [r, l] Password - Пароль пользователя
         * [r] ConfirmPassword - Подвержение пароля
         * [r] isPolicy - Согласие на обработку данных
         * [r, l] RememberMe - Запомни меня, автоматический вход в систему
         */
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool isPolicy { get; set; }
        public bool RememberMe { get; set; }
    }
}
