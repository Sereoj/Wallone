using System.Collections.Generic;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Builders
{
    public interface IPageBulder
    {
        //Отображение рекламы
        public bool IsAds { get; }

        //Валидация формы
        public IPageBulder Validate();

        //Проверка на соединение со сервером
        public IPageBulder HasConnection();

        //Проверка на получие данных
        public IPageBulder HasData();

        //Сборка
        public IPageBulder Build();
        public IPageBulder ShowAds(bool value);
        public IPageBulder Catalog(string root);
        public IPageBulder Page(string id);

        public string GetCatalog();
        public string GetPage();
        public string GetRouter();
        List<Parameter> GetFields();
    }
}