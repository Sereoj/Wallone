namespace Wallone.Core.Builders
{

    //Создание темы.

    public class ThemeBuilder<TInterface>
    {
        public TInterface Query(TInterface t)
        {
            return t;
        }
    }
}