namespace GES.Common.Factories
{
    /// <summary>
    /// Define the service factory to create/get the service that's injected
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public interface IServiceFactory<TService> where TService : class
    {
        /// <summary>
        /// Get the service with it key string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TService Get(string key);
    }
}
