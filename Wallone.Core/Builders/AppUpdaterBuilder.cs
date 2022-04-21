namespace Wallone.Core.Builders
{
    public class AppUpdaterBuilder : IAppSettings
    {
        public int Compare(string verionCurrent, string verionActual)
        {
            if (string.IsNullOrEmpty(verionCurrent))
            {
                //TODO
            }

            if (string.IsNullOrEmpty(verionActual))
            {
            }

            return verionActual.CompareTo(verionCurrent);
        }
    }
}