namespace WinDynamicDesktop.Core.Builders
{

    //Создание темы.
    public class ThemeCreateBuilder
    {
        public ThemeCreateBuilder SetName(string value)
        {
            return this;
        }

        public ThemeCreateBuilder SetDescription(string value)
        {
            return this;
        }

        public ThemeCreateBuilder SetCategory(string value)
        {
            return this;
        }

        public ThemeCreateBuilder SetTags(string value)
        {
            return this;
        }

        public ThemeCreateBuilder SetBrand(string value)
        {
            return this;
        }

        public ThemeCreateBuilder Build()
        {
            return this;
        }
    }

    public class ThemeBuilder<TInterface>
    {
        public TInterface Query(TInterface t)
        {
            return t;
        }
    }
}